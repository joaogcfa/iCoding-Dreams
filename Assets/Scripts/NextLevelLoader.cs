using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        // print(collision.gameObject.name);
        // if (collision.gameObject.name == "Player")
        // {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // }
    }
}
