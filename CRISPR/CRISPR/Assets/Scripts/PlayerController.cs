using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int size = 1;
    public float sizeScaleStep = 0.2f;

    [Header("Shooting")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 8f;
    public float shootCooldown = 0.5f;

    Rigidbody2D rb;
    Vector2 moveInput;
    Vector3 baseScale;
    float shootTimer = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        baseScale = transform.localScale;
        ApplyScale();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(x, y).normalized;

        if (shootTimer > 0f) shootTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && size >= 3 && shootTimer <= 0f)
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    public void AddSize(int amount)
    {
        if (amount <= 0) return;
        size += amount;
        Debug.Log("Player size = " + size);
        ApplyScale();
    }

    void ApplyScale()
    {
        float mul = 1f + sizeScaleStep * (size - 1);
        transform.localScale = baseScale * mul;
    }

    void Shoot()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("No projectile prefab.");
            return;
        }

        Vector3 spawnPos = (firePoint != null) ? firePoint.position : transform.position;
        GameObject p = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Vector2 dir = moveInput.sqrMagnitude > 0.001f ? moveInput.normalized : Vector2.up;
        Rigidbody2D prb = p.GetComponent<Rigidbody2D>();
        if (prb != null)
        {
            prb.linearVelocity = dir * projectileSpeed;
            prb.gravityScale = 0f;
        }

        Projectile ps = p.GetComponent<Projectile>();
        if (ps != null) ps.Initialize(1, sizeScaleStep);

        Collider2D pc = GetComponent<Collider2D>();
        Collider2D projCol = p.GetComponent<Collider2D>();
        if (pc != null && projCol != null) Physics2D.IgnoreCollision(projCol, pc, true);

        // reset player
        size = 1;
        ApplyScale();
        shootTimer = shootCooldown;
    }
}