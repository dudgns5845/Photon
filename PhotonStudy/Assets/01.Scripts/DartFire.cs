using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartFire : MonoBehaviour
{
    public float power = 30f;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * power);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(transform.forward * power);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        DartPiece dartboard = collision.gameObject.GetComponent<DartPiece>();
        if(dartboard != null)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            print("다트 충돌" + dartboard.type + "/" + dartboard.score);
            transform.SetParent(collision.transform);
            GetComponent<BoxCollider>().enabled = false;
        }
    }

}
