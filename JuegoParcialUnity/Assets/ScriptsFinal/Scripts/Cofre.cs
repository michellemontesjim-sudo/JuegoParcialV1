using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cofre : MonoBehaviour
{
    public int CantidadOro = 100;
    private Animator animator;
    private bool abierto = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            abierto = !abierto; // alterna abrir/cerrar
            animator.SetBool("IsOpen", abierto);

            if (abierto)
                Debug.Log($"Has recibido {CantidadOro} de oro!");
        }
    }
}
