using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBossManager : MonoBehaviour
{
    [Header("Barras de Vida - Héroes")]
    public Slider[] barrasVidaHeroes;
    public Text[] textosVidaHeroes;

    [Header("Barras de Vida - Jefes")]
    public Slider barraVidaLupus;
    public Slider barraVidaHelena;
    public Text textoVidaLupus;
    public Text textoVidaHelena;

    [Header("Panel Notificaciones")]
    public GameObject panelNotificaciones;
    public Text textoNotificacion;
    public int maxNotificaciones = 5;

    [Header("Panel Inventario")]
    public Text textoDinero;
    public Text textoPociones;
    public Text textoCofres;

    [Header("Panel Diálogo")]
    public GameObject panelDialogo;
    public Text textoDialogo;

    [Header("UI Combate")]
    public GameObject uiCombate;

    private Queue<string> colaNotificaciones = new Queue<string>();
    private bool mostrandoNotificacion = false;

    private void Start()
    {
        OcultarUICombate();
        OcultarDialogo();
    }

    private void Update()
    {
        ActualizarUI();

        if (!mostrandoNotificacion && colaNotificaciones.Count > 0)
        {
            StartCoroutine(MostrarSiguienteNotificacion());
        }
    }

    private void ActualizarUI()
    {
        // Actualizar héroes
        for (int i = 0; i < barrasVidaHeroes.Length; i++)
        {
            if (i < GameManager.Instance.datosHeroes.Count)
            {
                DatosHeroe heroe = GameManager.Instance.datosHeroes[i];
                barrasVidaHeroes[i].value = heroe.PorcentajeVida;
                textosVidaHeroes[i].text = $"{heroe.resistenciaActual:F0}/{heroe.resistenciaMaxima:F0}";

                // Cambiar color según salud
                Image fill = barrasVidaHeroes[i].fillRect.GetComponent<Image>();
                fill.color = heroe.PorcentajeVida > 0.5f ? Color.green :
                            heroe.PorcentajeVida > 0.25f ? Color.yellow : Color.red;
            }
        }

        // Actualizar jefes
        if (barraVidaLupus != null)
        {
            LupusController lupus = FindObjectOfType<LupusController>();
            if (lupus != null)
            {
                barraVidaLupus.value = lupus.resistenciaActual / lupus.resistenciaMaxima;
                textoVidaLupus.text = $"{lupus.resistenciaActual:F0}/{lupus.resistenciaMaxima:F0}";
            }
        }

        if (barraVidaHelena != null)
        {
            HelenaController helena = FindObjectOfType<HelenaController>();
            if (helena != null)
            {
                barraVidaHelena.value = helena.resistenciaActual / helena.resistenciaMaxima;
                textoVidaHelena.text = $"{helena.resistenciaActual:F0}/{helena.resistenciaMaxima:F0}";
            }
        }

        // Actualizar inventario
        textoDinero.text = $"{GameManager.Instance.dinero}G";
        textoPociones.text = $"x{GameManager.Instance.inventario.GetValueOrDefault("Pociones", 0)}";
        textoCofres.text = $"x{GameManager.Instance.inventario.GetValueOrDefault("Cofres", 0)}";
    }

    public void AgregarNotificacion(string mensaje)
    {
        colaNotificaciones.Enqueue(mensaje);

        // Limitar cola
        while (colaNotificaciones.Count > maxNotificaciones)
            colaNotificaciones.Dequeue();
    }

    private IEnumerator MostrarSiguienteNotificacion()
    {
        mostrandoNotificacion = true;

        string mensaje = colaNotificaciones.Dequeue();
        textoNotificacion.text = mensaje;
        panelNotificaciones.SetActive(true);

        yield return new WaitForSeconds(2f);

        panelNotificaciones.SetActive(false);
        mostrandoNotificacion = false;
    }

    public void MostrarUICombate()
    {
        if (uiCombate != null)
            uiCombate.SetActive(true);
    }

    public void OcultarUICombate()
    {
        if (uiCombate != null)
            uiCombate.SetActive(false);
    }

    public void MostrarDialogo(string texto)
    {
        if (panelDialogo != null)
        {
            panelDialogo.SetActive(true);
            textoDialogo.text = texto;
        }
    }

    public void OcultarDialogo()
    {
        if (panelDialogo != null)
            panelDialogo.SetActive(false);
    }

    public void MostrarRecompensas(int dinero, int cofres, List<string> objetos)
    {
        string mensaje = $"<color=yellow>¡RECOMPENSAS OBTENIDAS!</color>\n";
        mensaje += $"Dinero: {dinero}G\n";
        mensaje += $"Cofres: {cofres}\n";

        if (objetos.Count > 0)
        {
            mensaje += "Objetos:\n";
            foreach (string objeto in objetos)
                mensaje += $"- {objeto}\n";
        }

        AgregarNotificacion(mensaje);
    }
}