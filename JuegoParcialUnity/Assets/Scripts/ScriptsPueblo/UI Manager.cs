using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instancia;

    public TextMeshProUGUI textoInteraccion;

    private void Awake()
    {
        if (instancia == null) instancia = this;
    }

    public void MostrarTexto(string mensaje)
    {
        textoInteraccion.text = mensaje;
        textoInteraccion.gameObject.SetActive(true);
    }

    public void OcultarTexto()
    {
        textoInteraccion.gameObject.SetActive(false);
    }

}
