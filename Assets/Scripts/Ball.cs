using UnityEngine;
using UnityEngine.InputSystem;

public enum BallCategory { Small, Medium, Large, Huge }

public class Ball : MonoBehaviour
{
    private Camera mainCamera;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public BallCategory category;
    public bool isBadBall = false;
    private float baseSpeed;
    private float addedTime;
    private Color ballColor;
    private Vector3 ballScale;
    public AudioClip[] ballHitSFX;
    public AudioClip badBallSFX;
    public GameObject badBallIndicator;

    public void InitializeBall(float speedMultiplier, Rect spawnArea, bool badBall = false)
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        float randomX = Random.Range(spawnArea.xMin, spawnArea.xMax);
        float randomY = Random.Range(spawnArea.yMin, spawnArea.yMax);
        transform.position = new Vector2(randomX, randomY);
        isBadBall = badBall;
        if (!isBadBall)
        {
            category = (BallCategory)Random.Range(0, 4);
            switch (category)
            {
                case BallCategory.Small:
                    baseSpeed = 15f;
                    addedTime = 3f;
                    ballColor = Color.magenta;
                    ballScale = new Vector3(0.8f, 0.8f, 1f);
                    break;
                case BallCategory.Medium:
                    baseSpeed = 12f;
                    addedTime = 2f;
                    ballColor = Color.green;
                    ballScale = new Vector3(1f, 1f, 1f);
                    break;
                case BallCategory.Large:
                    baseSpeed = 9f;
                    addedTime = 1.5f;
                    ballColor = Color.blue;
                    ballScale = new Vector3(1.2f, 1.2f, 1f);
                    break;
                case BallCategory.Huge:
                    baseSpeed = 6f;
                    addedTime = 1f;
                    ballColor = Color.yellow;
                    ballScale = new Vector3(1.5f, 1.5f, 1f);
                    break;
            }
            badBallIndicator.SetActive(false);
        }
        else
        {
            baseSpeed = 12f;
            addedTime = -2f;
            ballColor = Color.black;
            ballScale = new Vector3(1f, 1f, 1f);
            badBallIndicator.SetActive(true);
        }
        baseSpeed *= speedMultiplier;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = ballColor;
        }
        transform.localScale = ballScale;
        Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        rb.linearVelocity = direction * baseSpeed;
    }

    void Update()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            HandleTouchOrClick(touchPosition);
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            HandleTouchOrClick(mousePosition);
        }
    }

    private void HandleTouchOrClick(Vector2 screenPosition)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (isBadBall)
            {
                AudioManager.Instance.PlaySFX(badBallSFX);
                TimerManager.instance.AddTime(addedTime);
            }
            else
            {
                AudioManager.Instance.PlaySFX(ballHitSFX[Random.Range(0,ballHitSFX.Length)]);
                TimerManager.instance.AddTime(addedTime);
            }
            BallSpawner.instance.NotifyBallDestroyed(gameObject);
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * baseSpeed;
        }
    }
}
