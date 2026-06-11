// GameManager.cs
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool IsPlaying { get; private set; }

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
        // Scene transition gets added in step 4
    }
}