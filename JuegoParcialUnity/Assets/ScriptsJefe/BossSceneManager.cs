using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossSceneManager : MonoBehaviour
{
    [Header("Referencias")]
    public HeroeController[] heroes;
    public LupusController lupus;
    public HelenaController helena;
    public UIBossManager uiManager;
    public DialogoManager dialogoManager;

    [Header("Configuración Batalla")]
    public float delayEntreTurnos = 1f;
    public bool batallaActiva = false;

    [Header("Recompensas")]
    public int dineroBase = 1000;
    public int cofresBase = 3;
    public GameObject[] objetosRecompensa;

    private bool batallaTerminada = false;

    private void Start()
    {
        IniciarSecuenciaNarrativa();
    }

    private void IniciarSecuenciaNarrativa()
    {
        StartCoroutine(SecuenciaCompleta());
    }

    private IEnumerator SecuenciaCompleta()
    {
        // 1. Diálogo inicial - Despiertan en cueva
        yield return StartCoroutine(dialogoManager.MostrarDialogo(
            "Despiertan en una cueva oscura... Helena no está por ningún lado"));

        yield return new WaitForSeconds(1f);

        // 2. Helena aparece transformada
        yield return StartCoroutine(dialogoManager.MostrarDialogo(
            "Helena: ¡JAJAJA! Idiotas... Me sirvieron perfectamente para liberar a mi maestro!"));

        helena.Transformar();
        yield return new WaitForSeconds(2f);

        // 3. Lupus aparece del espejo
        yield return StartCoroutine(dialogoManager.MostrarDialogo(
            "Lupus: ¡Después de siglos... he escapado de mi prisión! Gracias, fiel Astrega."));

        yield return new WaitForSeconds(1f);

        // 4. Diálogo de desafío
        yield return StartCoroutine(dialogoManager.MostrarDialogo(
            "Lupus: Ahora, morirán por interrumpir mi resurrección. ¡Helena, acaba con ellos!"));

        // 5. Iniciar batalla
        batallaActiva = true;
        uiManager.MostrarUICombate();

        // Monitorear batalla
        StartCoroutine(MonitorearBatalla());
    }

    private IEnumerator MonitorearBatalla()
    {
        while (!batallaTerminada)
        {
            yield return new WaitForSeconds(1f);

            // Verificar si todos los héroes murieron
            if (GameManager.Instance.TodosHeroesMuertos())
            {
                batallaTerminada = true;
                StartCoroutine(FinBatalla(false));
                break;
            }

            // Verificar si jefes murieron
            if (GameManager.Instance.lupusMuerto && GameManager.Instance.helenaMuerta)
            {
                batallaTerminada = true;
                StartCoroutine(FinBatalla(true));
                break;
            }
        }
    }

    public void JefeDerrotado()
    {
        // Verificar si ambos jefes están muertos
        if (GameManager.Instance.lupusMuerto && GameManager.Instance.helenaMuerta && !batallaTerminada)
        {
            batallaTerminada = true;
            StartCoroutine(FinBatalla(true));
        }
    }

    private IEnumerator FinBatalla(bool victoria)
    {
        batallaActiva = false;

        if (victoria)
        {
            yield return StartCoroutine(dialogoManager.MostrarDialogo(
                "¡Han derrotado a Lupus y Helena! La maldición se rompe..."));

            yield return new WaitForSeconds(2f);

            // Mostrar recompensas
            yield return StartCoroutine(GenerarRecompensas());

            yield return StartCoroutine(dialogoManager.MostrarDialogo(
                "La sala se ilumina, revelando un tesoro oculto. ¡Son ricos!"));

            // Transición a escena final
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("Final");
        }
        else
        {
            yield return StartCoroutine(dialogoManager.MostrarDialogo(
                "Todos los héroes han caído... La oscuridad consume el mundo."));

            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("GameOver");
        }
    }

    private IEnumerator GenerarRecompensas()
    {
        // Dinero aleatorio
        int dineroExtra = Random.Range(500, 2000);
        int dineroTotal = dineroBase + dineroExtra;
        GameManager.Instance.AgregarDinero(dineroTotal);

        // Cofres aleatorios
        int cofresExtra = Random.Range(1, 4);
        int cofresTotal = cofresBase + cofresExtra;
        GameManager.Instance.AgregarObjeto("Cofres", cofresTotal);

        // Objetos aleatorios
        List<string> objetosObtenidos = new List<string>();
        for (int i = 0; i < Random.Range(2, 5); i++)
        {
            if (objetosRecompensa.Length > 0)
            {
                GameObject objeto = objetosRecompensa[Random.Range(0, objetosRecompensa.Length)];
                objetosObtenidos.Add(objeto.name);
            }
        }

        // Mostrar en UI
        uiManager.MostrarRecompensas(dineroTotal, cofresTotal, objetosObtenidos);

        yield return new WaitForSeconds(3f);
    }
}