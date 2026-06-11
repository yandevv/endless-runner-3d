using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool IsPlaying { get; private set; }
    public float gameOverDelay = 1.5f;

    void Awake()
    {
        Instance = this;
        IsPlaying = true;
    }

    public void GameOver()
    {
        if (!IsPlaying) return;
        IsPlaying = false;
        ScoreManager.SaveHighScore();
        Debug.Log("Game Over — high score saved");
        Invoke(nameof(LoadGameOver), gameOverDelay);
    }

    void LoadGameOver() => SceneManager.LoadScene("GameOver");
}