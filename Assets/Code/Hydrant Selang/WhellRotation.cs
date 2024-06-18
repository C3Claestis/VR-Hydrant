using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
public class WhellRotation : XRBaseInteractable
{
    [SerializeField] private Transform wheelTransform;
    public UnityEvent<float> OnWhellRotated;
    private float currentAngle = 0.0f;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        currentAngle = FindWhellAngle();
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        currentAngle = FindWhellAngle();
    }
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if(updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (isSelected)
            {
                RotateWhell();
            }
        }
    }
    void RotateWhell()
    {
        float totalAngle = FindWhellAngle();

        float angleDif = currentAngle - totalAngle;
        wheelTransform.Rotate(transform.forward, -angleDif);

        currentAngle = totalAngle;
        OnWhellRotated.Invoke(angleDif);
    }
    private float FindWhellAngle()
    {
        float totalAngle = 0;

        foreach(IXRSelectInteractor interactor in interactorsSelecting)
        {
            Vector2 dir = FindLocalPoint(interactor.transform.position);
            totalAngle += ConvertToAngle(dir) * FindRotationSen();
        }
        return totalAngle;
    }
    private Vector2 FindLocalPoint(Vector3 pos)
    {
        return transform.InverseTransformPoint(pos).normalized;
    }
    private float ConvertToAngle(Vector2 dir)
    {
        return Vector2.SignedAngle(transform.up, dir);
    }
    private float FindRotationSen()
    {
        return 1.0f / interactorsSelecting.Count;
    }
}
