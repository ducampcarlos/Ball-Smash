using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TextMeshProUGUI gameOverText;
    public GameObject restartButton;
    public AudioClip gameOverSFX;
    public GameObject bar;

    void Awake()
    {
        if (instance == null) { instance = this; } else { Destroy(gameObject); }
    }

    void Start()
    {
        gameOverText.gameObject.SetActive(false);
        restartButton.SetActive(false);
        AudioManager.Instance.RestoreMusicVolume();
    }

    public void GameOver(float finalTime)
    {
        gameOverText.text = "Game Over! You survived " + finalTime.ToString("F1") + " seconds.";
        gameOverText.gameObject.SetActive(true);
        restartButton.SetActive(true);
        BallSpawner.instance.DestroyAllBalls();
        AudioManager.Instance.StartCoroutine(AudioManager.Instance.FadeOutMusic(1));
        AudioManager.Instance.PlaySFX(gameOverSFX);
        bar.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
