using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ArvreGaioGaio : MonoBehaviour {

    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    public void Cair() {
        rb.useGravity = true;
    }

    void Update() {
        // when the object reachs the ground (using raycast), turn on all constraints
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.001f)) {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

    }
}
