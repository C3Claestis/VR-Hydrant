using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class AsesmenManager : MonoBehaviourPun
{
    #region Serialized Fields

    [Header("Jumlah Player Yang Masuk")]
    [SerializeField] private int JumlahPlayer; // Number of players required to start

    [Header("Members Assesmen")]
    [SerializeField] private GameObject boxCollider; // Box collider object
    [SerializeField] private GameObject soundStart;  // Sound to play at start
    [SerializeField] private GameObject penilaian;   // Assessment object

    #endregion

    #region Unity Methods

    // Update is called once per frame
    void Update()
    {
        // Call the function to check player count and activate objects
        FungsiAwal();
    }

    #endregion

    #region Custom Methods

    /// <summary>
    /// Checks the number of "Player-VR(Clone)" objects in the scene and activates/deactivates related objects.
    /// </summary>
    void FungsiAwal()
    {
        // Find all GameObjects in the scene
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        int count = 0;

        // Count the number of GameObjects named "Player-VR(Clone)"
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "Player-VR(Clone)")
            {
                count++;
            }
        }

        // Check if the count matches the required number of players
        if (count == JumlahPlayer)
        {
            // Deactivate the box collider, activate the start sound and assessment
            boxCollider.SetActive(false);
            soundStart.SetActive(true);
            penilaian.SetActive(true);
            Debug.Log("Ada tepat " + JumlahPlayer + " objek bernama 'Player-VR(Clone)'. isGo diatur ke true.");
        }
        else
        {
            Debug.Log($"Jumlah objek bernama 'Player-VR(Clone)' adalah {count}. isGo tetap false.");
        }
    }

    /// <summary>
    /// Disconnects from Photon and loads the first scene.
    /// </summary>
    public void DisconnectFromPhoton()
    {
        // Check if connected to Photon
        if (PhotonNetwork.IsConnected)
        {
            // Disconnect from Photon
            PhotonNetwork.Disconnect();
            Debug.Log("Disconnected from Photon.");
            // Load the first scene (index 0)
            SceneManager.LoadScene(0);
        }
    }

    #endregion

    #region TODO

    // TODO: Add additional logic to handle different player counts dynamically.
    // TODO: Implement a waiting room or lobby system before starting the assessment.
    // TODO: Add visual or audio feedback for players when the assessment starts or fails to start.

    #endregion
}
