using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;

    void Start()
    {
        Vector3 spawnPos = new Vector3(
            Random.Range(-3f, 3f),
            1f,
            Random.Range(-3f, 3f)
        );

        PhotonNetwork.Instantiate(playerPrefab.name, spawnPos, Quaternion.identity);
    }
}