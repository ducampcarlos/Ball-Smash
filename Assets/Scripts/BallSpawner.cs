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
    private List<GameObject> currentBalls = new List<GameObject>();

    void Awake()
    {
        if (instance == null) { instance = this; } else { Destroy(gameObject); }
    }

    void Start()
    {
        SpawnBallSet();
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
            ballScript.InitializeBall(speedMultiplier, spawnArea);
            currentBalls.Add(ball);
        }
    }

    public void NotifyBallDestroyed(GameObject ball)
    {
        currentBalls.Remove(ball);
        if (currentBalls.Count == 0)
        {
            speedMultiplier += 0.2f;
            SpawnBallSet();
        }
    }
}
