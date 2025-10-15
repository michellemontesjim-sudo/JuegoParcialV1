using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscenas : MonoBehaviour
{
    public void Menu()
    {
        SceneManager.LoadScene("Menú", LoadSceneMode.Single);
    }

    public void IniciarJuego()
    {
        SceneManager.LoadScene("Cinematica", LoadSceneMode.Single);
    }
    
    public void opciones()
    {
        SceneManager.LoadScene("Ajustes", LoadSceneMode.Single);
    }
    
    public void salirJuego()
    {
        //Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // Para el juego compilado
#endif
    }
}
