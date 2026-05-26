using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Basketball : MonoBehaviourPun
{
    public Player lastPlayerWhoTouched;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PhotonView pv =
                collision.gameObject.GetComponent<PhotonView>();

            if (pv != null)
            {
                lastPlayerWhoTouched = pv.Owner;
            }
        }
    }
}