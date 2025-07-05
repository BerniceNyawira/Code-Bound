using UnityEngine;

public class RobotChase : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2.5f;
    public float chaseDistance = 20f;
    public float attackCooldown = 2f;
    public float attackDelay = 0.5f;
    public float attackAnimationSpeed = 0.5f;

    private bool isChasing = false;
    private bool canAttack = true;
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

        if (!isChasing || player == null || !canAttack) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= chaseDistance)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            rb.MovePosition(rb.position + dir * moveSpeed * Time.deltaTime);
        }
    }

    public void StartChasing() => isChasing = true;
    public void StopChasing() => isChasing = false;

    void OnCollisionEnter(Collision collision)
    {
        if (!canAttack) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            FloatingHealth fh = collision.gameObject.GetComponent<FloatingHealth>();
            DamageFlash flash = FindFirstObjectByType<DamageFlash>(); // Get global DamageFlash

            if (fh != null)
            {
                StartCoroutine(AttackRoutine(fh, flash));
            }
        }
    }

    System.Collections.IEnumerator AttackRoutine(FloatingHealth target, DamageFlash flash)
    {
        canAttack = false;

        anim.SetBool("isRunning", false);
        anim.SetBool("isAttacking", true);
        anim.speed = attackAnimationSpeed;

        yield return new WaitForSeconds(attackDelay);

        // Trigger flash
        if (flash != null)
            flash.Flash();

        target.TakeDamage();

        yield return new WaitForSeconds(attackCooldown);

        anim.SetBool("isAttacking", false);
        anim.speed = 1f;
        canAttack = true;
    }
}
