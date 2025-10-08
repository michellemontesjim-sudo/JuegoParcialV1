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
        SceneManager.LoadScene("Pueblo", LoadSceneMode.Single);
    }

    public void salirJuego()
    {
        Application.Quit();
    }
}
