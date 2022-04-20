using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControle : MonoBehaviour
{
    public AudioSource audioSourceMusicaClock;
    public AudioClip[] MusicasDeFundo; //caso queiram colocar mais de uma música de fundo

    // Start is called before the first frame update
    void Start()
    {
        AudioClip MusicaDessaFase = MusicasDeFundo[0];
        audioSourceMusicaClock.clip = MusicaDessaFase;
        audioSourceMusicaClock.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
