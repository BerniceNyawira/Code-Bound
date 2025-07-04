using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private List<string> collectedItems = new List<string>();
    public Text inventoryText; // 👉 drag the InventoryText UI object here

    private bool isInventoryVisible = false;

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
    }

    public void ToggleInventory()
    {
        isInventoryVisible = !isInventoryVisible;

        if (inventoryText != null)
        {
            inventoryText.gameObject.SetActive(isInventoryVisible);

            if (isInventoryVisible)
            {
                inventoryText.text = "Collected Items:\n";
                foreach (string item in collectedItems)
                    inventoryText.text += "- " + item + "\n";
            }
        }
    }
}
