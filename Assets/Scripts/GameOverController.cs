using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    void Start()
    {
        if (scoreText != null) scoreText.text = "Score: " + ScoreManager.CurrentScore;
        if (highScoreText != null) highScoreText.text = "Best: " + ScoreManager.HighScore;
    }

    public void Retry() => SceneManager.LoadScene("Game");
    public void BackToMenu() => SceneManager.LoadScene("Menu");
}