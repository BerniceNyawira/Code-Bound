using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoorTrigger : MonoBehaviour
{
    public int totalPartsRequired = 2; // Set this in Inspector or from GameManager
    public string nextSceneName = "LevelComplete"; // Name of next scene

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int collected = InventoryManager.Instance.GetCollectedItems().Count;

            if (collected >= totalPartsRequired)
            {
                Debug.Log("All parts collected! Proceeding...");
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.Log("Not enough parts yet!");
            }
        }
    }
}
