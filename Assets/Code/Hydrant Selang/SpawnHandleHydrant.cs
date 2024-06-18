using UnityEngine;
using Photon.Pun;

/// <summary>
/// Untuk spawn handle ketika player ingin mengambil kembali handle dari pillar
/// </summary>
public class SpawnHandleHydrant : MonoBehaviour
{
    [SerializeField] Transform pointSpawn; // Titik spawn objek

    // Metode untuk memunculkan handle
    public void SpawnHandle()
    {
        // Membuat instance objek handle pada titik spawn dengan rotasi default
        PhotonNetwork.Instantiate("Handle", pointSpawn.position, Quaternion.identity);
    }

    // Metode untuk memunculkan selang
    public void SpawnSelang()
    {
        // Membuat instance objek selang pada titik spawn dengan rotasi 180 derajat pada sumbu Y
        PhotonNetwork.Instantiate("Selang Fire Hose New", pointSpawn.position, Quaternion.Euler(0f, 180f, 0f));
    }
    public void SpawnNozzel()
    {
        // Membuat instance objek selang pada titik spawn dengan rotasi 180 derajat pada sumbu Y
        PhotonNetwork.Instantiate("Nozzle", pointSpawn.position, Quaternion.identity);
    }
}
