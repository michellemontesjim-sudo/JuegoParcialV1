using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cofre : MonoBehaviour
{
    public Animator animator;
    private bool jugadorCerca = false;
    public int oro = 100;
    public GameObject panelDialogo;       // Panel del diálogo normal
    public TextMeshProUGUI textoDialogo;  // Texto del diálogo normal

    public GameObject panelFinal;         // Panel negro final
    public TextMeshProUGUI textoFinal;    // Texto del panel final
    public float fadeDuration = 2f;       // Duración del fade

    private bool cofreAbierto = false;

    private void Start()
    {
        panelDialogo.SetActive(false);
        panelFinal.SetActive(false);  // Asegurar que el panel final esté apagado al inicio
    }

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            if (!cofreAbierto)
            {
                animator.SetBool("IsOpen", true);
                StartCoroutine(DialogoSecuencial());
                cofreAbierto = true;
            }
            else
            {
                textoDialogo.text = "El cofre ya está vacío...";
            }
        }
    }

    IEnumerator DialogoSecuencial()
    {
        textoDialogo.text = "¡Has recibido " + oro + " de oro!";
        yield return new WaitForSeconds(2f);

        textoDialogo.text = "Queremos recompensarte por esta gran batalla que tuviste...";
        yield return new WaitForSeconds(3f);

        textoDialogo.text = "Sabemos que no fue fácil para ti, viajero...";
        yield return new WaitForSeconds(3f);

        textoDialogo.text = "Continúa con tu camino, gran héroe.";
        yield return new WaitForSeconds(3f);

        panelDialogo.SetActive(false);
        StartCoroutine(MostrarFinal()); // Aquí aparece el panel final
    }

    IEnumerator MostrarFinal()
    {
        panelFinal.SetActive(true);
        Image panelImage = panelFinal.GetComponent<Image>();

        Color colorFondo = panelImage.color;
        colorFondo.a = 0;
        panelImage.color = colorFondo;

        textoFinal.color = new Color(textoFinal.color.r, textoFinal.color.g, textoFinal.color.b, 0);

        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = t / fadeDuration;

            panelImage.color = new Color(colorFondo.r, colorFondo.g, colorFondo.b, alpha);
            textoFinal.color = new Color(textoFinal.color.r, textoFinal.color.g, textoFinal.color.b, alpha);

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            panelDialogo.SetActive(true);
            textoDialogo.text = "Presiona E para abrir el cofre";
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;

            if (panelDialogo != null)
            {
                panelDialogo.SetActive(false);
            }
        }
    }
}