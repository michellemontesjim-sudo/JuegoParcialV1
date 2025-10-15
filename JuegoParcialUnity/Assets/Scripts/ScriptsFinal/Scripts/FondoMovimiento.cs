using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FondoMovimiento : MonoBehaviour
{
    [SerializeField] private Vector2 velocidadMovimiento;
    private Vector2 offset;
    private Material material;
    private Rigidbody2D jugadorRB;

    private void Awake()
    {
        // Intentar obtener el material desde SpriteRenderer o MeshRenderer
        if (TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            material = spriteRenderer.material;
        }
        else if (TryGetComponent<MeshRenderer>(out var meshRenderer))
        {
            material = meshRenderer.material;
        }
        else
        {
            Debug.LogError("[FondoMovimiento] No se encontró ni SpriteRenderer ni MeshRenderer en este objeto.", gameObject);
        }

        // Obtener el Rigidbody2D del jugador
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
        {
            jugadorRB = jugador.GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogError("[FondoMovimiento] No se encontró un objeto con tag 'Player'.");
        }
    }

    private void Update()
    {
        if (material == null || jugadorRB == null) return;

        offset = new Vector2(jugadorRB.velocity.x * velocidadMovimiento.x, jugadorRB.velocity.y * velocidadMovimiento.y) * Time.deltaTime;
        material.mainTextureOffset += offset;
    }

}
