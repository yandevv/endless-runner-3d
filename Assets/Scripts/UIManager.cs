using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    void Awake() => Instance = this;

    public void UpdateScore(int score)
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
    }

    public void UpdateHighScore(int highScore)
    {
        if (highScoreText != null) highScoreText.text = "Best: " + highScore;
    }
}