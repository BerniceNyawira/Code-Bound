using UnityEngine;

public class RobotChase : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2.5f;
    public float chaseDistance = 20f;
    public bool isChasing = false;
    private Animator anim;
    private Rigidbody rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        anim.SetBool("isRunning", isChasing);

        if (!isChasing || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= chaseDistance)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

            // Use Rigidbody movement
            rb.MovePosition(rb.position + dir * moveSpeed * Time.deltaTime);
        }
    }

    public void StartChasing() => isChasing = true;
    public void StopChasing() => isChasing = false;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FloatingHealth fh = collision.gameObject.GetComponent<FloatingHealth>();
            if (fh != null)
            {
                anim.SetBool("isAttacking", true);
                fh.TakeDamage();
                Invoke(nameof(ResetAttack), 1f);
            }
        }
    }

    void ResetAttack()
    {
        anim.SetBool("isAttacking", false);
    }
}
