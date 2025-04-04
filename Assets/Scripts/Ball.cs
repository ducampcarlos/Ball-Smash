using UnityEngine;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    private Camera mainCamera;
    Rigidbody2D rb;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        //Starts in a random position of the screen
        float randomX = Random.Range(-mainCamera.orthographicSize * mainCamera.aspect, mainCamera.orthographicSize * mainCamera.aspect);
        float randomY = Random.Range(-mainCamera.orthographicSize, mainCamera.orthographicSize);
        transform.position = new Vector2(randomX, randomY);

        rb.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 15f, ForceMode2D.Impulse);
    }

    void Update()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            HandleTouchOrClick(touchPosition);
        }
#else
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            HandleTouchOrClick(mousePosition);
        }
#endif
    }

    private void HandleTouchOrClick(Vector2 screenPosition)
    {
        Vector2 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == this.gameObject)
        {
            GameManager.instance.ScoreUp();
            Destroy(gameObject);
        }
    }

}
