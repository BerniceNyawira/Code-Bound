using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private List<string> collectedItems = new List<string>();
    public Text inventoryText; // Optional UI text
    public int totalPartsRequired = 2;
    private bool isInventoryVisible = false;
    public TextMeshProUGUI clueCounterText; // ← Drag your TMP or UI text here


    public delegate void AllPartsCollectedEvent();
    public static event AllPartsCollectedEvent OnAllPartsCollected;

    void Start()
    {
        UpdateClueCounter();
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public List<string> GetCollectedItems()
    {
        return new List<string>(collectedItems); // Return a copy
    }

    private void UpdateClueCounter()
{
    if (clueCounterText != null)
        clueCounterText.text = $"Parts Collected: {collectedItems.Count} / {totalPartsRequired}";
}


    public void AddItem(string itemName)
    {
        if (!collectedItems.Contains(itemName))
        {
            collectedItems.Add(itemName);
            Debug.Log("Collected: " + itemName);

            UpdateClueCounter();

            if (isInventoryVisible && inventoryText != null)
                UpdateInventoryText();

            CheckIfAllCollected();
        }
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
            OnAllPartsCollected?.Invoke();
            ShowLevelCompletedUI();
        }
    }

    private void ShowLevelCompletedUI()
{
    LevelCompletedUI levelCompletedUI = FindAnyObjectByType<LevelCompletedUI>(FindObjectsInactive.Include);
    if (levelCompletedUI != null)
    {
        levelCompletedUI.gameObject.SetActive(true);
    }
    else
    {
        Debug.LogWarning("⚠ LevelCompletedUI not found in scene!");
    }
}

}

// Note: Ensure that the LevelCompletedUI script is set up to handle the event from InventoryManager.