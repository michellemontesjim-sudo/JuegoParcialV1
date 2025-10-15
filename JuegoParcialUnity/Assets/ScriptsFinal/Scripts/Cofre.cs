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
                textoDialogo.text = "¡Has recibido " + oro + " de oro!";
                cofreAbierto = true;
            }
            else
            {
                textoDialogo.text = "El cofre ya está vacío...";
            }
        }
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
            panelDialogo.SetActive(false);
        }
    }
}
