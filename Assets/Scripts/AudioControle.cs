using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControle : MonoBehaviour
{
    public AudioSource audioSourceMusicaClock;
    public AudioClip[] MusicasDeFundo; //caso queiram colocar mais de uma música de fundo
    public float velocidade = 0;
    public int qualfreq = 0;
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
        int i = 0.01
        velocidade(i++);
        if (velocidade == 1000)
        {
            velocidade = 0;
            qualfreq++;
            AudioClip MusicaDessaFase = MusicasDeFundo[qualfreq];
            audioSourceMusicaClock.clip = MusicaDessaFase;
            audioSourceMusicaClock.Play();
        }

        if(qualfreq == 16)
        {
            qualfreq = 0;
        }
    }
}
