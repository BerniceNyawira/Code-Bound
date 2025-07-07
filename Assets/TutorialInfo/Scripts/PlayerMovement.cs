using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotationSpeed = 10f;

    private Rigidbody rb;
    private Animator anim;
    private Vector3 movement;
    private Vector3 lastDirection = Vector3.forward; // default face forward (Z+)

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        movement = new Vector3(moveX, 0f, moveZ).normalized;

        // Update lastDirection if moving
        if (movement != Vector3.zero)
        {
            lastDirection = movement;
        }

        // Always rotate toward the last direction
        Quaternion toRotation = Quaternion.LookRotation(lastDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

        // Animation
        anim.SetBool("isRunning", movement.magnitude > 0.1f);
        Debug.Log("TimeScale: " + Time.timeScale);
    }

    void FixedUpdate()
    {
        if (movement != Vector3.zero)
        {
            Vector3 newPos = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);
        }
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
