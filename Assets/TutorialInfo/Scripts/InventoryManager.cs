using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private List<string> collectedItems = new List<string>();
    public Text inventoryText; // 🎯 Drag your UI Text here
    public int totalPartsRequired = 2; // Set this in Inspector

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

        if (isInventoryVisible && inventoryText != null)
        {
            UpdateInventoryText();
        }

        CheckIfAllCollected();
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

    private void CheckIfAllCollected()
    {
        if (collectedItems.Count >= totalPartsRequired)
        {
            Debug.Log("✅ All parts collected! Opening door...");
            if (ExitDoorManager.Instance != null)
            {
                ExitDoorManager.Instance.ActivateExit();
            }
        }
    }

    public List<string> GetCollectedItems()
    {
        return collectedItems;
    }
}
