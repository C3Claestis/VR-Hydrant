using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
public class TwoHandSelang : XRGrabInteractable
{
    #region TwoHandGrab Members

    // List of second hand grab points for two-handed interaction
    public List<XRSimpleInteractable> secondhandGrabPoints = new List<XRSimpleInteractable>();

    // Reference to the second interactor (hand) in two-handed interaction
    private XRBaseInteractor secondInteractor;

    // Initial rotation of the attached object when it's grabbed
    private Quaternion attachInitialRotation;

    // Enum to specify the type of rotation for two-handed interaction
    public enum TwoHandRotationType { None, First, Second };
    public TwoHandRotationType twoHandRotationType;

    #endregion

    // Boolean flag indicating whether the object is dropped
    private bool isDrop;
    private PhotonView photon;
    // Start is called before the first frame update
    void Start()
    {
        photon = GetComponent<PhotonView>();
        // Add event listeners for each second hand grab point
        foreach (var item in secondhandGrabPoints)
        {
            item.onSelectEntered.AddListener(OnSecondGrabHand);
            item.onSelectExited.AddListener(OnSecondGrabRelease);
        }
    }

    // ProcessInteractable is called by the interaction system every frame
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (secondInteractor && selectingInteractor)
        {
            // Set the rotation of the attaching hand based on two-hand rotation type
            selectingInteractor.attachTransform.rotation = GetTwoHandRotation();
        }
        base.ProcessInteractable(updatePhase);
    }

    // Get the target rotation for two-handed interaction
    private Quaternion GetTwoHandRotation()
    {
        Quaternion targetRotation;
        if (twoHandRotationType == TwoHandRotationType.None)
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position);
        }
        else if (twoHandRotationType == TwoHandRotationType.First)
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, selectingInteractor.attachTransform.up);
        }
        else
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, secondInteractor.attachTransform.up);
        }
        return targetRotation;
    }

    // Event handler when second hand grabs the object
    public void OnSecondGrabHand(XRBaseInteractor interactor)
    {
        Debug.Log("SECOND HAND GRAB");
        secondInteractor = interactor;
    }

    // Event handler when second hand releases the object
    public void OnSecondGrabRelease(XRBaseInteractor interactor)
    {
        Debug.Log("SECOND HAND RELEASE");
        interactor = null;
    }

    // Event handler when the object is grabbed by the first hand
    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        Debug.Log("FIRST GRAB ENTER");
        base.OnSelectEntered(interactor);
        attachInitialRotation = interactor.attachTransform.localRotation;
        isDrop = false;
        photon.RequestOwnership();
    }

    // Event handler when the object is released by the first hand
    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        Debug.Log("SECOND HAND EXIT");
        base.OnSelectExited(interactor);
        secondInteractor = null;
        interactor.attachTransform.localRotation = attachInitialRotation;
        isDrop = true;
    }

    // Determine whether the object can be grabbed by the interactor
    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        bool isAlreadyGrab = selectingInteractor && !interactor.Equals(selectingInteractor);
        return base.IsSelectableBy(interactor);
    }

    // Get the drop status of the object
    public bool GetIsDrop() => isDrop;
}
