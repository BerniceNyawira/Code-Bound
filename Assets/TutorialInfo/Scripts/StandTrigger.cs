using UnityEngine;
using TMPro;

public class StandTrigger : MonoBehaviour
{
    public TextMeshProUGUI instructionText;
    public GameObject combineButton;

    private bool hasTriggered = false;

    void Start()
    {
        instructionText.text = "Find the stand to begin combining...";
        combineButton.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            instructionText.text = "Press Combine to start building...";
            combineButton.SetActive(true);
        }
    }
}
