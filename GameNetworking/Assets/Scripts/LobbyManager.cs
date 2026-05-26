using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_Text playerListText;
    public TMP_Text roomInfoText;
    public GameObject startGameButton;

    void Start()
    {
        UpdatePlayerList();

        // Only host can start game
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    void UpdatePlayerList()
    {
        playerListText.text = "Players:\n";

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerListText.text += player.NickName + "\n";
        }

        roomInfoText.text = "Room: " + PhotonNetwork.CurrentRoom.Name +
                            "\nPlayers: " + PhotonNetwork.CurrentRoom.PlayerCount + "/4";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameScene");
        }
    }
}