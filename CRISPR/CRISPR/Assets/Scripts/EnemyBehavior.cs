using UnityEngine;

public class EnemyWander : MonoBehaviour
{
    public int size = 1;
    public float moveSpeed = 2f;
    public float wanderChangeInterval = 2f;
    public float chaseRange = 4f;

    Transform player;
    Rigidbody2D rb;
    Vector2 dir;
    float timer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Try to find the player object (with PlayerController)
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in players)
        {
            if (obj.GetComponent<PlayerController>() != null)
            {
                player = obj.transform;
                break;
            }
        }

        PickDirection();
    }

    void Update()
    {
        // Count down until we pick a new random direction
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            PickDirection();
        }
    }

    void FixedUpdate()
    {
        // If we have a player, check distance and maybe chase
        if (player != null)
        {
            float distToPlayer = Vector2.Distance(transform.position, player.position);
            if (distToPlayer < chaseRange)
            {
                // Move towards the player
                Vector2 chaseDirection = ((Vector2)player.position - rb.position).normalized;
                rb.MovePosition(rb.position + chaseDirection * moveSpeed * Time.fixedDeltaTime);
                return;
            }
        }

        // If not chasing, just wander around
        rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);
    }

    void PickDirection()
    {
        // Pick a random direction to move in
        dir = Random.insideUnitCircle.normalized;
        timer = wanderChangeInterval;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController pc = collision.collider.GetComponent<PlayerController>();
        if (pc == null) return;

        HandlePlayerCollision(collision.collider.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc == null) return;

        HandlePlayerCollision(other.gameObject);
    }

    void HandlePlayerCollision(GameObject playerGO)
    {
        PlayerController pc = playerGO.GetComponent<PlayerController>();
        if (pc == null) return;

        int playerSize = pc.size;

        // If enemy is bigger, kill the player
        if (size > playerSize)
        {
            Destroy(playerGO);
        }
        // If player is bigger, player kills the enemy
        else if (size < playerSize)
        {
            pc.AddSize(1);

            SpawnManager spawnManager = UnityEngine.Object.FindFirstObjectByType<SpawnManager>();
            if (spawnManager != null)
            {
                spawnManager.NotifyEnemyDestroyed(this.gameObject);
            }

            Destroy(gameObject);
        }
    }
}