using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change_ObgScale : MonoBehaviour
{
    Vector3 mousescroll;
    float x;
    float y;
    float z;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousescroll = Input.mouseScrollDelta;
        x += mousescroll.y;
        y += mousescroll.y;
        z += mousescroll.y;
        transform.localScale = new Vector3(x,y,z);
        if (x>4)
        {
            x = 4;
        }
        if (x < 0.1f)
        {
            x = 0.1f;
        }
        if (y>4)
        {
            y = 4;
        }
        if (y<0.1f)
        {
            y = 0.1f;
        }
        if (z>4)
        {
            z = 4;
        }
        if (z<0.1f)
        {
            z = 0.1f;
        }
    }

    void FixedUpdate ()
    {
        transform.localScale = transform.localScale + new Vector3 (1, 1, 1);
    }
}
