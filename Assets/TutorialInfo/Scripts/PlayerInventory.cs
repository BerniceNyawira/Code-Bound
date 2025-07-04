using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public Transform inventoryDisplay; // Drag the InventoryDisplay object here
    public List<GameObject> collectedItems = new List<GameObject>();

    public void AddItem(GameObject itemModelPrefab)
    {
        // Instantiate a small version for display
        GameObject visualItem = Instantiate(itemModelPrefab, inventoryDisplay);

        visualItem.transform.localPosition = new Vector3(collectedItems.Count * 0.5f, 0, 0); // arrange side by side
        visualItem.transform.localScale = visualItem.transform.localScale * 0.3f; // shrink
        visualItem.transform.localRotation = Quaternion.identity;

        // Disable collider and scripts so it's just visual
        Collider col = visualItem.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        MonoBehaviour[] scripts = visualItem.GetComponents<MonoBehaviour>();
        foreach (var s in scripts) s.enabled = false;

        collectedItems.Add(visualItem);
    }
}
