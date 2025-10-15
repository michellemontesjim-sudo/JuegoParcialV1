using UnityEngine;

public class ExtraLifePickup : MonoBehaviour
{
    public int giveLives = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            for (int i = 0; i < giveLives; i++) player.AddLife();

            Destroy(gameObject);
        }
    }
}
