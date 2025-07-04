using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public GameObject modelToDisplayInInventory; // Assign the same model used

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null && modelToDisplayInInventory != null)
            {
                inventory.AddItem(modelToDisplayInInventory);
                Destroy(gameObject); // Remove the pickup from the scene
            }
        }
    }
}
