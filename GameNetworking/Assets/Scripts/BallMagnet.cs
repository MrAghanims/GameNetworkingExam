using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class BallMagnet : MonoBehaviour
{
    public Transform holdPoint;
    private bool isHoldingBall;

    public float throwForce = 12f;
    public float upwardForce = 5f;
    public float shotArc = 0.5f;
    public float minThrowForce = 5f;
    public float maxThrowForce = 20f;

    public float minArc = 0.3f;
    public float maxArc = 1.2f;

    public float maxChargeTime = 2f;

    private float currentCharge;
    private bool isCharging;

    private Rigidbody heldBall;
    private bool canPickup = true;

    private Transform heldBallTransform;

    public GameObject hoopTarget;
    public float aimAssistStrength = 0.5f;
    private PhotonView pv;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touched: " + other.name);

        if (heldBall != null || !canPickup) return;

        if (other.CompareTag("Ball"))
        {
            Debug.Log("BALL DETECTED");

            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                PickupBall(rb);
            }
        }
    }
    void Start()
    {
        if (hoopTarget == null)
            hoopTarget = GameObject.Find("HoopTarget");
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (pv == null || !pv.IsMine) return;
        if (heldBall == null) return;
     
        // Start charging
        if (Input.GetMouseButtonDown(0))
        {
            isCharging = true;
            currentCharge = 0f;
        }

        // While holding mouse
        if (Input.GetMouseButton(0) && isCharging)
        {
            currentCharge += Time.deltaTime;

            // Clamp charge time
            currentCharge = Mathf.Clamp(currentCharge, 0f, maxChargeTime);

            Debug.Log("Charging: " + currentCharge);
        }

        // Release shot
        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            isCharging = false;

            ThrowBall();
        }
        if (pv == null || !pv.IsMine)
            return;
        if (heldBall != null)
        {
            heldBallTransform.position = Vector3.Lerp(
                heldBallTransform.position,
                holdPoint.position,
                20f * Time.deltaTime
            );

            heldBallTransform.rotation = holdPoint.rotation;
        }
    }

    void PickupBall(Rigidbody rb)
    {
        PhotonView ballPV = rb.GetComponent<PhotonView>();

        if (ballPV != null)
        {
            ballPV.RequestOwnership();

            if (!ballPV.IsMine)
                return;
        }

        heldBall = rb;
        heldBallTransform = rb.transform;

        heldBall.velocity = Vector3.zero;
        heldBall.angularVelocity = Vector3.zero;

        heldBall.useGravity = false;
        heldBall.isKinematic = true;

        isHoldingBall = true;
    }

    void ThrowBall()
    {
        PhotonView ballPV = heldBall.GetComponent<PhotonView>();

        if (ballPV != null && !ballPV.IsMine)
            return;

        heldBall.isKinematic = false;
        heldBall.useGravity = true;
        heldBallTransform = null;

        heldBall.velocity = Vector3.zero;
        heldBall.angularVelocity = Vector3.zero;

        // Convert charge into 0-1 value
        float chargePercent = currentCharge / maxChargeTime;

        // Scale power and arc
        float finalForce = Mathf.Lerp(minThrowForce, maxThrowForce, chargePercent);
        float finalArc = Mathf.Lerp(minArc, maxArc, chargePercent);

        // Create shot direction
        Vector3 toHoop = (hoopTarget.transform.position - holdPoint.position).normalized;

        Vector3 forwardDir = Vector3.Lerp(
            transform.forward,
            toHoop,
            aimAssistStrength
        );

        Vector3 shootDirection =
            (forwardDir + Vector3.up * finalArc).normalized;

      
        // Shoot
        heldBall.AddForce(shootDirection * finalForce, ForceMode.Impulse);
        heldBall.AddTorque(transform.right * 5f, ForceMode.Impulse);

        heldBall = null;
        isHoldingBall = false;
        canPickup = false;

        Invoke(nameof(EnablePickup), 1f);
        
    }
    void EnablePickup()
    {
        canPickup = true;
    }
}