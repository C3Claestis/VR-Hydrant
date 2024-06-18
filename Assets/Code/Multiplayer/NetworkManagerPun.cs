using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[System.Serializable]
public class DefaultRoom
{
    public string Name;
    public int sceneIndex;
    public int maxPlayer;
}
public class NetworkManagerPun : MonoBehaviourPunCallbacks
{
    public List<DefaultRoom> defaultRooms;
    public GameObject roomsUI;
  
    public void ConnectingToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Try Connecting...");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Server");
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();       
    }

    public override void OnJoinedLobby() 
    {
        base.OnJoinedLobby();
        roomsUI.SetActive(true);
    }
    public void InitializeRoom(int defaultRoomsIndex)
    {
        DefaultRoom roomsettings = defaultRooms[defaultRoomsIndex];

        //LOAD SCENE
        PhotonNetwork.LoadLevel(roomsettings.sceneIndex);

        //CREATE ROOM
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)roomsettings.maxPlayer;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom(roomsettings.Name, roomOptions, TypedLobby.Default);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        base.OnJoinedRoom();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player Entered");
        base.OnPlayerEnteredRoom(newPlayer);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player Left Room");
        base.OnPlayerLeftRoom(otherPlayer);
    }
}
