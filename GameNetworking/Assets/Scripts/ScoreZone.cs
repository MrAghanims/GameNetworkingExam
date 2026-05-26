using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Basketball ball =
                other.GetComponent<Basketball>();

            if (ball != null &&
                ball.lastPlayerWhoTouched != null)
            {
                ScoreManager.AddScore(
                    ball.lastPlayerWhoTouched,
                    2);
            }
        }
    }
}