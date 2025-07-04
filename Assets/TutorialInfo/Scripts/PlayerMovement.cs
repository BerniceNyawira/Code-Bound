using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform cameraTransform; // Drag the Main Camera here in the Inspector

    private Rigidbody rb;
    private Vector3 movement;

    private Animator anim;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // Step 1: Get camera directions
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // Step 2: Remove any Y direction to keep movement flat
        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        // Step 3: Build the camera-relative movement vector
        movement = (camForward * moveZ + camRight * moveX).normalized;

        // Step 4: Rotate the player to face movement direction
        if (movement != Vector3.zero)
            transform.forward = movement;

        // Step 5: Animate
        anim.SetBool("isRunning", movement.magnitude > 0);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void PlayCollectAnimation()
    {
        anim.SetTrigger("collect");
    }

    public void PlayDieAnimation()
    {
        anim.SetTrigger("die");
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
    }
}
