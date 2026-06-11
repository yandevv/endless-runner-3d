using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PlayGame() => SceneManager.LoadScene("Game");

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit pressed (only actually quits in a built game, not in the editor)");
    }
}