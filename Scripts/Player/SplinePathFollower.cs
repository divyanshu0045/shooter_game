using UnityEngine;

/// <summary>
/// Manages the on-rails movement of the player along a predefined path.
/// This implementation uses a simple array of Transforms as waypoints.
/// </summary>
public class SplinePathFollower : MonoBehaviour
{
    [Header("Path Settings")]
    [Tooltip("The list of waypoints that define the path.")]
    public Transform[] waypoints;

    [Tooltip("The speed at which the player moves along the path.")]
    public float moveSpeed = 5f;

    private int currentWaypointIndex = 0;
    private bool isMoving = true;

    void Start()
    {
        if (waypoints.Length > 0)
        {
            // Set the initial position to the first waypoint
            transform.position = waypoints[0].position;
            // Start moving towards the second waypoint if it exists
            currentWaypointIndex = 1;
        }
        else
        {
            isMoving = false;
            Debug.LogWarning("No waypoints assigned to SplinePathFollower. Movement is disabled.");
        }
    }

    void Update()
    {
        if (!isMoving || waypoints.Length == 0)
        {
            return;
        }

        // Move towards the current target waypoint
        MoveAlongPath();
    }

    /// <summary>
    /// Moves the object towards the current waypoint and updates the target when reached.
    /// </summary>
    private void MoveAlongPath()
    {
        if (currentWaypointIndex >= waypoints.Length)
        {
            // Reached the end of the path
            isMoving = false;
            Debug.Log("End of the path reached.");
            // Trigger level complete logic via GameManager
            GameManager.Instance.LevelComplete();
            return;
        }

        // Get the target waypoint's position
        Vector3 targetPosition = waypoints[currentWaypointIndex].position;

        // Move our position a step closer to the target.
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Optional: Smoothly look towards the direction of movement or next point
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);
        }

        // Check if we are close enough to the target waypoint
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Move to the next waypoint
            currentWaypointIndex++;
        }
    }

    /// <summary>
    /// Pauses movement along the path. Useful for scripted events or boss fights.
    /// </summary>
    public void PauseMovement()
    {
        isMoving = false;
    }

    /// <summary>
    /// Resumes movement along the path.
    /// </summary>
    public void ResumeMovement()
    {
        isMoving = true;
    }
}