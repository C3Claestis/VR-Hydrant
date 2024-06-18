using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class SwitchWaterMode : MonoBehaviourPunCallbacks
{
    [SerializeField] StatusWater pillar;
    [SerializeField] GameObject air_padam;
    [SerializeField] GameObject air_perisai;
    [SerializeField] InputAction switchAction; // InputAction untuk mengontrol switch

    private bool isSwitch = false;

    private void OnEnable()
    {
        // Enable aksi input saat skrip diaktifkan
        switchAction.Enable();
    }

    private void OnDisable()
    {
        // Disable aksi input saat skrip dinonaktifkan
        switchAction.Disable();
    }

    private void Update()
    {
        // Cek apakah aksi input telah dipicu pada setiap frame
        if (switchAction.triggered && pillar.GetActiveHydrant())
        {
            photonView.RPC("UpdateWaterMode", RpcTarget.AllBuffered, !isSwitch); // Panggil metode RPC untuk memperbarui mode air
        }
        if (!pillar.GetActiveHydrant())
        {
            air_perisai.SetActive(false);
            air_padam.SetActive(true);
        }
    }

    [PunRPC]
    public void UpdateWaterMode(bool newMode)
    {
        isSwitch = newMode;
        if (isSwitch)
        {
            air_padam.SetActive(false);
            air_perisai.SetActive(true);
        }
        else
        {
            air_padam.SetActive(true);
            air_perisai.SetActive(false);
        }
    }
}
