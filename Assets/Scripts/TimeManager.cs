using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;
    public float countdown = 30f; 
    public TextMeshProUGUI timerText;
    private float totalSurvivalTime = 0f;
    private bool isGameOver = false;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    void Update()
    {
        if (!isGameOver)
        {
            if (countdown > 0)
            {
                countdown -= Time.deltaTime;
                totalSurvivalTime = 30f + (Time.timeSinceLevelLoad - 30f);
                timerText.text = Mathf.Max(0, Mathf.Round(countdown)) + "s";
            }
            else
            {

                isGameOver = true;
                GameOver();
            }
        }
    }

    public void AddTime(float timeToAdd)
    {
        countdown += timeToAdd;
    }

    void GameOver()
    {
        GameManager.instance.GameOver(totalSurvivalTime);
    }
}
