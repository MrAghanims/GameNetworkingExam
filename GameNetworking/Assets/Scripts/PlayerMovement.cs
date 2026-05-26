using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    public float speed = 5f;
    public float jumpForce = 5f;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Only control YOUR player
        if (!photonView.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>());
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(x, 0, z);
        transform.Translate(move * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}