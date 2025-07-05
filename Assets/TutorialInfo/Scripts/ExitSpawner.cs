using UnityEngine;

public class ExitPortalSpawner : MonoBehaviour
{
    public GameObject exitPrefab; // Drag your door/portal prefab
    public int totalPartsRequired = 2; // Adjust per level

    private bool spawned = false;

    void Update()
    {
        if (!spawned && InventoryManager.Instance.GetCollectedItems().Count >= totalPartsRequired)
        {
            Vector3 spawnPos = FindSpawnPosition();
            Instantiate(exitPrefab, spawnPos, Quaternion.identity);
            spawned = true;
        }
    }

    Vector3 FindSpawnPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 forward = player.transform.forward;
            return player.transform.position + forward * 3f; // 3 units ahead
        }
        return Vector3.zero;
    }
}
