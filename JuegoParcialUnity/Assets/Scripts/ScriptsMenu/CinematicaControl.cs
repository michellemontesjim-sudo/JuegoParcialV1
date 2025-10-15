using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CinematicaControl : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    //public string siguienteEscena = "Juego"; // cambia por el nombre real de tu escena
    public Button skipButton;

    void Start()
    {
        
        videoPlayer.Play();

        
        videoPlayer.loopPointReached += VideoTerminado;

        // Configura el botón Skip
        if (skipButton != null)
            skipButton.onClick.AddListener(SkipCinematica);
    }

    void VideoTerminado(VideoPlayer vp)
    {
        SceneManager.LoadScene("Pueblo");
    }

    void SkipCinematica()
    {
        SceneManager.LoadScene("Pueblo");
    }
}
