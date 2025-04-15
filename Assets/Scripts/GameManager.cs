using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TextMeshProUGUI gameOverText;
    public GameObject restartButton;

    void Awake()
    {
        if (instance == null) { instance = this; } else { Destroy(gameObject); }
    }

    void Start()
    {
        gameOverText.gameObject.SetActive(false);
        restartButton.SetActive(false);
    }

    public void GameOver(float finalTime)
    {
        gameOverText.text = "Game Over! You survived " + finalTime.ToString("F1") + " seconds.";
        gameOverText.gameObject.SetActive(true);
        restartButton.SetActive(true);
        BallSpawner.instance.DestroyAllBalls();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
