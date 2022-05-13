using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMap : MonoBehaviour
{
    public bool isPressed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        isPressed = gameObject.GetComponent<PhysicsButton>().isPressed;
        if (isPressed) Load();

    }

    public void Load()
    {
        GameObject.Find("Terrain").SetActive(false);
        isPressed = false;

    }
}
