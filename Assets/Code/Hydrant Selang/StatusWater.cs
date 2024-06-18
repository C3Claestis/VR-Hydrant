using System;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using Photon.Pun;

public class StatusWater : MonoBehaviourPunCallbacks
{
    #region Condition Of Status Water
    [SerializeField] private bool isAssesmen = false;
    [SerializeField] private bool waterOnKanan;
    [SerializeField] private bool waterOnKiri;
    [SerializeField] private bool isActive;
    #endregion

    #region Members For 3D Object
    [Header("Knob Pillar")]
    [SerializeField] XRKnob xRKnob;

    [Header("Lever Pillar")]
    [SerializeField] XRLever leverLeft;
    [SerializeField] XRLever leverRight;

    [Header("Selang GameObject With Script Trajectory")]
    [SerializeField] TrajectoryPredictor selangKanan;
    [SerializeField] TrajectoryPredictor selangKiri;

    [Header("Selang Error Without Nozzle For Condition")]
    [SerializeField] GameObject error_selang_kiri;
    [SerializeField] GameObject error_selang_kanan;

    [Header("Selang Error With Nozzle For Condition")]
    [SerializeField] GameObject error_selang_kiri_nozzel;
    [SerializeField] GameObject error_selang_kanan_nozzel;

    [Header("Water Particle Error")]
    [SerializeField] GameObject water_kanan;
    [SerializeField] GameObject water_kiri;
    #endregion

    #region GETTER SETTER
    public void SetWaterKanan(bool onoff) => waterOnKanan = onoff;
    public void SetWaterKiri(bool onoff) => waterOnKiri = onoff;
    public void SetActiveHydrant(bool isAktif) => isActive = isAktif;
    public bool GetWaterStatusKanan() => waterOnKanan;
    public bool GetWaterStatusKiri() => waterOnKiri;
    public bool GetActiveHydrant() => isActive;
    #endregion

    [Header("Score Manager For Decrease Score")]
    [SerializeField] ScoreManager scoreManager;

    private bool previousLeverRightValue = false;
    private bool previousLeverLeftValue = false;
    
    private void Update()
    {
        // Cek perubahan pada status lever sebelum memperbarui status hydrant
        bool currentLeverRightValue = leverRight.value;
        bool currentLeverLeftValue = leverLeft.value;

        if (currentLeverRightValue != previousLeverRightValue || currentLeverLeftValue != previousLeverLeftValue)
        {
            if (currentLeverRightValue || !currentLeverLeftValue)
            {
                isActive = true;
                photonView.RPC("UpdateHydrantActivationStatus", RpcTarget.All, true);
                Debug.Log("LEVER TELAH DI NYALAKAN");
            }
            else
            {
                isActive = false;
                photonView.RPC("UpdateHydrantActivationStatus", RpcTarget.All, false);
                Debug.Log("LEVER TELAH DI MATIKAN");
            }

            // Memanggil fungsi untuk mengupdate status selang kanan dan kiri hanya jika ada perubahan pada lever
            UpdateSelangStatus();

            // Perbarui status lever sebelumnya
            previousLeverRightValue = currentLeverRightValue;
            previousLeverLeftValue = currentLeverLeftValue;
        }
        ScaneSelangKiri();
        ScanSelangKanan();
    }

    private void UpdateSelangStatus()
    {
        UpdateSelangKiriStatus();
        UpdateSelangKananStatus();
    }

    private void UpdateSelangKiriStatus()
    {
        if (isActive && !leverLeft.value && xRKnob.value >= 0.25f)
        {
            ActivateSelangKiri(true, true);
        }
        else if (isActive && !leverLeft.value && xRKnob.value < 0.25f)
        {
            ActivateSelangKiri(false, true);
        }
        else
        {
            ActivateSelangKiri(false, false);
        }
    }

    private void UpdateSelangKananStatus()
    {
        if (isActive && leverRight.value && xRKnob.value >= 0.25f)
        {
            ActivateSelangKanan(true, true);
        }
        else if (isActive && leverRight.value && xRKnob.value < 0.25f)
        {
            ActivateSelangKanan(false, true);
        }
        else
        {
            ActivateSelangKanan(false, false);
        }
    }

