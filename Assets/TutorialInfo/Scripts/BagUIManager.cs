using UnityEngine;
using UnityEngine.UI;

public class BagUIManager : MonoBehaviour
{
    public GameObject inventoryDisplay;
    public Button bagButton;
    private bool isOpen = false;

    void Start()
    {
        if (inventoryDisplay != null)
            inventoryDisplay.SetActive(false); // Hide at start

        if (bagButton != null)
            bagButton.onClick.AddListener(ToggleInventory);
    }

    void ToggleInventory()
    {
        isOpen = !isOpen;
        if (inventoryDisplay != null)
            inventoryDisplay.SetActive(isOpen);
    }
}
