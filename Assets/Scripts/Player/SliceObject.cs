using System.Collections;
using UnityEngine;
using EzySlice;

using Unity.VisualScripting;


public class SliceObject : MonoBehaviour
{
    public Transform startSlicePoint, endSlicePoint;
    public VelocityEstimator velocityEstimator;
    public LayerMask sliceableLayer;

    public Material crossSectionMat;
    public float swingPowerThreshold = 2f;
    public float cutForce = 2000f;
    public float despawnTime = 3f;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer);
        if (hasHit)
        {
            GameObject target = hit.transform.gameObject;
            Slice(target);
        }
    }

    public void Slice(GameObject target)
    {
        // Calculate velocity, and don't slice if its below threshold
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        if (velocity.magnitude < swingPowerThreshold) return;

        // Calculate slicing plane
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        planeNormal.Normalize();

        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal);
        if (hull != null)
        {
            target.GetComponent<Collider>().enabled = false;

            // Create object halves upon slice
            GameObject upperHull = hull.CreateUpperHull(target, crossSectionMat);
            SetupSlicedComponent(upperHull);
            GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMat);
            SetupSlicedComponent(lowerHull);

            // Destroy original object
            UnchildAllChildrenAndDestroy(target);
        }
    }

    public void SetupSlicedComponent(GameObject slicedObject)
    {
        // Add collider and rb
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;
        // Add force after slice
        rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
        // Make object sliceable again after time
        StartCoroutine(MakeSliceable(slicedObject, 0.5f));
        // Despawn object after time
        StartCoroutine(ShrinkThenDestroy(slicedObject));
    }

    void UnchildAllChildrenAndDestroy(GameObject parentObject) {
        // Iterate through all child objects of the parent
        while (parentObject.transform.childCount > 0) {
            Transform child = parentObject.transform.GetChild(0);
            // Add rb and force
            if (!child.GetComponent<Rigidbody>()) child.AddComponent<Rigidbody>();
            Rigidbody rb = child.GetComponent<Rigidbody>();
            rb.AddExplosionForce(cutForce, child.transform.position, 1);
            // Unparent the child (set parent to null)
            child.SetParent(null);
            // Despawn object after time
            StartCoroutine(ShrinkThenDestroy(child.gameObject));
        }

        // Destroy the parent object after unparenting its children
        Destroy(parentObject);
    }


    private IEnumerator MakeSliceable(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.layer = LayerMask.NameToLayer("Sliceable");
    }

    private IEnumerator ShrinkThenDestroy(GameObject obj)
    {
        // Tag object for destruction
        obj.tag = "WillBeDestroyed";

        yield return new WaitForSeconds(despawnTime);
        if (obj == null) yield break;

        // Set layer to default so obj can't be sliced while shrinking
        obj.layer = LayerMask.NameToLayer("NotSliceable");

        Vector3 startingScale = obj.transform.localScale;
        float elapsedTime = 0f;
        float duration = 2f;

        while (obj && elapsedTime < duration)
        {
           obj.transform.localScale = Vector3.Lerp(startingScale, Vector3.zero, elapsedTime / duration);
           elapsedTime += Time.deltaTime;
           yield return null;
        }
        if (obj != null) Destroy(obj);
    }

}
