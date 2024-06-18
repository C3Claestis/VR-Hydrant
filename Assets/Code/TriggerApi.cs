using UnityEngine;
using Photon.Pun;

public class TriggerApi : MonoBehaviourPun
{
    #region Serialized Fields

    [SerializeField] private GameObject[] api_child; // Array of child GameObjects

    #endregion

    #region Unity Methods

    private void Update()
    {
        // Check if the api_child array is null or empty
        if (api_child == null || api_child.Length == 0)
        {
            // Destroy this GameObject across the network
            PhotonNetwork.Destroy(gameObject);
            return;
        }

        // Check if all elements in api_child are null
        bool allNull = true;
        foreach (GameObject child in api_child)
        {
            if (child != null)
            {
                allNull = false;
                break;
            }
        }

        if (allNull)
        {
            // Destroy this GameObject across the network
            PhotonNetwork.Destroy(gameObject);
        }
    }

    #endregion

    #region TODO

    // TODO: Add more detailed logging for debugging purposes.
    // TODO: Consider adding a method to dynamically update api_child array if children change during runtime.
    // TODO: Implement a more efficient way to check the null status of child elements if the array is large.

    #endregion
}
