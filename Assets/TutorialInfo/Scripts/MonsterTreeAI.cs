using UnityEngine;
using UnityEngine.AI;

public class MonsterTreeAI : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 20f;
    public float attackRange = 5f;
    public float speed = 3.5f;

    private NavMeshAgent agent;
    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= chaseRange)
        {
            isChasing = true;
        }

        if (isChasing)
        {
            agent.SetDestination(player.position);

            if (distance <= attackRange)
            {
                Debug.Log("Monster attacks the player!");
                // Later: reduce player health or trigger a jumpscare
            }
        }
    }
    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(1);
        }
    }
}

}

