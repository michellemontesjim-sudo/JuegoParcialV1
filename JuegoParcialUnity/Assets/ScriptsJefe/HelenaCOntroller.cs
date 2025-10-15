using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelenaController : MonoBehaviour
{
    [Header("Stats Helena - EXACTAS DEL PDF")]
    public int fuerza = 18;
    public float resistenciaMaxima = 35f;
    public float resistenciaActual = 35f;

    [Header("Ataque")]
    public AtaqueData ataque = new AtaqueData
    {
        nombre = "Garras Demoníacas",
        dados = new int[] { 8, 6 } // 1D8 + 1D6
    };

    [Header("Componentes")]
    public Animator animator;
    public AudioSource audioSource;
    public GameObject efectoTransformacion;
    public AudioClip sonidoAtaque;
    public AudioClip sonidoDanio;
    public AudioClip sonidoMuerte;

    // Estado
    private bool muerta = false;
    private bool transformada = false;
    private BossSceneManager bossManager;

    private void Start()
    {
        bossManager = FindObjectOfType<BossSceneManager>();
        if (animator == null) animator = GetComponent<Animator>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        resistenciaActual = resistenciaMaxima;
    }

    public void Transformar()
    {
        if (transformada) return;

        transformada = true;
        animator.SetBool("Transformada", true);

        if (efectoTransformacion != null)
            Instantiate(efectoTransformacion, transform.position, Quaternion.identity);

        NotificarUI("¡Helena revela su forma demoníaca!");
    }

    public void EjecutarAtaqueAyuda(int danioExtra = 0)
    {
        if (muerta || !transformada) return;

        StartCoroutine(EjecutarAtaque(danioExtra));
    }

    private IEnumerator EjecutarAtaque(int danioExtra = 0)
    {
        animator.SetTrigger("Atacar");
        yield return new WaitForSeconds(0.3f);

        int danio = SistemaDados.CalcularDanioAtaque(ataque.dados) + danioExtra;

        HeroeController heroe = ObtenerHeroeAleatorio();
        if (heroe != null)
        {
            heroe.RecibirDanio(danio);
            NotificarUI($"Helena causa {danio} de daño");
        }

        if (audioSource != null && sonidoAtaque != null)
            audioSource.PlayOneShot(sonidoAtaque);
    }

    public void RecibirDanio(int danio)
    {
        if (muerta) return;

        resistenciaActual -= danio;
        resistenciaActual = Mathf.Max(0, resistenciaActual);

        animator.SetTrigger("Danio");
        if (audioSource != null && sonidoDanio != null)
            audioSource.PlayOneShot(sonidoDanio);

        if (resistenciaActual <= 0)
            Morir();
    }

    private void Morir()
    {
        muerta = true;
        GameManager.Instance.helenaMuerta = true;

        animator.SetTrigger("Morir");
        if (audioSource != null && sonidoMuerte != null)
            audioSource.PlayOneShot(sonidoMuerte);

        // Notificar a Lupus
        LupusController lupus = FindObjectOfType<LupusController>();
        if (lupus != null)
        {
            // Lupus se vuelve más agresivo
            NotificarUI("¡Lupus entra en furia al ver morir a Helena!");
        }

        // Desactivar colisiones
        GetComponent<Collider2D>().enabled = false;
    }

    private HeroeController ObtenerHeroeAleatorio()
    {
        HeroeController[] heroes = FindObjectsOfType<HeroeController>();
        List<HeroeController> heroesVivos = new List<HeroeController>();

        foreach (HeroeController heroe in heroes)
        {
            if (heroe.GetComponent<DatosHeroe>()?.vivo == true)
                heroesVivos.Add(heroe);
        }

        return heroesVivos.Count > 0 ? heroesVivos[Random.Range(0, heroesVivos.Count)] : null;
    }

    private void NotificarUI(string mensaje)
    {
        UIBossManager ui = FindObjectOfType<UIBossManager>();
        if (ui != null) ui.AgregarNotificacion(mensaje);
    }
}