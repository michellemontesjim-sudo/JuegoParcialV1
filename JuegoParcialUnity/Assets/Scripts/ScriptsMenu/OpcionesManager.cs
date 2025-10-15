using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpcionesManager : MonoBehaviour
{
    [Header("Controles de UI")]
    public Slider sliderVolumen;
    public Slider sliderBrillo;
    public Image filtroBrillo;

    [Header("Audio")]
    public AudioSource musicaFondo;

    void Start()
    {
        
        sliderVolumen.value = PlayerPrefs.GetFloat("Volumen", 1f);
        sliderBrillo.value = PlayerPrefs.GetFloat("Brillo", 1f);

        
        CambiarVolumen(sliderVolumen.value);
        CambiarBrillo(sliderBrillo.value);

        
        sliderVolumen.onValueChanged.AddListener(CambiarVolumen);
        sliderBrillo.onValueChanged.AddListener(CambiarBrillo);
    }

    public void CambiarVolumen(float valor)
    {
        if (musicaFondo != null)
            musicaFondo.volume = valor;

        PlayerPrefs.SetFloat("Volumen", valor);
    }

    public void CambiarBrillo(float valor)
    {

        Color color = filtroBrillo.color;
        color.a = 1f - valor; 
        filtroBrillo.color = color;

        PlayerPrefs.SetFloat("Brillo", valor);
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
