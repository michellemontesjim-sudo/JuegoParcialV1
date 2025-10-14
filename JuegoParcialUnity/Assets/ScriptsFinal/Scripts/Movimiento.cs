using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [Header("Movimiento")]
    public float velocidad = 5f;
    private float horizontal;
    private bool mirandoDerecha = true;

    [Header("Salto")]
    public float speedSalto = 8f;
    public Transform checkPiso;
    public LayerMask layerPiso;

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, speedSalto);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        voltear();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * velocidad, rb.velocity.y);
        //rb.AddForce(Vector2.right * horizontal * velocidad);
    }

    public bool isGrounded()
    {
        return Physics2D.OverlapCircle(checkPiso.position, 0.2f, layerPiso);
    }

    private void voltear()
    {
        if (mirandoDerecha && horizontal < 0f || !mirandoDerecha && horizontal > 0f)
        {
            mirandoDerecha = !mirandoDerecha;

            /*Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;*/

            transform.Rotate(0f, 180f, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Moneda")
        {
            GameManager.Instance.SetMonedas();

            Debug.Log(GameManager.Instance.cantMonedas);

            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlataformaMovil")
        {
            transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlataformaMovil")
        {
            transform.parent = null;
        }
    }
}
