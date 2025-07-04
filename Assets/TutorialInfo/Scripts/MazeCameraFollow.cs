using UnityEngine;

public class MazeCameraFollow : MonoBehaviour
{
    public Transform target; // Player reference
    public Vector3 offset = new Vector3(0, 6, -6); // Above + behind
    public float smoothSpeed = 2.5f;
    public float rotationSmoothness = 3f;
    public float tiltAngle = 25f;

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Calculate the desired position (rotated offset)
        Vector3 desiredPosition = target.position + target.rotation * offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // 2. Rotate toward the player only on the Y axis (prevent spinning)
        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion flatRotation = Quaternion.LookRotation(direction);
            Quaternion tiltRotation = Quaternion.Euler(tiltAngle, flatRotation.eulerAngles.y, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, tiltRotation, rotationSmoothness * Time.deltaTime);
        }
    }
}
