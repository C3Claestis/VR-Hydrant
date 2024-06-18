using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TiriskanSelang : MonoBehaviourPun
{
    [SerializeField] NetworkXRGrabInteractible[] networkXRGrabs;

    // Start is called before the first frame update
    void Start()
    {
        // Disable all NetworkXRGrabInteractible components except the first one
        for (int i = 1; i < networkXRGrabs.Length; i++)
        {
            networkXRGrabs[i].enabled = false;
        }
    }

    // Enable the next NetworkXRGrabInteractible component in the sequence
    public void GrabInteractForSelangTirisan(NetworkXRGrabInteractible nextpoint)
    {
        int index = System.Array.IndexOf(networkXRGrabs, nextpoint);
        if (index >= 0)
        {
            photonView.RPC("RPCGrabInteractForSelangTirisan", RpcTarget.All, index);
        }
    }

    // Disable water particle objects
    public void DisableWater(GameObject waterObject)
    {
        PhotonView waterPhotonView = waterObject.GetComponent<PhotonView>();
        if (waterPhotonView != null)
        {
            photonView.RPC("RPCDisableWater", RpcTarget.All, waterPhotonView.ViewID);
        }
    }

    #region Photon RPC Methods

    [PunRPC]
    private void RPCGrabInteractForSelangTirisan(int nextpointIndex)
    {
        if (nextpointIndex >= 0 && nextpointIndex < networkXRGrabs.Length)
        {
            // Enable the specified NetworkXRGrabInteractible component
            networkXRGrabs[nextpointIndex].enabled = true;
        }
    }

    [PunRPC]
    private void RPCDisableWater(int waterViewID)
    {
        PhotonView waterPhotonView = PhotonView.Find(waterViewID);
        if (waterPhotonView != null)
        {
            // Disable and destroy the water object
            waterPhotonView.gameObject.SetActive(false);
            Debug.Log($"Disabling and Destroying water object with PhotonView ID: {waterViewID}");
            Destroy(waterPhotonView.gameObject);
        }
        else
        {
            Debug.LogWarning($"PhotonView with ID {waterViewID} not found!");
        }
    }

    #endregion
}
