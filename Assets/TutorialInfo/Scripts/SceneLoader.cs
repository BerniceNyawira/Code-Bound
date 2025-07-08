using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneLoader : MonoBehaviour
{
    void Awake()
    {
        // Singleton pattern
        int numSceneLoaders = FindObjectsByType<SceneLoader>(FindObjectsSortMode.None).Length;
        if (numSceneLoaders > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Load specific levels
    public void LoadLevel1() => LoadLevel("1");
    public void LoadLevel2() => LoadLevel("2"); 
    public void LoadLevel3() => LoadLevel("3");
    public void LoadLevel4() => LoadLevel("4");

    // ✅ Load CodeBase scene on button click
    public void LoadCodeBase() => LoadLevel("CodeBase");

    // Base level loading method
    public void LoadLevel(string levelName)
    {
        if (Application.CanStreamedLevelBeLoaded(levelName))
        {
            SceneManager.LoadScene(levelName);
            PlayerPrefs.SetString("LastPlayedLevel", levelName);
            Debug.Log("Loading: " + levelName);
        }
        else
        {
            Debug.LogError($"Scene '{levelName}' not found in build settings!");
            LoadMainMenu();
        }
    }

    // Retry the last played level
    public void RetryLevel()
    {
        string lastLevel = PlayerPrefs.GetString("LastPlayedLevel", "Level 1");
        Debug.Log("Retrying: " + lastLevel);
        LoadLevel(lastLevel);
    }

    // Navigation
    public void LoadRules() => SceneManager.LoadScene("Rules");
    public void LoadMainMenu() => SceneManager.LoadScene("MainMenu");
    public void QuitGame() => Application.Quit();
}
