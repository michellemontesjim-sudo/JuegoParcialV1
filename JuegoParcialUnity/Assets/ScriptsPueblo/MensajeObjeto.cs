using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenajeObjeto : MonoBehaviour
{
    public string mensaje = "Presiona E para leer el mensaje";
    private bool jugadorCerca = false;
    public string mensajeInteraccion = "Los números han existido desde el inicio del tiempo...";

    [Header("UI")]
    public TextMeshProUGUI textoUI;

    private void Start()
    {
        if (textoUI != null)
            textoUI.gameObject.SetActive(false); // el texto empieza oculto
    }

    private void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            Interactuar();
        }
    }

    private void Interactuar()
    {
        Debug.Log("Interacción con " + gameObject.name);
        GestorMensajes.instance.MostrarMensaje(mensajeInteraccion, 2f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            UIManager.instancia.MostrarTexto(mensaje);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            UIManager.instancia.OcultarTexto();
        }
    }
}
