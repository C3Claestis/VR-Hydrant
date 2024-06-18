using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
public class NetworkXRSimple : XRSimpleInteractable
{
    private PhotonView photon;
    // Start is called before the first frame update
    void Start()
    {
        photon = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        photon.RequestOwnership();
        base.OnSelectEntered(interactor);
    }
}
