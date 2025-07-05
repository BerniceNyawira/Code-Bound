using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
    public string nextSceneName = "LevelCompleted";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("✅ Player entered portal — loading next scene...");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
