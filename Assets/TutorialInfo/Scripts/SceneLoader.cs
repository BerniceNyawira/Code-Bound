using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Loads the Rules screen
    public void LoadRules()
    {
        SceneManager.LoadScene("Rules"); // Change this to the actual name of your Rules scene
    }

    // Loads Level 1 (your game start)
    public void LoadLevelOne()
    {
        SceneManager.LoadScene("level 1"); // Make sure this matches the name in Build Settings
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Change this to your actual Main Menu scene name
    }
    // Quits the game (won't work in Editor, only in build)
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!"); // So you see something when testing in Editor
    }
}
