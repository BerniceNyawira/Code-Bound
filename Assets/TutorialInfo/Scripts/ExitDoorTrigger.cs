using UnityEngine;

public class ExitSpawner : MonoBehaviour
{
    public GameObject exitPortalPrefab; // 🎯 Assign your portal/door prefab here
    public float spawnOffset = 5f; // Distance in front of player to spawn
    private bool portalSpawned = false;

    void OnEnable()
    {
        InventoryManager.OnAllPartsCollected += SpawnPortal;
    }

    void OnDisable()
    {
        InventoryManager.OnAllPartsCollected -= SpawnPortal;
    }

    void SpawnPortal()
    {
        if (portalSpawned) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Player not found!");
            return;
        }

        // Calculate position in front of player
        Vector3 spawnPos = player.transform.position + player.transform.forward * spawnOffset;
        spawnPos.y = player.transform.position.y; // keep ground level

        Instantiate(exitPortalPrefab, spawnPos, Quaternion.identity);
        Debug.Log("🚪 Exit portal spawned!");

        portalSpawned = true;
    }
}
