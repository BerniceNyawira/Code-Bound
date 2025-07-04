using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private List<string> collectedItems = new List<string>();
    public Text inventoryText; // 🎯 Drag your UI Text here

    private bool isInventoryVisible = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(string itemName)
    {
        collectedItems.Add(itemName);
        Debug.Log("Collected: " + itemName);

        // Optional: auto-update inventory UI if it's already open
        if (isInventoryVisible && inventoryText != null)
        {
            UpdateInventoryText();
        }
    }

    public void ToggleInventory()
    {
        isInventoryVisible = !isInventoryVisible;

        if (inventoryText != null)
        {
            inventoryText.gameObject.SetActive(isInventoryVisible);

            if (isInventoryVisible)
            {
                UpdateInventoryText();
            }
        }
    }

    private void UpdateInventoryText()
    {
        inventoryText.text = "Collected Items:\n";
        foreach (string item in collectedItems)
        {
            inventoryText.text += "- " + item + "\n";
        }
    }

    public List<string> GetCollectedItems()
    {
        return collectedItems;
    }
}
