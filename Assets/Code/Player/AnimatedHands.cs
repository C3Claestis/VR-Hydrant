using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using CommonUsages = UnityEngine.XR.CommonUsages;
using InputDevice = UnityEngine.XR.InputDevice;

public class AnimatedHands : MonoBehaviour
{
    [SerializeField] InputActionProperty _pinchAnim;
    [SerializeField] InputActionProperty _gripAnim;

    private InputDevice inputDevice;
    private bool isPrimaryButtonDown = false;
    private Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();

        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);

        foreach (var device in devices)
        {
            Debug.Log("Device name: " + device.name + ", Device role: " + device.role.ToString());
            inputDevice = device;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float _triggerValue = _pinchAnim.action.ReadValue<float>();
        _anim.SetFloat("Trigger", _triggerValue);

        float _gripValue = _gripAnim.action.ReadValue<float>();
        _anim.SetFloat("Grip", _gripValue);

        // Memeriksa penekanan tombol utama pada kontroler kanan
        if (inputDevice.isValid)
        {
            if (inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out isPrimaryButtonDown) && isPrimaryButtonDown)
            {
                Debug.Log("Primary button on the right controller is pressed.");
                // Tambahkan aksi yang diinginkan di sini
            }
        }
    }
}
