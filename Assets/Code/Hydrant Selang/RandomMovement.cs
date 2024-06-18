using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    #region Variables

    // Speed of object movement
    public float moveSpeed = 5f;
    // Time interval to change direction
    public float changeDirectionTime = 2f;
    private Vector3 randomDirection;
    private float timeSinceChange = 0f;
    Nozzel nozzel;

    [SerializeField] StatusWater pillarStatus;
    // Minimum position bounds
    Vector3 minBounds = new Vector3(0, 0, -Mathf.Infinity);

    #endregion

    #region Unity Callbacks

    void Start()
    {
        // Initialize random direction
        nozzel = GetComponent<Nozzel>();
        randomDirection = GetRandomDirection();
    }

    void Update()
    {
        if (pillarStatus.GetActiveHydrant() && !nozzel.GetActive())
        {
            // Update time since last direction change
            timeSinceChange += Time.deltaTime;

            // If time since last change exceeds the limit, change direction
            if (timeSinceChange >= changeDirectionTime)
            {
                randomDirection = GetRandomDirection();
                timeSinceChange = 0f;
            }

            // Move object in random direction
            transform.Translate(randomDirection * moveSpeed * Time.deltaTime);

            // Limit object position
            Vector3 newPosition = transform.position;
            newPosition.x = Mathf.Max(newPosition.x, minBounds.x);
            newPosition.y = Mathf.Max(newPosition.y, minBounds.y);

            transform.position = newPosition;
        }
    }

    #endregion

    #region Utility Methods

    // Function to get a random direction
    Vector3 GetRandomDirection()
    {
        // Generate random direction in 3D space
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        Vector3 randomDir = new Vector3(randomX, randomY, randomZ);

        // Normalize direction to have length 1
        return randomDir.normalized;
    }

    #endregion
}
