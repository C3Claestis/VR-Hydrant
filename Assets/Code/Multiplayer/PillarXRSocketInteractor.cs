using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class PillarObjects
{
    public GameObject objects;
    public bool isActive; // Tambahkan variabel boolean untuk menentukan apakah objek aktif atau tidak
}

public class PillarXRSocketInteractor : MonoBehaviourPunCallbacks
{
    [SerializeField] List<PillarObjects> objek;

    // Method untuk mengaktifkan GameObject melalui RPC
    public void ActivateObject()
    {
        photonView.RPC("RPC_ActivateObject", RpcTarget.All);
    }

    // RPC method untuk mengaktifkan GameObject
    [PunRPC]
    private void RPC_ActivateObject()
    {
        // Iterasi melalui semua objek dalam list dan atur aktivasi berdasarkan kondisi isActive
        foreach (PillarObjects pillarObject in objek)
        {
            pillarObject.objects.SetActive(pillarObject.isActive);
        }
    }
}
