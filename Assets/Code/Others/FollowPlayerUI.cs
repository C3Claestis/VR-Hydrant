using UnityEngine;

public class FollowPlayerUI : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        // Mencari referensi ke kamera utama
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Pastikan kamera utama telah ditemukan sebelum memproses rotasi UI
        if (mainCamera != null)
        {
            // Dapatkan arah vektor dari UI ke kamera
            Vector3 directionToCamera = mainCamera.transform.position - transform.position;

            // Lakukan rotasi ke arah kamera menggunakan Quaternion.LookRotation
            Quaternion rotationToCamera = Quaternion.LookRotation(directionToCamera, Vector3.up);

            // Terapkan rotasi ke UI
            transform.rotation = rotationToCamera;
        }
        else
        {
            Debug.LogWarning("Kamera utama tidak ditemukan.");
        }
    }
}
