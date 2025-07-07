using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        Vector3 newPos = player.position;
        newPos.y = transform.position.y; // Keep height constant
        transform.position = newPos;
    }
}
