using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Load the Rules screen
    public void LoadRules()
    {
        SceneManager.LoadScene("Rules");
    }

    // General method to load any level and remember it
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
        PlayerPrefs.SetString("LastPlayedLevel", levelName);
    }

    // Retry the last played level
    public void RetryLevel()
    {
        string lastLevel = PlayerPrefs.GetString("LastPlayedLevel", "level 1");
        Debug.Log("Retrying level: " + lastLevel);
        SceneManager.LoadScene(lastLevel);
    }

    // Go back to Main Menu
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
