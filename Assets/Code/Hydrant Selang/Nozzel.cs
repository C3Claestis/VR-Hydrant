using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using Photon.Pun;

public class Nozzel : XRGrabInteractable
{
    PhotonView photonView; //Script photonView gameobject

    #region TwoHandGrab Members

    // List of grab points for two-handed grab
    public List<XRSimpleInteractable> secondhandGrabPoints = new List<XRSimpleInteractable>();
    // Interactor for the second hand
    private XRBaseInteractor secondInteractor;
    // Initial rotation when attached
    private Quaternion attachInitialRotation;
    // Rotation type for two-handed grab
    public enum TwoHandRotationType { None, First, Second };
    public TwoHandRotationType twoHandRotationType;

    #endregion

    #region Members Hydrant Handle

    // Status of two-handed grab activity
    private bool TwoHandActive = false;
    // Reference to the trajectory predictor
    [SerializeField] TrajectoryPredictor trajectoryPredictor;
    // Reference to the hydrant status
    [SerializeField] StatusWater pillar_hydrant;

    #endregion

    #region Unity Methods
    // Called when the object is created
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        // Add listeners for the second grab event
        foreach (var item in secondhandGrabPoints)
        {
            item.onSelectEntered.AddListener(OnSecondGrabHand);
            item.onSelectExited.AddListener(OnSecondGrabRelease);
        }
    }

    #endregion

    #region Ovveride Methods
    // Called during interaction
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        // Process interaction
        if (secondInteractor && selectingInteractor)
        {
            selectingInteractor.attachTransform.rotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position);
        }
        base.ProcessInteractable(updatePhase);
    }

    // Called when the grab interaction starts
    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        Debug.Log("FIRST GRAB ENTER");
        base.OnSelectEntered(interactor);
        // Save initial rotation when attached
        attachInitialRotation = interactor.attachTransform.localRotation;
        photonView.RequestOwnership();
    }

    // Called when the grab interaction ends
    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        Debug.Log("SECOND HAND EXIT");
        base.OnSelectExited(interactor);
        secondInteractor = null;
        // Restore initial rotation
        interactor.attachTransform.localRotation = attachInitialRotation;
    }

    // Checks if the object can be selected by the interactor
    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        // Check if the object is already grabbed by another interactor
        bool isAlreadyGrab = selectingInteractor && !interactor.Equals(selectingInteractor);
        return base.IsSelectableBy(interactor);
    }
    #endregion

    #region Custom Methods
    // Called when the second grab begins
    public void OnSecondGrabHand(XRBaseInteractor interactor)
    {
        Debug.Log("SECOND HAND GRAB");
        secondInteractor = interactor;
        TwoHandActive = true;
    }

    // Called when the second grab ends
    public void OnSecondGrabRelease(XRBaseInteractor interactor)
    {
        Debug.Log("SECOND HAND RELEASE");
        interactor = null;
        TwoHandActive = false;
    }
    #endregion

    #region Getter 
    // Getter for the status of two-handed grab activity
    public bool GetActive() => TwoHandActive;
    #endregion

    #region RPC Methods
    [PunRPC]
    private void RPCWoosh(bool value)
    {
        trajectoryPredictor.SetIsReadyWoosh(value);
    }
    #endregion
}
