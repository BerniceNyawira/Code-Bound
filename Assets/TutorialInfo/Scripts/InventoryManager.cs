using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private List<string> collectedItems = new List<string>();
    public Text inventoryText; // 🎯 Optional UI Text
    public int totalPartsRequired = 2;

    private bool isInventoryVisible = false;

    public delegate void AllPartsCollectedEvent();
    public static event AllPartsCollectedEvent OnAllPartsCollected;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }


    public void AddItem(string itemName)
    {
        collectedItems.Add(itemName);
        Debug.Log("Collected: " + itemName);

        if (isInventoryVisible && inventoryText != null)
            UpdateInventoryText();

        CheckIfAllCollected();
    }

    public void ToggleInventory()
    {
        isInventoryVisible = !isInventoryVisible;

        if (inventoryText != null)
        {
            inventoryText.gameObject.SetActive(isInventoryVisible);
            if (isInventoryVisible)
                UpdateInventoryText();
        }
    }

    private void UpdateInventoryText()
    {
        if (inventoryText == null) return;

        inventoryText.text = "Collected Items:\n";
        foreach (string item in collectedItems)
        {
            inventoryText.text += "- " + item + "\n";
        }
    }

    private void CheckIfAllCollected()
    {
        if (collectedItems.Count >= totalPartsRequired)
        {
            Debug.Log("✅ All parts collected!");
            OnAllPartsCollected?.Invoke(); // Broadcast event
        }
    }

    public List<string> GetCollectedItems()
    {
        return collectedItems;
    }
}
