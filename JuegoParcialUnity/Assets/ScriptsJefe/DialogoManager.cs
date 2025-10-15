using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogoManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelDialogo;
    public Text textoDialogo;
    public Text textoNombre;
    public Image imagenPersonaje;

    [Header("Configuración")]
    public float velocidadTexto = 0.05f;
    public KeyCode teclaContinuar = KeyCode.Space;

    [Header("Sprites Personajes")]
    public Sprite spriteHelena;
    public Sprite spriteLupus;
    public Sprite spriteNarrador;

    private bool mostrandoDialogo = false;
    private bool textoCompleto = false;
    private Coroutine corrutinaTexto;

    public IEnumerator MostrarDialogo(string texto, string nombrePersonaje = "Narrador")
    {
        // Mostrar panel
        panelDialogo.SetActive(true);
        mostrandoDialogo = true;
        textoCompleto = false;

        // Configurar nombre y sprite
        textoNombre.text = nombrePersonaje;
        ConfigurarSpritePersonaje(nombrePersonaje);

        // Mostrar texto con efecto de escritura
        textoDialogo.text = "";
        corrutinaTexto = StartCoroutine(EscribirTexto(texto));

        // Esperar a que termine de escribir
        yield return new WaitUntil(() => textoCompleto);

        // Esperar input para continuar
        yield return new WaitUntil(() => Input.GetKeyDown(teclaContinuar));

        // Ocultar panel
        panelDialogo.SetActive(false);
        mostrandoDialogo = false;
    }

    private IEnumerator EscribirTexto(string texto)
    {
        textoCompleto = false;

        foreach (char caracter in texto)
        {
            textoDialogo.text += caracter;

            // Efecto de sonido opcional aquí
            yield return new WaitForSeconds(velocidadTexto);
        }

        textoCompleto = true;
    }

    private void ConfigurarSpritePersonaje(string nombre)
    {
        if (imagenPersonaje == null) return;

        switch (nombre.ToLower())
        {
            case "helena":
                imagenPersonaje.sprite = spriteHelena;
                imagenPersonaje.color = Color.white;
                break;
            case "lupus":
                imagenPersonaje.sprite = spriteLupus;
                imagenPersonaje.color = Color.white;
                break;
            default:
                imagenPersonaje.sprite = spriteNarrador;
                imagenPersonaje.color = Color.gray;
                break;
        }
    }

    // Método para forzar completar texto
    public void CompletarTextoInmediato()
    {
        if (corrutinaTexto != null)
        {
            StopCoroutine(corrutinaTexto);
            textoCompleto = true;
        }
    }

    public bool DialogoActivo()
    {
        return mostrandoDialogo;
    }
}