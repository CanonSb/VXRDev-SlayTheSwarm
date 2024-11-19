using System.Collections;
using UnityEngine;

[ExecuteAlways] // This makes the script run in the editor
public class ColliderVisualizer : MonoBehaviour
{
    public Color gizmoColor = Color.green; // Color of the Gizmo

    private void OnDrawGizmos()
    {
        // Set the color for the Gizmo
        Gizmos.color = gizmoColor;

        // Draw a wireframe box or shape matching the collider
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
        }
    }
}
