using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DialogoManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public TMP_Text npcNombreText;
    public TMP_Text dialogoText;
    public GameObject dialogoPanel;
    public GameObject opcionesPanel;
    public Button opcionBotonPrefab;

    private Dialogo currentDialogue;

    void Start()
    {
        dialogoPanel.SetActive(false);
    }

    public void StartDialogue(Dialogo dialogue)
    {
        if (dialogue == null) return;

        currentDialogue = dialogue;
        dialogoPanel.SetActive(true);

        npcNombreText.text = dialogue.npcNombre;

        // Mostrar primera oración
        if (dialogue.oraciones.Length > 0)
            dialogoText.text = dialogue.oraciones[0];
        else
            dialogoText.text = "";

        ShowOptions(dialogue.opciones);
    }

    void ShowOptions(DialogueOption[] options)
    {
        // 🔹 Limpia las opciones anteriores
        foreach (Transform child in opcionesPanel.transform)
            Destroy(child.gameObject);

        // 🔹 Si no hay opciones, cerrar después de mostrar la oración
        if (options == null || options.Length == 0)
        {
            Invoke(nameof(EndDialogue), 2f); // Espera 2 segundos antes de cerrar
            return;
        }

        // 🔹 Crea nuevas opciones
        foreach (var option in options)
        {
            var button = Instantiate(opcionBotonPrefab, opcionesPanel.transform);

            // 🔹 Crea una variable local para esta iteración
            DialogueOption currentOption = option;

            button.GetComponentInChildren<TMP_Text>().text = currentOption.textoOpcion;
            button.onClick.AddListener(() => OnOptionSelected(currentOption));
        }
    }

    void OnOptionSelected(DialogueOption option)
    {
        // 🔹 Limpia opciones anteriores
        foreach (Transform child in opcionesPanel.transform)
            Destroy(child.gameObject);

        if (option.siguienteDialogo != null)
            StartDialogue(option.siguienteDialogo);
        else
            EndDialogue();
    }

    public void EndDialogue()
    {
        // 🔹 Limpieza visual
        dialogoText.text = "";
        npcNombreText.text = "";
        foreach (Transform child in opcionesPanel.transform)
            Destroy(child.gameObject);

        dialogoPanel.SetActive(false);
        currentDialogue = null;
    }

    // 🔹 Permite cerrar el diálogo manualmente desde otro script (por ejemplo, al alejarse del NPC)
    public void CloseDialogue()
    {
        EndDialogue();
    }
}
