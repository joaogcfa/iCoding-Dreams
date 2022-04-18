using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Holdable : MonoBehaviour {

    [HideInInspector] public float gripForce = 10f;

    [HideInInspector] public GravityGun gun;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (gun != null)
        {
            rb.velocity = Vector3.zero;
        }
    }

    void OnCollisionEnter(Collision collision) {
        float collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        if (collisionForce > gripForce && gun != null) {
            gun.ReleaseObject();
            gun = null;
        }
    }

    void OnCollisionExit(Collision collision) {
        if (gun != null) {
            gun.ResetReachedAttachmentPoint();
        }
    }
}
