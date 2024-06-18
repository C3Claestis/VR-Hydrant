using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
public class TwistHydrant : MonoBehaviour
{
    [SerializeField] XRLever lever;
    [SerializeField] float value_true;
    [SerializeField] float value_false;

    // Update is called once per frame
    void Update()
    {
        if (lever.value)
        {
            transform.localRotation = Quaternion.Euler(0, 0, value_true);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, value_false);
        }
    }
}