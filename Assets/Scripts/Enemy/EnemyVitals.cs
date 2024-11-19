using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class EnemyVitals : MonoBehaviour
{
    public List<GameObject> vitalObjects;
    public NavMeshAgent agent;
    public GameObject highestParentObj;

    public GameObject destroyable;
    private List<GameObject> enemyParts;

    // Start is called before the first frame update
    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        enemyParts = new List<GameObject>();
        AddAllChildrenAsEnemyParts(highestParentObj.transform);

        // Add event listeners for each GameObject in the list
        foreach (GameObject obj in vitalObjects)
        {
            if (obj != null)
            {
                // Add a listener to check when the object is destroyed
                obj.AddComponent<DestroyListener>().Initialize(OnGameObjectDestroyed);
            }
        }
    }

    void Update()
    {
        // Check if the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(destroyable);
        }
    }

    // Callback function when any monitored GameObject is destroyed
    private void OnGameObjectDestroyed(GameObject destroyedObject)
    {
        if (destroyedObject == null) return;
        // Disable the specified component
        if (agent != null && agent.enabled) agent.enabled = false;
        DestroyRemainingParts();
    }

    // Custom listener class to detect destruction of GameObjects
    private class DestroyListener : MonoBehaviour
    {
        private System.Action<GameObject> onDestroyedCallback;

        public void Initialize(System.Action<GameObject> callback)
        {
            onDestroyedCallback = callback;
        }

        void OnDestroy()
        {
            // Check if the gameObject is still valid before invoking the callback
            onDestroyedCallback?.Invoke(gameObject);
        }
    }



    // Function to unparent and destroy all children and their descendants recursively
    private void AddAllChildrenAsEnemyParts(Transform parent)
    {
        enemyParts.Add(parent.gameObject);
        // Iterate through all child objects of the current parent transform
        foreach (Transform child in parent)
        {
            AddAllChildrenAsEnemyParts(child);
        }
    }

    private void DestroyRemainingParts()
    {
        foreach (GameObject part in enemyParts)
        {
            if (part == null || !part.activeInHierarchy) continue;
            if (part.tag != "WillBeDestroyed") 
            {
                if (!part.GetComponent<Rigidbody>()) part.AddComponent<Rigidbody>();
                Rigidbody rb = part.GetComponent<Rigidbody>();
                rb.AddExplosionForce(100f, part.transform.position, 1);
                // Unparent the child (set parent to null)
                part.transform.SetParent(null);
                StartCoroutine(ShrinkThenDestroy(part));
            }
        }
    }

    private IEnumerator ShrinkThenDestroy(GameObject obj)
    {
        if (obj == null) yield break;
        // Tag object for destruction
        obj.tag = "WillBeDestroyed";
        yield return new WaitForSeconds(3f);
        if (obj == null) yield break;
        
        // Set layer to default so obj can't be sliced while shrinking
        obj.layer = LayerMask.NameToLayer("NotSliceable");

        Vector3 startingScale = obj.transform.localScale;
        float elapsedTime = 0f;
        float duration = 2f;

        while (obj != null && elapsedTime < duration)
        {
           obj.transform.localScale = Vector3.Lerp(startingScale, Vector3.zero, elapsedTime / duration);
           elapsedTime += Time.deltaTime;
           yield return null;
        }
        if (obj != null) Destroy(obj);
    }
}
