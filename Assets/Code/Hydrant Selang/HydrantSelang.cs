using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HydrantSelang : MonoBehaviour
{
    #region Private Fields

    private XRGrabInteractable grabInteractable; // Component to handle XR grab interactions
    private Transform originalParent; // Original parent of this object
    private Vector3 originalLocalScale; // Original local scale of this object relative to its parent

    #endregion

    #region Unity Methods

    private void Start()
    {
        // Get the XRGrabInteractable component on this object
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Store the original parent of this object
        originalParent = transform.parent;

        // Store the original local scale of this object relative to its parent
        originalLocalScale = transform.localScale;

        // Add a listener for the onSelectEnter event
        grabInteractable.onSelectEntered.AddListener(OnSelectEnter);
    }

    #endregion

    #region Private Methods

    private void OnSelectEnter(XRBaseInteractor interactor)
    {
        // After the object is grabbed, set its parent back to the original parent
        transform.parent = originalParent;

        // Calculate the local scale relative to the original parent
        Vector3 newLocalScale = Vector3.Scale(transform.localScale, originalLocalScale);

        // Set the object's local scale back to the relative scale
        transform.localScale = newLocalScale;
    }

    #endregion

    #region TODO

    // TODO: Add additional functionality if needed.
    // TODO: Consider adding error handling for null references.
    // TODO: Optimize performance by reducing unnecessary calculations.

    #endregion
}
