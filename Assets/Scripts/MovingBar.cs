using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class MovingBar : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 100f;

    private Rigidbody2D rb;
    private Camera mainCamera;
    private float screenBuffer = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        RepositionBar();
    }

    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        // Si se fue completamente fuera del viewport, la barrita hace su truco de desaparición y reaparición
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);
        if (viewportPos.x < -0.2f || viewportPos.x > 1.2f || viewportPos.y < -0.2f || viewportPos.y > 1.2f)
        {
            RepositionBar();
        }
    }

    void RepositionBar()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 spawnPos = Vector2.zero;
        Vector2 moveDir = Vector2.zero;

        int directionIndex = Random.Range(0, 8); // 0 a 7
        switch (directionIndex)
        {
            case 0: // izquierda
                spawnPos = new Vector2(0 - screenBuffer, Random.Range(0, screenSize.y));
                moveDir = Vector2.right;
                break;
            case 1: // derecha
                spawnPos = new Vector2(screenSize.x + screenBuffer, Random.Range(0, screenSize.y));
                moveDir = Vector2.left;
                break;
            case 2: // abajo
                spawnPos = new Vector2(Random.Range(0, screenSize.x), 0 - screenBuffer);
                moveDir = Vector2.up;
                break;
            case 3: // arriba
                spawnPos = new Vector2(Random.Range(0, screenSize.x), screenSize.y + screenBuffer);
                moveDir = Vector2.down;
                break;
            case 4: // esquina superior izquierda
                spawnPos = new Vector2(0 - screenBuffer, screenSize.y + screenBuffer);
                moveDir = new Vector2(1, -1);
                break;
            case 5: // esquina superior derecha
                spawnPos = new Vector2(screenSize.x + screenBuffer, screenSize.y + screenBuffer);
                moveDir = new Vector2(-1, -1);
                break;
            case 6: // esquina inferior derecha
                spawnPos = new Vector2(screenSize.x + screenBuffer, 0 - screenBuffer);
                moveDir = new Vector2(-1, 1);
                break;
            case 7: // esquina inferior izquierda
                spawnPos = new Vector2(0 - screenBuffer, 0 - screenBuffer);
                moveDir = new Vector2(1, 1);
                break;
        }

        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(spawnPos.x, spawnPos.y, 10f));
        transform.position = worldPos;

        rb.linearVelocity = moveDir.normalized * speed;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (ball != null)
        {
            Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();
            if (ballRb != null)
            {
                Vector2 newDir = (ballRb.position - rb.position).normalized;
                ballRb.linearVelocity = newDir * ballRb.linearVelocity.magnitude;
            }
        }
    }
}
