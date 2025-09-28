using UnityEngine;

/// <summary>
/// A helper script to visualize an object's collider in the Unity Editor.
/// Attach this to any GameObject with a BoxCollider, SphereCollider, or CapsuleCollider.
/// </summary>
[RequireComponent(typeof(Collider))]
public class HitboxVisualizer : MonoBehaviour
{
    [Tooltip("The color of the hitbox gizmo in the editor.")]
    public Color gizmoColor = new Color(1, 0, 0, 0.5f); // Semi-transparent red by default

    /// <summary>
    /// This is a special Unity method that is called only in the Editor to draw gizmos.
    /// </summary>
    private void OnDrawGizmos()
    {
        // Set the color for the gizmo
        Gizmos.color = gizmoColor;

        // Try to get the collider component
        Collider col = GetComponent<Collider>();

        // Check the type of collider and draw the appropriate shape
        if (col is BoxCollider boxCollider)
        {
            // For a box collider, we need to account for the object's rotation
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawCube(boxCollider.center, boxCollider.size);
        }
        else if (col is SphereCollider sphereCollider)
        {
            // For a sphere, we just need the position and radius
            Gizmos.DrawSphere(transform.position + sphereCollider.center, sphereCollider.radius);
        }
        else if (col is CapsuleCollider capsuleCollider)
        {
            // Capsule is more complex, but we can approximate it for visualization
            // This is a simplified representation
            Vector3 center = transform.position + capsuleCollider.center;
            float radius = capsuleCollider.radius;
            float height = capsuleCollider.height;

            // Draw two spheres for the ends and a cube for the body
            Vector3 top = center + transform.up * (height / 2 - radius);
            Vector3 bottom = center - transform.up * (height / 2 - radius);
            Gizmos.DrawSphere(top, radius);
            Gizmos.DrawSphere(bottom, radius);
            // This doesn't draw the cylinder part, but the spheres are often enough for placement debugging.
        }
    }
}