    private void ActivateSelangKiri(bool selang, bool water)
    {
        Debug.Log($"Activating left hose: selang={selang}, water={water}");
        photonView.RPC("ManagerSelangKiri", RpcTarget.All, selang, water);        
    }

    void ScaneSelangKiri()
    {
        if (!error_selang_kiri.activeSelf && !error_selang_kiri_nozzel.activeSelf && isActive && !leverLeft.value)
        {
            Debug.Log("Activating left water: no errors, hydrant active, left lever off");
            if (isAssesmen)
            {
                scoreManager.DecreaseScorePillar(50, true);
                photonView.RPC("WaterStatusKiri", RpcTarget.All, true);
            }
            else
            {
                photonView.RPC("WaterStatusKiri", RpcTarget.All, true);
            }            
        }
        else if (error_selang_kiri_nozzel.activeSelf && isActive && !leverLeft.value)
        {
            Debug.Log("Deactivating left water: nozzle error, hydrant active, left lever off");
            photonView.RPC("WaterStatusKiri", RpcTarget.All, false);
        }
        else if (error_selang_kiri.activeSelf && isActive && !leverLeft.value)
        {
            Debug.Log("Deactivating left water: nozzle error, hydrant active, left lever off");
            photonView.RPC("WaterStatusKiri", RpcTarget.All, false);
        }
        else
        {
            Debug.Log("Deactivating left water: other conditions not met");
            photonView.RPC("WaterStatusKiri", RpcTarget.All, false);
        }
    }
    void ScanSelangKanan()
    {
        if (!error_selang_kanan.activeSelf && !error_selang_kanan_nozzel.activeSelf && isActive && leverRight.value)
        {
            Debug.Log("Activating right water: no errors, hydrant active, right lever on");
            if (isAssesmen)
            {
                scoreManager.DecreaseScorePillar(50, true);
                photonView.RPC("WaterStatusKanan", RpcTarget.All, true);
            }
            else
            {
                photonView.RPC("WaterStatusKanan", RpcTarget.All, true);
            }            
        }
        else if (error_selang_kanan_nozzel.activeSelf && isActive && leverRight.value)
        {
            Debug.Log("Deactivating right water: nozzle error, hydrant active, right lever on");
            photonView.RPC("WaterStatusKanan", RpcTarget.All, false);
        }
        else if (error_selang_kanan.activeSelf && isActive && leverRight.value)
        {
            Debug.Log("Deactivating right water: nozzle error, hydrant active, right lever on");
            photonView.RPC("WaterStatusKanan", RpcTarget.All, false);
        }
        else
        {
            Debug.Log("Deactivating right water: other conditions not met");
            photonView.RPC("WaterStatusKanan", RpcTarget.All, false);
        }
    }
    private void ActivateSelangKanan(bool selang, bool water)
    {
        Debug.Log($"Activating right hose: selang={selang}, water={water}");
        photonView.RPC("ManagerSelangKanan", RpcTarget.All, selang, water);        
    }


    [PunRPC]
    private void UpdateHydrantActivationStatus(bool status)
    {
        isActive = status;
        Debug.Log($"Hydrant activation status updated: {isActive}");
    }

    [PunRPC]
    private void ManagerSelangKanan(bool selang, bool water)
    {
        selangKanan.SetIsReadyWoosh(selang);
        waterOnKanan = water;
        Debug.Log($"Right hose updated: selang={selang}, water={water}");
    }

    [PunRPC]
    private void ManagerSelangKiri(bool selang, bool water)
    {
        selangKiri.SetIsReadyWoosh(selang);
        waterOnKiri = water;
        Debug.Log($"Left hose updated: selang={selang}, water={water}");
    }

    [PunRPC]
    private void WaterStatusKanan(bool active)
    {
        water_kanan.SetActive(active);
    }

    [PunRPC]
    private void WaterStatusKiri(bool active)
    {
        water_kiri.SetActive(active);
    }
}
