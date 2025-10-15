using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NPCInteraccion : MonoBehaviour
{
    public Dialogo firstDialogue; // Asigna el primer diálogo del NPC
    private DialogoManager dialogoManager;
    private bool isPlayerInRange = false;

    void Start()
    {
        dialogoManager = FindObjectOfType<DialogoManager>();
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            dialogoManager.StartDialogue(firstDialogue);
            UI_InteraccionHint.Instance.HideHint();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            UI_InteraccionHint.Instance.ShowHint("Presiona E para hablar");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            UI_InteraccionHint.Instance.HideHint();
            dialogoManager.CloseDialogue();
        }
    }
}
