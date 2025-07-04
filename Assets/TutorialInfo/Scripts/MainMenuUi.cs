using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("LevelOne");
    }

    public void OpenRules()
    {
        SceneManager.LoadScene("GameRules");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game pressed.");
    }
}
