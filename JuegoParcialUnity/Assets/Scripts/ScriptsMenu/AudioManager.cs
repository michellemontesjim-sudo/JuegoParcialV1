using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musica;
    

    void Start()
    {
        
        musica.volume = PlayerPrefs.GetFloat("Volumen", 1f);
    }

    void Update()
    {
        
        musica.volume = PlayerPrefs.GetFloat("Volumen", 1f);
    }
}
