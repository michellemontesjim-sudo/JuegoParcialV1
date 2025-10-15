using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HeroeController : MonoBehaviour
{
    [Header("Configuración Héroe")]
    public int numeroHeroe = 1;

    [Header("Movimiento")]
    public float velocidad = 5f;
    public float fuerzaSalto = 10f;

    [Header("Combate")]
    public Transform puntoAtaque;
    public float rangoAtaque = 1f;
    public LayerMask capaEnemigos;
    public float cooldownAtaque = 1f;

    [Header("Componentes")]
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public AudioSource audioSource;

    [Header("Efectos")]
    public GameObject efectoAtaque;
    public AudioClip sonidoAtaque;
    public AudioClip sonidoDanio;
    public AudioClip sonidoMuerte;

    // Privadas
    private DatosHeroe datos;
    private Rigidbody2D rb;
    private float inputHorizontal;
    private bool mirandoDerecha = true;
    private bool enSuelo = true;
    private float tiempoUltimoAtaque;
    private int ataqueSeleccionado = 0;
    private bool puedeMoverse = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (animator == null) animator = GetComponent<Animator>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        // Obtener datos del GameManager
        if (GameManager.Instance != null)
        {
            datos = GameManager.Instance.ObtenerDatosHeroe(numeroHeroe);
            if (datos == null) Debug.LogError($"No se encontraron datos para héroe {numeroHeroe}");
        }
    }

    private void Update()
    {
        if (datos == null || !datos.vivo || !puedeMoverse) return;

        // Movimiento
        inputHorizontal = Input.GetAxisRaw("Horizontal");

        // Salto
        if (Input.GetButtonDown("Jump") && enSuelo)
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
            animator.SetTrigger("Saltar");
        }

        // Ataque
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= tiempoUltimoAtaque + cooldownAtaque)
        {
            StartCoroutine(EjecutarAtaque());
        }

        // Cambiar ataque (solo héroe 2)
        if (numeroHeroe == 2 && datos.ataques.Count > 1)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) ataqueSeleccionado = 0;
            if (Input.GetKeyDown(KeyCode.Alpha2)) ataqueSeleccionado = 1;
            if (Input.GetKeyDown(KeyCode.Alpha3)) ataqueSeleccionado = 2;
        }

        // Animaciones
        animator.SetFloat("VelocidadX", Mathf.Abs(inputHorizontal));
        animator.SetBool("EnSuelo", enSuelo);

        // Voltear sprite
        if ((inputHorizontal > 0 && !mirandoDerecha) || (inputHorizontal < 0 && mirandoDerecha))
            Voltear();
    }

    private void FixedUpdate()
    {
        if (!puedeMoverse || datos == null || !datos.vivo) return;
        rb.velocity = new Vector2(inputHorizontal * velocidad, rb.velocity.y);
    }

    private IEnumerator EjecutarAtaque()
    {
        tiempoUltimoAtaque = Time.time;
        animator.SetTrigger("Atacar");

        // Esperar frame de animación
        yield return new WaitForSeconds(0.2f);

        // 1. Tirada de éxito
        ResultadoExito exito = SistemaDados.RealizarTiradaExito();

        if (exito.esPifia)
        {
            NotificarUI($"Héroe {numeroHeroe}: ¡PIFIA! Falló el ataque");
            yield break;
        }

        // 2. Tirada de daño
        ResultadoDanio danio = SistemaDados.RealizarTiradaDanio(datos.fuerza);

        if (danio.tipo == TipoResultadoDanio.Pifia || danio.tipo == TipoResultadoDanio.Debil)
        {
            NotificarUI($"Héroe {numeroHeroe}: {danio.mensaje}");
            yield break;
        }

        // 3. Calcular daño del ataque
        AtaqueData ataque = datos.ataques[Mathf.Clamp(ataqueSeleccionado, 0, datos.ataques.Count - 1)];
        int danioFinal = SistemaDados.CalcularDanioAtaque(ataque.dados);

        // Efectos
        if (efectoAtaque != null)
            Instantiate(efectoAtaque, puntoAtaque.position, Quaternion.identity);

        if (audioSource != null && sonidoAtaque != null)
            audioSource.PlayOneShot(sonidoAtaque);

        // Aplicar daño a enemigos en rango
        Collider2D[] enemigos = Physics2D.OverlapCircleAll(puntoAtaque.position, rangoAtaque, capaEnemigos);

        foreach (Collider2D enemigo in enemigos)
        {
            LupusController lupus = enemigo.GetComponent<LupusController>();
            HelenaController helena = enemigo.GetComponent<HelenaController>();

            if (lupus != null) lupus.RecibirDanio(danioFinal);
            if (helena != null) helena.RecibirDanio(danioFinal);
        }

        NotificarUI($"Héroe {numeroHeroe} causa {danioFinal} de daño!");
    }

    public void RecibirDanio(int danio)
    {
        if (datos == null || !datos.vivo) return;

        datos.resistenciaActual -= danio;
        GameManager.Instance.ActualizarVidaHeroe(numeroHeroe, datos.resistenciaActual);

        // Efectos
        StartCoroutine(EfectoDanioVisual());
        if (audioSource != null && sonidoDanio != null)
            audioSource.PlayOneShot(sonidoDanio);

        if (datos.resistenciaActual <= 0)
            Morir();
    }

    private IEnumerator EfectoDanioVisual()
    {
        if (spriteRenderer != null)
        {
            Color original = spriteRenderer.color;
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = original;
        }
    }

    private void Morir()
    {
        datos.vivo = false;
        animator.SetTrigger("Morir");

        if (audioSource != null && sonidoMuerte != null)
            audioSource.PlayOneShot(sonidoMuerte);

        // Desactivar colisiones y movimiento
        GetComponent<Collider2D>().enabled = false;
        puedeMoverse = false;
        rb.velocity = Vector2.zero;
    }

    private void Voltear()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void NotificarUI(string mensaje)
    {
        // Buscar UI Manager en la escena
        UIBossManager ui = FindObjectOfType<UIBossManager>();
        if (ui != null) ui.AgregarNotificacion(mensaje);
    }

    // Detección de suelo
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
            enSuelo = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
            enSuelo = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (puntoAtaque != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(puntoAtaque.position, rangoAtaque);
        }
    }
}