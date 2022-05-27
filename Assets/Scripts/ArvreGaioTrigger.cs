using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArvreGaioTrigger : MonoBehaviour {


    [SerializeField] private ArvreGaioGaio gaio;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gaio.Cair();
        }
    }

}
