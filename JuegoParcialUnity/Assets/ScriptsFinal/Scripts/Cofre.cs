using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cofre : MonoBehaviour
{
    public Animator animator;
    private bool jugadorCerca = false;
    public int oro = 100;

    void Update()
    {
        // Si el jugador está cerca y presiona la tecla E, se abre el cofre
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            animator.SetBool("IsOpen", true);
            Debug.Log("Has recibido " + oro + " de oro!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            Debug.Log("Presiona E para abrir el cofre");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
        }
    }
}
