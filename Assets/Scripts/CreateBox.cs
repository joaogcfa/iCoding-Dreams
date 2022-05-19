using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreateBox : MonoBehaviour
{

    public GameObject Box;
    private Vector3 pos;
    public int max = 1;
    public int count;
    TMP_Text FruitCounter;
    // private GameObject CounterText;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        // print(gameObject.transform.Find("Counter"));
        FruitCounter = GameObject.Find("Counter").GetComponent<TMP_Text>();
        print(FruitCounter);
        FruitCounter.SetText("0/{0}", max);
    }

    // Update is called once per frame
    void Update()
    {
        if(count >= max){
            Destroy(gameObject.transform.parent.gameObject);
        }
        
    }

    void OnTriggerEnter(Collider col){
        print(col.gameObject.tag);
        if(col.gameObject.tag == "Fruit"){
            col.gameObject.GetComponent<Holdable>().enabled = false;
            Destroy(col.gameObject);
            pos = GameObject.Find("Player").transform.position;
            pos += new Vector3 (0,2,4);
            Instantiate(Box, pos, Quaternion.identity);
            count++;
            FruitCounter.SetText("{0}/{1}", count, max);
            
        }
    }
}
