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
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 playerForward = GameObject.FindGameObjectWithTag("Player").transform.forward;

        // 🧭 Spawn it 6 units *in front* and 2 units *up*
        Vector3 spawnPos = playerPos + playerForward * 6f + Vector3.up * 2f;

        // 🎯 Make it face the player
        Vector3 directionToPlayer = playerPos - spawnPos;
        Quaternion facePlayerRotation = Quaternion.LookRotation(directionToPlayer);

        // 🔄 Rotate upright
        Quaternion uprightFix = Quaternion.Euler(-90f, 0f, 0f);
        Quaternion spawnRotation = facePlayerRotation * uprightFix;

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
