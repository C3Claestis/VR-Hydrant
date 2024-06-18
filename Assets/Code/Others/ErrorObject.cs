using UnityEngine;

/// <summary>
/// Untuk mengubah layer objek yang tembus atau tidak sesuai
/// </summary>
public class ErrorObject : MonoBehaviour
{
    [SerializeField] GameObject parent_selang;
    private int invisibleLayer = 12;  // Ganti dengan indeks layer untuk "Invisible"
    private int defaultLayer = 6;    // Ganti dengan indeks layer default

    private bool wasParentActive;

    void Start()
    {
        // Lacak status aktif awal dari parent_selang
        wasParentActive = parent_selang.activeSelf;

        // Atur layer awal dari objek berdasarkan status aktif parent_selang
        gameObject.layer = wasParentActive ? defaultLayer : invisibleLayer;
    }

    void Update()
    {
        // Cek apakah status aktif dari parent_selang telah berubah
        if (parent_selang.activeSelf != wasParentActive)
        {
            // Update layer secara terus-menerus berdasarkan status aktif dari parent_selang
            if (!parent_selang.activeSelf)
            {
                // Ubah layer menjadi invisible
                SetLayerRecursively(gameObject, invisibleLayer);
            }
            else
            {
                // Ubah layer kembali ke default
                SetLayerRecursively(gameObject, defaultLayer);
            }

            // Perbarui status aktif yang dilacak
            wasParentActive = parent_selang.activeSelf;
        }
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (child == null)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
