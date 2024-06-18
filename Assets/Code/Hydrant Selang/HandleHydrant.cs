using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class HandleHydrant : MonoBehaviourPunCallbacks
{
    #region Serialized Fields

    [Header("XR Interaction")]
    [SerializeField] private XRSocketInteractor socketInteractor; // Component to handle XR socket interactions

    #endregion

    #region Unity Methods

    private void Start()
    {
        // Add a listener for the OnSelectEntered() event of the XRSocketInteractor
        socketInteractor.onSelectEntered.AddListener(OnSelectEntered);
    }

    #endregion

    #region Private Methods

    private void OnSelectEntered(XRBaseInteractable interactable)
    {
        // Ensure the object placed in the interactor is the object you want to destroy
        GameObject objectToDestroy = interactable.gameObject;

        // Get the outermost parent of the object
        GameObject outermostParent = GetOutermostParent(objectToDestroy);

        // Check if the object has a parent
        if (objectToDestroy.transform.parent != null)
        {
            // Destroy the outermost parent of the object on all clients using RPC
            PhotonView outermostParentPhotonView = outermostParent.GetComponent<PhotonView>();
            if (outermostParentPhotonView != null)
            {
                photonView.RPC("DestroyObject", RpcTarget.All, outermostParentPhotonView.ViewID);
            }
            else
            {
                Debug.LogWarning("Outermost parent does not have a PhotonView component.");
            }
        }
        else
        {
            // If the object does not have a parent, destroy the object itself on all clients using RPC
            PhotonView objectPhotonView = objectToDestroy.GetComponent<PhotonView>();
            if (objectPhotonView != null)
            {
                photonView.RPC("DestroyObject", RpcTarget.All, objectPhotonView.ViewID);
            }
            else
            {
                Debug.LogWarning("Object to destroy does not have a PhotonView component.");
            }
        }
    }

    // RPC method to destroy an object on all clients
    [PunRPC]
    private void DestroyObject(int viewID)
    {
        // Get the GameObject based on the PhotonView ID
        PhotonView objPhotonView = PhotonView.Find(viewID);
        if (objPhotonView != null)
        {
            // Destroy the object
            Destroy(objPhotonView.gameObject);
        }
        else
        {
            Debug.LogWarning($"PhotonView with ID {viewID} not found.");
        }
    }

    // Method to get the outermost parent of a GameObject
    private GameObject GetOutermostParent(GameObject childObject)
    {
        Transform parent = childObject.transform.parent;
        while (parent != null)
        {
            if (parent.parent == null)
            {
                return parent.gameObject;
            }
            parent = parent.parent;
        }
        // Return the object itself if it has no parent
        return childObject;
    }

    #endregion

    #region TODO

    // TODO: Add additional logging for debugging purposes.
    // TODO: Consider adding more error handling for null references.
    // TODO: Optimize performance by reducing unnecessary checks.

    #endregion
}
