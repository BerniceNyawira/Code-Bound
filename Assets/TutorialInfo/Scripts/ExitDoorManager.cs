using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPortalTrigger : MonoBehaviour
{
    public string nextSceneName = "LevelComplete";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered exit portal!");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
