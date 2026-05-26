using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviourPunCallbacks
{
    public TMP_InputField playerNameInput;
    public GameObject createRoomButton;
    public GameObject joinRoomButton;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        // Disable buttons at start
        createRoomButton.SetActive(false);
        joinRoomButton.SetActive(false);
    }

    public void Connect()
    {
        PhotonNetwork.NickName = playerNameInput.text;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");

        // Enable buttons ONLY when ready
        createRoomButton.SetActive(true);
        joinRoomButton.SetActive(true);
    }

    // CREATE ROOM
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(
            "Room1",
            new RoomOptions { MaxPlayers = 3 }
        );
    }

    // JOIN ROOM
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom("Room1");
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join Room Failed: " + message);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create Room Failed: " + message);
    }
}