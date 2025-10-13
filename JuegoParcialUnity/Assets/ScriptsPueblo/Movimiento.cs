using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [Header("Movimiento")]
    public float velocidad = 5f;
    public float horizontal;
    public bool mirandoDerecha = true;
    public bool attack = false;
    public Transform firePoint;

    [Header("Salto")]
    public float speedSalto = 6f;
    public Transform checkPiso;
    public LayerMask layerPiso;

    [Header("Animación")]
    public Animator anim;

    private Vector2 startPoint;
    private Vector3 respawnPoint;

    //[Header("Audio")]
    //public AudioSource saltar;



    public void Awake()
    {
        startPoint = rb.position;
        respawnPoint = startPoint;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(horizontal * velocidad, rb.velocity.y);

        if (horizontal > 0 || horizontal < 0)
        {
            anim.SetFloat("Caminar", Mathf.Abs(horizontal));

        }
        else
        {
            anim.SetFloat("Caminar", 0);
        }

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, speedSalto);
            anim.SetBool("Saltar", true);
            //saltar.Play();
        }

        if (isGrounded() && rb.velocity.y <= 0)
        {
            anim.SetBool("Saltar", false);
        }

        voltear();

        if (horizontal != 0)
        {
            anim.SetFloat("Caminar", Mathf.Abs(horizontal));
        }
        else
        {
            anim.SetFloat("Caminar", 0);
        }

        // Saltar
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, speedSalto);
            anim.SetBool("Saltar", true);
            //saltar.Play();
        }


        if (isGrounded() && rb.velocity.y <= 0)
        {
            anim.SetBool("Saltar", false);
        }


        voltear();


    }

    public bool isGrounded()
    {
        return Physics2D.OverlapCircle(checkPiso.position, 0.2f, layerPiso);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * velocidad, rb.velocity.y);
    }



    void voltear()
    {

        if (horizontal > 0 && !mirandoDerecha)
        {
            Girar();
        }

        else if (horizontal < 0 && mirandoDerecha)
        {
            Girar();
        }
    }

    void Girar()
    {
        mirandoDerecha = !mirandoDerecha;


        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;


        if (firePoint != null)
        {
            firePoint.Rotate(0f, 0f, 180f);
        }
    }
}
