using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
public class NetworkXRGrabInteractible : XRGrabInteractable
{
    private PhotonView photon;
    // Start is called before the first frame update
    void Start()
    {
        photon = GetComponent<PhotonView>();
    }
    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        photon.RequestOwnership();
        base.OnSelectEntered(interactor);
    }
}
