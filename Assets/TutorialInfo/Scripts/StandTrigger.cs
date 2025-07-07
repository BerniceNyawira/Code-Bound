using UnityEngine;

[RequireComponent(typeof(Collider))]
public class StandTrigger : MonoBehaviour
{
    [Header("References")]
    public ComponentCombiner combiner;
    public GameObject interactionPrompt; // "Press E to interact" text
    public float interactionRadius = 2f;

    private bool playerInRange;

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            combiner.StartCombination();
            if (interactionPrompt != null) interactionPrompt.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
            playerInRange = true;
            if (interactionPrompt != null) interactionPrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other))
        {
            playerInRange = false;
            if (interactionPrompt != null) interactionPrompt.SetActive(false);
        }
    }

    bool IsPlayer(Collider col)
    {
        return col.CompareTag("Player") || 
               col.GetComponent<CharacterController>() != null;
    }

    // Visualize interaction radius in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}