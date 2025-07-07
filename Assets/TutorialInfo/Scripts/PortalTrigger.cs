using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
    [Tooltip("Set to TRUE if using separate scenes per level (Level1Completed, Level2Completed, etc.)")]
    public bool usePerLevelCompletion = true;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (usePerLevelCompletion)
        {
            string currentLevel = SceneManager.GetActiveScene().name;
            string completionScene = currentLevel + "Completed"; // No space

            if (Application.CanStreamedLevelBeLoaded(completionScene))
            {
                Debug.Log($"✅ Loading level completion scene: {completionScene}");
                SceneManager.LoadScene(completionScene);
            }
            else
            {
                Debug.LogError($"❌ Scene '{completionScene}' not found in Build Settings!");
            }
        }
        else
        {
            // Fallback generic scene (optional)
            SceneManager.LoadScene("LevelCompleted");
        }
    }
}
