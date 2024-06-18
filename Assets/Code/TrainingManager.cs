using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class TrainingManager : MonoBehaviourPun
{
    #region Serialized Fields

    [Header("Jumlah Player Yang Masuk")]
    [SerializeField] private int JumlahPlayer; // Number of players that need to join

    [Header("Members Assesmen")]
    [SerializeField] private GameObject boxCollider; // Box collider object
    [SerializeField] private GameObject button_confirm; // Confirm button
    [SerializeField] private GameObject[] UI; // Array of UI elements
    [SerializeField] private GameObject area_menggulung; // Area menggulung object
    [SerializeField] private GameObject area_meluruskan; // Area meluruskan object

    [Header("Sound Members")]
    [SerializeField] private GameObject announcement; // Announcement sound object
    

    #endregion

    #region Private Variables

    private int value_count; // Counter for UI elements
    private bool isGo = false; // Flag to indicate if the training can start
    private bool shouldUpdateUI = true; // Flag to control UI updates
    #endregion

    #region Unity Methods

    void Update()
    {
        if (isGo)
        {
            FungsiMulai();
        }
        else
        {
            FungsiAwal();
        }
    }

    #endregion

    #region Custom Methods

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

        // Check if the number of players is exactly as required
        if (count == JumlahPlayer)
        {
            isGo = true;
            boxCollider.SetActive(false);
            button_confirm.SetActive(true);
            announcement.SetActive(true);
            value_count = 1;
            Debug.Log("Ada tepat 2 objek bernama 'Player-VR(Clone)'. isGo diatur ke true.");
        }
        else
        {
            Debug.Log($"Jumlah objek bernama 'Player-VR(Clone)' adalah {count}. isGo tetap false.");
        }
    }

    void FungsiMulai()
    {
        if (shouldUpdateUI)
        {
            // Deactivate all UI elements first
            for (int i = 0; i < UI.Length; i++)
            {
                UI[i].SetActive(false);
            }

            // Activate the UI element corresponding to value_count
            if (value_count > 0 && value_count <= UI.Length)
            {
                UI[value_count - 1].SetActive(true);
            }

            shouldUpdateUI = false;
        }        
    }

    public void DisconnectFromPhoton()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            Debug.Log("Disconnected from Photon.");
            SceneManager.LoadScene(0);
        }
    }

    [PunRPC]
    public void IncrementCount()
    {        
        value_count++;
        shouldUpdateUI = true; // Set the flag to true after incrementing
    }

    public void IncrementCountRPC()
    {
        photonView.RPC("IncrementCount", RpcTarget.All);
    }

    #endregion

    #region TODO

    // TODO: Add more detailed logging for debugging purposes.
    // TODO: Implement additional feedback for the player when the number of players is correct.
    // TODO: Add UI animations for transitions between different UI states.
    // TODO: Refactor repetitive code into separate methods where possible.

    #endregion
}
