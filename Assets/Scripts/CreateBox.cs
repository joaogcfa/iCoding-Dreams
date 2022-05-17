using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBox : MonoBehaviour
{

    public GameObject Box;
    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        print(gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void OnTriggerEnter(Collider col){
        print(col.gameObject.tag);
        if(col.gameObject.tag == "Fruit"){
            col.gameObject.GetComponent<Holdable>().enabled = false;
            Destroy(col.gameObject);
            pos = GameObject.Find("Player").transform.position;
            pos += new Vector3 (0,2,4);
            Instantiate(Box, pos, Quaternion.identity);
            
        }
    }
}
