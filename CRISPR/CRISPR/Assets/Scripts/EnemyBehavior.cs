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
        GameObject[] candidates = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject g in candidates)
        {
            if (g.GetComponent<PlayerController>() != null)
            {
                player = g.transform;
                break;
            }
        }
        PickDirection();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) PickDirection();
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            float d = Vector2.Distance(transform.position, player.position);
            if (d < chaseRange)
            {
                Vector2 chase = ((Vector2)player.position - rb.position).normalized;
                rb.MovePosition(rb.position + chase * moveSpeed * Time.fixedDeltaTime);
                return;
            }
        }
        rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);
    }

    void PickDirection()
    {
        dir = Random.insideUnitCircle.normalized;
        timer = wanderChangeInterval;
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        PlayerController pc = c.collider.GetComponent<PlayerController>();
        if (pc == null) return;
        HandlePlayerCollision(c.collider.gameObject);
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

        if (size > playerSize)
        {
            Destroy(playerGO);
        }
        else if (size < playerSize)
        {
            pc.AddSize(1);
            SpawnManager sm = UnityEngine.Object.FindFirstObjectByType<SpawnManager>();
            if (sm != null) sm.NotifyEnemyDestroyed(this.gameObject);
            Destroy(gameObject);
        }
        // if equal size do nothing
    }
}