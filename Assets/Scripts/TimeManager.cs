using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;
    public float countdown = 30f; 
    public TextMeshProUGUI timerText;
    private float totalSurvivalTime = 0f;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    void Update()
    {
        if (countdown > 0)
        {
            countdown -= Time.deltaTime;
            totalSurvivalTime = 30f + (Time.timeSinceLevelLoad - 30f);
            timerText.text = "Time: " + Mathf.Max(0, Mathf.Round(countdown)) + "s";
        }
        else
        {
            GameOver();
        }
    }

    public void AddTime(float timeToAdd)
    {
        countdown += timeToAdd;
    }

    void GameOver()
    {
        // Aquí se muestra el tiempo total sobrevivido.
        Debug.Log("Game Over! Sobreviviste: " + (30f + (Time.timeSinceLevelLoad - 30f)) + " segundos");
        // Se pueden activar pantallas finales y otros efectos.
    }
}
