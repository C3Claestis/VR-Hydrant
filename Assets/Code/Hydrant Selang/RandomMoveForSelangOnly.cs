using System.Collections;
using UnityEngine;
using Photon.Pun;

public class RandomMoveForSelangOnly : MonoBehaviourPun
{
    #region Variables

    // Speed of object movement
    public float moveSpeed = 5f;
    // Time interval to change direction
    public float changeDirectionTime = 2f;
    private Vector3 randomDirection;
    private float timeSinceChange = 0f;

    [SerializeField] StatusWater pillarStatus;
    [SerializeField] GameObject water;
    [SerializeField] GameObject water_pillar;

    [SerializeField] ScoreManager scoreManager;

    // Minimum movement bounds
    Vector3 minBounds = new Vector3(0, 0, -Mathf.Infinity);

    #endregion

    #region Unity Callbacks

    void Start()
    {
        // Initialize random direction and globally deactivate water pillar
        randomDirection = GetRandomDirection();
        photonView.RPC("PillarWater", RpcTarget.All);
    }

    void Update()
    {
        if (pillarStatus.GetActiveHydrant())
        {
            // Activate water if hydrant is active
            water.SetActive(true);

            timeSinceChange += Time.deltaTime;

            if (timeSinceChange >= changeDirectionTime)
            {
                // Change random direction after a certain time
                randomDirection = GetRandomDirection();
                timeSinceChange = 0f;
            }

            // Move object according to random direction
            transform.Translate(randomDirection * moveSpeed * Time.deltaTime);

            // Limit movement within specified area
            Vector3 newPosition = transform.position;
            newPosition.x = Mathf.Max(newPosition.x, minBounds.x);
            newPosition.y = Mathf.Max(newPosition.y, minBounds.y);

            transform.position = newPosition;

            // Decrease score when selang is active
            scoreManager.DecreaseScoreSelang(20, true);
        }
        else
        {
            // Deactivate water if hydrant is not active
            water.SetActive(false);
        }
    }

    #endregion

    #region Utility Methods

    // Get random direction for object movement
    Vector3 GetRandomDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        Vector3 randomDir = new Vector3(randomX, randomY, randomZ);

        return randomDir.normalized;
    }

    // Method to update hydrant status and send RPC to all players
    public void UpdateAndSyncHydrantStatus(bool hydrantStatus)
    {
        // Set hydrant status on local object
        pillarStatus.SetActiveHydrant(hydrantStatus);

        // If hydrant is active, start or continue movement
        if (hydrantStatus)
        {
            enabled = true;
        }
        else // If hydrant is not active, stop movement
        {
            enabled = false;
        }

        // Call RPC to update hydrant status on all players
        photonView.RPC("UpdateHydrantStatus", RpcTarget.All, hydrantStatus);
    }

    // RPC to update hydrant status on all players
    [PunRPC]
    void UpdateHydrantStatus(bool hydrantStatus)
    {
        // Set hydrant status according to received value
        pillarStatus.SetActiveHydrant(hydrantStatus);

        // If hydrant is active, start or continue movement
        if (hydrantStatus)
        {
            enabled = true;
        }
        else // If hydrant is not active, stop movement
        {
            enabled = false;
        }
    }

    // RPC to globally deactivate water pillar
    [PunRPC]
    void PillarWater()
    {
        water_pillar.SetActive(false);
    }

    #endregion
}
