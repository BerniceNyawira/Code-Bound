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
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    Vector3 playerPos = player.transform.position;
    Vector3 playerForward = player.transform.forward;

    // 📍 Spawn in front + a little above
    Vector3 spawnPos = playerPos + playerForward * 6f + Vector3.up * 3f;

    // 🎯 Make portal face player + stand upright
    Vector3 directionToPlayer = playerPos - spawnPos;
    Quaternion facePlayer = Quaternion.LookRotation(directionToPlayer, Vector3.up);
    Quaternion spawnRotation = Quaternion.Euler(90f, 90f, 90f);

    Instantiate(exitPrefab, spawnPos, spawnRotation);
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
