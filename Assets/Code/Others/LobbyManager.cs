using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class LobbyManager : MonoBehaviour
{
    public void DisconnectFromPhoton()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            Debug.Log("Disconnected from Photon.");
            SceneManager.LoadScene(0);
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
