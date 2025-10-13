using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogo : MonoBehaviour
{
    public string npcNombre;
    [TextArea(2, 5)]
    public string[] oraciones;  // Líneas que dice el NPC
    public DialogueOption[] opciones; // Respuestas posibles
}

[System.Serializable]
public class DialogueOption
{
    public string textoOpcion; // Texto de la respuesta
    public Dialogo siguienteDialogo; // Hacia dónde lleva la respuesta (opcional)
}
