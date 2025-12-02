using UnityEngine;

public class Cell : MonoBehaviour
{
    public int maxHealth = 3;
    int health;

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
            LevelManager.Instance.RegisterCell(this);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Projectile p = other.GetComponent<Projectile>();
        if (p == null) return;

        health -= 1;
        UpdateSprite();
        Destroy(other.gameObject);

        if (health <= 0)
            Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (!Application.isPlaying) return;
        if (LevelManager.Instance != null)
            LevelManager.Instance.UnregisterCell(this);
    }

    void UpdateSprite()
    {
        if (sr == null) return;

        if (health >= 3) sr.sprite = sprite3;
        else if (health == 2) sr.sprite = sprite2;
        else if (health == 1) sr.sprite = sprite1;
        else sr.sprite = null;
    }
}