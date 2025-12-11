using UnityEngine;

public class Cell : MonoBehaviour
{
    // Cell base health
    public int maxHealth = 3;
    int health;

    // Art for different health amounts
    public Sprite sprite3;
    public Sprite sprite2;
    public Sprite sprite1;

    SpriteRenderer sr;

    void Awake()
    {
        health = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        UpdateSprite();

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.RegisterCell(this);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Only get hurt by bullets
        Projectile projectile = other.GetComponent<Projectile>();
        if (projectile == null) return;

        // Take damage
        health -= 1;
        UpdateSprite();

        // Destroy bullet
        Destroy(other.gameObject);

        // Cell death/destruction
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (!Application.isPlaying) return;

        // Tell the LevelManager when the cell is killed
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.UnregisterCell(this);
        }
    }

    void UpdateSprite()
    {
        if (sr == null) return;

        // Change art based on health
        if (health >= 3)
        {
            sr.sprite = sprite3;
        }
        else if (health == 2)
        {
            sr.sprite = sprite2;
        }
        else if (health == 1)
        {
            sr.sprite = sprite1;
        }
        else
        {
            sr.sprite = null;
        }
    }
}