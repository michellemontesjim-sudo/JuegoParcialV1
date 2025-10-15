using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LupusController : MonoBehaviour
{
    [Header("Stats Lupus - EXACTAS DEL PDF")]
    public int fuerza = 20;
    public float resistenciaMaxima = 82f;
    public float resistenciaActual = 82f;

    [Header("Ataques")]
    public AtaqueData[] ataques = new AtaqueData[]
    {
        new AtaqueData { nombre = "Garras Oscuras", dados = new int[] { 4, 6 } }, // 1D4 + 1D6
        new AtaqueData { nombre = "Hechizo Maldito", dados = new int[] { 6, 6 } }  // 2D6
    };

    [Header("Configuración")]
    public float cooldownAtaque = 2f;
    public float probabilidadRegeneracion = 0.3f;
    public int regeneracionVida = 10;

    [Header("Componentes")]
    public Animator animator;
    public AudioSource audioSource;
    public GameObject efectoDanio;
    public AudioClip sonidoAtaque;
    public AudioClip sonidoDanio;
    public AudioClip sonidoMuerte;

    // Referencias
    private HelenaController helena;
    private BossSceneManager bossManager;
    private float tiempoSiguienteAtaque;
    private bool muerto = false;

    private void Start()
    {
        helena = FindObjectOfType<HelenaController>();
        bossManager = FindObjectOfType<BossSceneManager>();

        if (animator == null) animator = GetComponent<Animator>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        resistenciaActual = resistenciaMaxima;
    }

    private void Update()
    {
        if (muerto || bossManager == null || !bossManager.batallaActiva) return;

        if (Time.time >= tiempoSiguienteAtaque)
        {
            StartCoroutine(EjecutarTurno());
            tiempoSiguienteAtaque = Time.time + cooldownAtaque;
        }
    }

    private IEnumerator EjecutarTurno()
    {
        // Decidir acción: 70% ataque normal, 30% regeneración
        if (Random.value <= probabilidadRegeneracion && resistenciaActual < resistenciaMaxima)
        {
            // Regeneración
            resistenciaActual = Mathf.Min(resistenciaActual + regeneracionVida, resistenciaMaxima);
            NotificarUI($"Lupus se regenera {regeneracionVida} de vida");
            animator.SetTrigger("Regenerar");
        }
        else
        {
            // Ataque normal o especial
            if (Random.value <= 0.4f) // 40% probabilidad de ataque especial
            {
                yield return StartCoroutine(AtaqueEspecial());
            }
            else
            {
                yield return StartCoroutine(AtaqueNormal());
            }
        }
    }

    private IEnumerator AtaqueNormal()
    {
        animator.SetTrigger("Atacar");
        yield return new WaitForSeconds(0.3f);

        // Seleccionar ataque aleatorio
        AtaqueData ataque = ataques[Random.Range(0, ataques.Length)];
        int danio = SistemaDados.CalcularDanioAtaque(ataque.dados);

        // Aplicar daño a héroe aleatorio
        HeroeController heroe = ObtenerHeroeAleatorio();
        if (heroe != null)
        {
            heroe.RecibirDanio(danio);
            NotificarUI($"Lupus usa {ataque.nombre}: {danio} daño");
        }

        if (audioSource != null && sonidoAtaque != null)
            audioSource.PlayOneShot(sonidoAtaque);
    }

    private IEnumerator AtaqueEspecial()
    {
        // Tirada especial para llamar a Helena
        ResultadoLupus resultado = SistemaDados.RealizarTiradaLupus(GameManager.Instance.helenaMuerta);

        if (resultado.esPifia)
        {
            NotificarUI($"Lupus: {resultado.mensaje}");
        }
        else if (resultado.esExito)
        {
            if (resultado.atacaHelena && helena != null && !GameManager.Instance.helenaMuerta)
            {
                // Helena ataca
                helena.EjecutarAtaqueAyuda(resultado.danio);
                NotificarUI(resultado.mensaje);
            }
            else
            {
                // Lupus ataca solo
                HeroeController heroe = ObtenerHeroeAleatorio();
                if (heroe != null)
                {
                    heroe.RecibirDanio(resultado.danio);
                    NotificarUI(resultado.mensaje);
                }
            }
        }

        yield return new WaitForSeconds(0.5f);
    }

    public void RecibirDanio(int danio)
    {
        if (muerto) return;

        resistenciaActual -= danio;
        resistenciaActual = Mathf.Max(0, resistenciaActual);

        // Efectos
        animator.SetTrigger("Danio");
        if (efectoDanio != null)
            Instantiate(efectoDanio, transform.position, Quaternion.identity);

        if (audioSource != null && sonidoDanio != null)
            audioSource.PlayOneShot(sonidoDanio);

        if (resistenciaActual <= 0)
            Morir();
    }

    private void Morir()
    {
        muerto = true;
        GameManager.Instance.lupusMuerto = true;

        animator.SetTrigger("Morir");
        if (audioSource != null && sonidoMuerte != null)
            audioSource.PlayOneShot(sonidoMuerte);

        // Notificar al boss manager
        if (bossManager != null)
            bossManager.JefeDerrotado();

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