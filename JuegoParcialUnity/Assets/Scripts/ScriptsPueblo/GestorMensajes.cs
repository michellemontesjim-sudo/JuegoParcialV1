using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GestorMensajes : MonoBehaviour
{
    public static GestorMensajes instance;

    public CanvasGroup mensajeGlobal;
    public TextMeshProUGUI textoUI;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void MostrarMensaje(string texto, float duracion = 2f)
    {
        textoUI.text = texto;
        StopAllCoroutines();
        StartCoroutine(FadeMensaje(duracion));
    }

    private System.Collections.IEnumerator FadeMensaje(float duracion)
    {
        // Fade In
        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            mensajeGlobal.alpha = t;
            yield return null;
        }
        mensajeGlobal.alpha = 1;

        // Mantener visible
        yield return new WaitForSeconds(duracion);

        // Fade Out
        for (float t = 1f; t > 0f; t -= Time.deltaTime)
        {
            mensajeGlobal.alpha = t;
            yield return null;
        }
        mensajeGlobal.alpha = 0;
    }
}
