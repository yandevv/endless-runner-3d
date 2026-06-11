using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int CurrentScore { get; private set; }
    public static int HighScore => PlayerPrefs.GetInt("HighScore", 0);

    private static float scoreAccumulator;

    void Start()
    {
        // Reset the statics on every (re)load — kills the cross-run leak
        CurrentScore = 0;
        scoreAccumulator = 0f;

        if (UIManager.Instance != null)
            UIManager.Instance.UpdateHighScore(HighScore);
    }

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying) return;

        // Accumulate as float (= distance traveled), floor for the int display
        scoreAccumulator += SpeedScaler.CurrentSpeed * Time.deltaTime;
        CurrentScore = Mathf.FloorToInt(scoreAccumulator);

        if (UIManager.Instance != null)
            UIManager.Instance.UpdateScore(CurrentScore);
    }

    public static void SaveHighScore()
    {
        if (CurrentScore > HighScore)
        {
            PlayerPrefs.SetInt("HighScore", CurrentScore);
            PlayerPrefs.Save();   // force write to disk now
        }
    }
}