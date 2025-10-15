using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cofre : MonoBehaviour
{
    public Animator animator;
    private bool jugadorCerca = false;
    public int oro = 100;
    public GameObject panelDialogo;

    public TextMeshProUGUI textoDialogo;

    private bool cofreAbierto = false;

    private void Start()
    {
        panelDialogo.SetActive(false);
    }

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            if (!cofreAbierto)
            {
                animator.SetBool("IsOpen", true);
                StartCoroutine(DialogoSecuencial()); // Inicia el diálogo en partes
                cofreAbierto = true;
            }
            else
            {
                textoDialogo.text = "El cofre ya está vacío...";
            }
        }
    }

    IEnumerator DialogoSecuencial()
    {
        textoDialogo.text = "¡Has recibido " + oro + " de oro!";
        yield return new WaitForSeconds(2f);

        textoDialogo.text = "Queremos recompensarte por esta gran batalla que tuviste...";
        yield return new WaitForSeconds(3f);

        textoDialogo.text = "Sabemos que no fue fácil para ti, viajero...";
        yield return new WaitForSeconds(3f);

        textoDialogo.text = "Continúa con tu camino, gran héroe.";
        yield return new WaitForSeconds(3f);

        panelDialogo.SetActive(false); // Se cierra al final (opcional)
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            panelDialogo.SetActive(true);
            textoDialogo.text = "Presiona E para abrir el cofre";
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            if (panelDialogo != null)
            {
                panelDialogo.SetActive(false);
            }
        }
    }
}