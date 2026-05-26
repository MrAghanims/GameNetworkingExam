using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviourPunCallbacks
{
    public TMP_Text scoreboardText;

    void Update()
    {
        UpdateScoreboard();
    }

    void UpdateScoreboard()
    {
        string board = "";

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            int score = 0;

            if (player.CustomProperties.ContainsKey("Score"))
            {
                score = (int)player.CustomProperties["Score"];
            }

            board +=
                player.NickName +
                " : " +
                score +
                "\n";
        }

        scoreboardText.text = board;
    }

    public static void AddScore(Player player, int amount)
    {
        int currentScore = 0;

        if (player.CustomProperties.ContainsKey("Score"))
        {
            currentScore =
                (int)player.CustomProperties["Score"];
        }

        ExitGames.Client.Photon.Hashtable hash =
    new ExitGames.Client.Photon.Hashtable();

        hash["Score"] =
            currentScore + amount;

        player.SetCustomProperties(hash);
    }
}