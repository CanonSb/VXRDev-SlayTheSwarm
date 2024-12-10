using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;


// using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class EnemyVitals : MonoBehaviour
{
    public List<GameObject> vitalObjects;
    public NavMeshAgent agent;
    public GameObject highestParentObj;

    private List<GameObject> _enemyParts;
    private List<MeshCollider> _enemyColliders;
    private EnemyMovement _movement;
    private GoblinSounds _goblinSounds;

    void Start()
    {
        // Set variables
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        _enemyParts = new List<GameObject>();
        _enemyColliders = new List<MeshCollider>();
        _movement = GetComponent<EnemyMovement>();
        _goblinSounds = GetComponent<GoblinSounds>();

        // Startup Functions
        AddAllChildrenAsEnemyParts(highestParentObj.transform);
        AddAllPartColliders(_enemyParts);
        StartCoroutine(CheckTargetDistance());

        // Add event listeners for each GameObject in the vitals list
        foreach (GameObject obj in vitalObjects)
        {
            if (obj != null)
            {
                // Add a listener to check when the object is destroyed
                obj.AddComponent<DestroyListener>().Initialize(OnGameObjectDestroyed);
            }
        }
    }

    // TODO: Disable goblin audio sources as well upon death
    // Callback function when any monitored GameObject is destroyed
    public void OnGameObjectDestroyed(GameObject destroyedObject)
    {
        if (destroyedObject == null) return;
        // Remove the listeners attached to the children before destroying the parent
        foreach (GameObject obj in vitalObjects)
        {
            if (obj != null)
            {
                Destroy(obj.GetComponent<DestroyListener>());
            }
        }
        // Dsiable agent, movement, and goblin sounds script
        if (agent != null && agent.enabled) agent.enabled = false;
        if (_movement != null && _movement.enabled) _movement.enabled = false;
        if (_goblinSounds != null && _goblinSounds.enabled) _goblinSounds.enabled = false;
        // Begin destroying remaining body parts
        DestroyRemainingParts();
        // Destroy this object containing everything after some time
        Invoke("DestroyParent", 10f);
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
        _enemyParts.Add(parent.gameObject);
        // Iterate through all child objects of the current parent transform
        foreach (Transform child in parent)
        {
            AddAllChildrenAsEnemyParts(child);
        }
    }

    private void DestroyRemainingParts()
    {
        foreach (GameObject part in _enemyParts)
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

    private void DestroyParent()
    {
        Destroy(gameObject);
    }



    // #region COLLIDER STUFF

    private void AddAllPartColliders(List<GameObject> parts)
    {
        foreach (GameObject part in parts)
        {
            MeshCollider coll = null;
            if (part != null) coll = part.GetComponent<MeshCollider>();
            if (coll != null) _enemyColliders.Add(coll);
        }
    }

    public void SetAllCollidersState(bool state, List<MeshCollider> colliders)
    {
        foreach (MeshCollider coll in colliders)
        {
            if (coll != null) coll.enabled = state;
        }
    }

    // Only enable enemy colliders if they are close to the player to try and reduce lag
    private IEnumerator CheckTargetDistance()
    {
        while (true)
        {
            if (gameObject == null) break;
            // Attempt to update collider states if agent is still enabled and has destination
            // TODO: maybe change this to be based off distance from sword rather than agent.destination (player)
            //       and have a better way to check if goblin is marked for death
            if (agent.destination != null && agent.enabled)
            {
                float distanceToTarget = Vector3.Distance(transform.position, agent.destination);
                if (distanceToTarget < 2) SetAllCollidersState(true, _enemyColliders);
                else SetAllCollidersState(false, _enemyColliders);                
            }
            yield return new WaitForSeconds(0.1f);            
        } 
    }

    // #endregion
}
