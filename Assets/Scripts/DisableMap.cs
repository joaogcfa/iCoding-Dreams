using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMap : MonoBehaviour
{
    public bool isPressed;
    private bool booleana;
    // Start is called before the first frame update
    void Start()
    {
        booleana = true;
    }

    // Update is called once per frame
    void Update()
    {
        isPressed = gameObject.GetComponent<PhysicsButton>().isPressed;
        if (isPressed && booleana) Load();

    }

    public void Load()
    {
        GameObject.Find("Mapa").SetActive(false);
        booleana = false;

    }
}
