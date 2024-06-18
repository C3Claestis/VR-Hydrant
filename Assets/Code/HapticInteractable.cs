using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
public class Haptic
{
    #region Serialized Fields

    [Range(0, 1)]
    [SerializeField] private float intensity; // Intensity of the haptic feedback
    [SerializeField] private float duration;  // Duration of the haptic feedback

    #endregion

    #region Haptic Methods

    /// <summary>
    /// Triggers haptic feedback using the provided event arguments.
    /// </summary>
    /// <param name="eventArgs">Event arguments containing the interactor object.</param>
    public void TriggerHaptic(BaseInteractionEventArgs eventArgs)
    {
        if (eventArgs.interactorObject is XRBaseControllerInteractor controllerInteractor)
        {
            TriggerHaptic(controllerInteractor.xrController);
        }
    }

    /// <summary>
    /// Triggers haptic feedback on the specified controller.
    /// </summary>
    /// <param name="controller">The controller to trigger the haptic feedback on.</param>
    public void TriggerHaptic(XRBaseController controller)
    {
        if (intensity > 0)
        {
            controller.SendHapticImpulse(intensity, duration);
        }
    }

    #endregion
}

public class HapticInteractable : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private Haptic hapticOnActivated;    // Haptic feedback for activation
    [SerializeField] private Haptic hapticHoverEntered;   // Haptic feedback for hover enter
    [SerializeField] private Haptic hapticHoverExited;    // Haptic feedback for hover exit
    [SerializeField] private Haptic hapticSelectEntered;  // Haptic feedback for select enter
    [SerializeField] private Haptic hapticSelectExited;   // Haptic feedback for select exit

    #endregion

    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        // Get the XRBaseInteractable component
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();

        // Add listeners to the interactable's events for haptic feedback
        interactable.activated.AddListener(hapticOnActivated.TriggerHaptic);
        interactable.hoverEntered.AddListener(hapticHoverEntered.TriggerHaptic);
        interactable.hoverExited.AddListener(hapticHoverExited.TriggerHaptic);
        interactable.selectEntered.AddListener(hapticSelectEntered.TriggerHaptic);
        interactable.selectExited.AddListener(hapticSelectExited.TriggerHaptic);
    }

    #endregion

    #region TODO

    // TODO: Add more haptic feedback configurations for other interaction events if necessary.
    // TODO: Implement logic to dynamically adjust haptic feedback intensity and duration based on specific conditions or interactions.
    // TODO: Add visual or audio cues in addition to haptic feedback for a more immersive experience.

    #endregion
}
