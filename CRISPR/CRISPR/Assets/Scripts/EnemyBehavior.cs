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
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;
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
        if (!c.collider.CompareTag("Player")) return;
        HandlePlayerCollision(c.collider.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        HandlePlayerCollision(other.gameObject);
    }

    void HandlePlayerCollision(GameObject playerGO)
    {
        PlayerController pc = playerGO.GetComponent<PlayerController>();
        if (pc == null)
        {
            return;
        }

        int playerSize = pc.size;
        Debug.Log("Enemy collided. enemySize=" + size + " playerSize=" + playerSize);

        if (size > playerSize)
        {
            Debug.Log("Enemy kills player.");
            Destroy(playerGO);
        }
        else if (size < playerSize)
        {
            Debug.Log("Player destroys enemy and grows.");
            pc.AddSize(1);

            SpawnManager sm = UnityEngine.Object.FindFirstObjectByType<SpawnManager>();
            if (sm != null) sm.NotifyEnemyDestroyed(this.gameObject);

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("nothing happen.");
        }
    }
}