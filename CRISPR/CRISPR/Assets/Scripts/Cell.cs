using UnityEngine;

public class Cell : MonoBehaviour
{
    public int maxHealth = 3;
    int health;

    void Awake()
    {
        health = maxHealth;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Projectile p = other.GetComponent<Projectile>();
        if (p == null) return;

        health -= 1;
        Debug.Log("Cell hit. Health = " + health);

        Destroy(other.gameObject);

        if (health <= 0)
        {
            Debug.Log("Cell destroyed!");
            Destroy(gameObject);
        }
    }
}
