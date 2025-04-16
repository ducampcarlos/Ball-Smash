using UnityEngine;
using System.Collections.Generic;

public class BallSpawner : MonoBehaviour
{
    public static BallSpawner instance;
    public GameObject ballPrefab;
    public int ballsPerSet = 5;
    public float speedMultiplier = 1f;
    public BoxCollider2D leftBoundary;
    public BoxCollider2D rightBoundary;
    public BoxCollider2D topBoundary;
    public BoxCollider2D bottomBoundary;
    public AudioClip clearSetSFX;
    public float badBallChance = 0.3f;
    private List<GameObject> currentBalls = new List<GameObject>();

    void Awake()
    {
        if (instance == null) { instance = this; } else { Destroy(gameObject); }
    }

    void Start()
    {
        Invoke("SpawnBallSet", 0.5f);
    }

    Rect GetSpawnArea()
    {
        float xMin = leftBoundary.bounds.max.x;
        float xMax = rightBoundary.bounds.min.x;
        float yMin = bottomBoundary.bounds.max.y;
        float yMax = topBoundary.bounds.min.y;
        return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
    }

    public void SpawnBallSet()
    {
        currentBalls.Clear();
        Rect spawnArea = GetSpawnArea();
        for (int i = 0; i < ballsPerSet; i++)
        {
            GameObject ball = Instantiate(ballPrefab);
            Ball ballScript = ball.GetComponent<Ball>();
            ballScript.InitializeBall(speedMultiplier, spawnArea, false);
            currentBalls.Add(ball);
        }
        if (Random.value < badBallChance)
        {
            GameObject badBall = Instantiate(ballPrefab);
            Ball badBallScript = badBall.GetComponent<Ball>();
            badBallScript.InitializeBall(speedMultiplier, spawnArea, true);
            currentBalls.Add(badBall);
        }
    }

    public void NotifyBallDestroyed(GameObject ball)
    {
        if (currentBalls.Contains(ball))
            currentBalls.Remove(ball);
        bool normalExists = false;
        GameObject badBallObj = null;
        foreach (GameObject b in currentBalls)
        {
            Ball ballScript = b.GetComponent<Ball>();
            if (ballScript.isBadBall)
                badBallObj = b;
            else
                normalExists = true;
        }
        if (!normalExists && badBallObj != null)
        {
            currentBalls.Remove(badBallObj);
            Destroy(badBallObj);
        }
        if (currentBalls.Count == 0)
        {
            AudioManager.Instance.PlaySFX(clearSetSFX);
            speedMultiplier += 0.2f;
            SpawnBallSet();
        }
    }

    public void DestroyAllBalls()
    {
        foreach (GameObject ball in currentBalls)
        {
            Destroy(ball);
        }
    }
}
