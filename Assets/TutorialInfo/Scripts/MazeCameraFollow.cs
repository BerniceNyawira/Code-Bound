using UnityEngine;

public class MazeCameraFollow : MonoBehaviour
{
    public Transform target; // Player
    public Vector3 offset = new Vector3(0, 6, -6); // Position above & behind
    public float followSpeed = 5f; // Faster catch-up
    public float rotationSpeed = 4f;
    public float tiltAngle = 25f;

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Offset relative to the player's rotation
        Vector3 desiredPosition = target.position + target.rotation * offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // 2. Look in the same direction as the player (with optional tilt)
        Quaternion desiredRotation = Quaternion.Euler(tiltAngle, target.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);

        // if (direction != Vector3.zero)
        // {
        //     Quaternion flatLook = Quaternion.LookRotation(direction);
        //     Quaternion tiltLook = Quaternion.Euler(tiltAngle, flatLook.eulerAngles.y, 0f);

        //     transform.rotation = Quaternion.Slerp(transform.rotation, tiltLook, rotationSmoothness * Time.deltaTime);
        // }
    }
}
