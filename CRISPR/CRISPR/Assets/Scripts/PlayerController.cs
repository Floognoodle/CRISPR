using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    // Player size
    public int size = 1;
    public float sizeScaleStep = 0.2f;

    [Header("Shooting")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 8f;
    public float shootCooldown = 0.5f;

    [Header("Audio")]
    public AudioClip bumpSound;

    Rigidbody2D rb;
    Vector2 moveInput;
    Vector3 baseScale;
    float shootTimer = 0f;
    AudioSource audioSource;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        baseScale = transform.localScale;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        ApplyScale();
    }

    void Update()
    {
        // Player movement & bullet spam prevention
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(x, y).normalized;

        if (shootTimer > 0f)
        {
            shootTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && size >= 3 && shootTimer <= 0f)
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    // Increase player size
    public void AddSize(int amount)
    {
        if (amount <= 0) return;

        size += amount;

        // Limit size
        if (size > 3)
        {
            size = 3;
        }

        ApplyScale();
    }

    void ApplyScale()
    {
        int clampedSize = Mathf.Clamp(size, 1, 3);

        // Adjust the player art size based on actual size
        float scaleMultiplier = 1f + sizeScaleStep * (clampedSize - 1);
        transform.localScale = baseScale * scaleMultiplier;
    }

    void Shoot()
    {
        if (projectilePrefab == null) return;

        // Projectile rules
        Vector3 spawnPos = (firePoint != null) ? firePoint.position : transform.position;
        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Vector2 shootDir;
        if (moveInput.sqrMagnitude > 0.001f)
        {
            shootDir = moveInput.normalized;
        }
        else
        {
            shootDir = Vector2.up;
        }

        Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();
        if (projRb != null)
        {
            projRb.linearVelocity = shootDir * projectileSpeed;
            projRb.gravityScale = 0f;
        }

        Projectile projScript = proj.GetComponent<Projectile>();
        if (projScript != null)
        {
            projScript.Initialize(1, sizeScaleStep);
        }

        Collider2D playerCollider = GetComponent<Collider2D>();
        Collider2D projCollider = proj.GetComponent<Collider2D>();
        if (playerCollider != null && projCollider != null)
        {
            Physics2D.IgnoreCollision(projCollider, playerCollider, true);
        }

        size = 1;
        ApplyScale();

        shootTimer = shootCooldown;
    }

    void PlayBumpSound()
    {
        if (bumpSound == null) return;

        audioSource.PlayOneShot(bumpSound);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Play a sound when bumping into an enemy
        if (collision.collider.GetComponent<EnemyWander>() != null)
        {
            PlayBumpSound();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<EnemyWander>() != null)
        {
            PlayBumpSound();
        }
    }

    void OnDestroy()
    {
        if (!Application.isPlaying) return;

        if (LevelFailedUI.Instance != null)
        {
            LevelFailedUI.Instance.Show();
        }
    }
}