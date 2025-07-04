using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // ← Add this

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public float respawnDelay = 2f;

    public Slider healthBar; // ← Add this
    private Vector3 spawnPoint;
    private DamageFlash damageFlash;

    void Start()
    {
        currentHealth = maxHealth;
        spawnPoint = transform.position;

        healthBar.value = currentHealth; // ← Initialize bar

        damageFlash = FindFirstObjectByType<DamageFlash>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth > 0)
        {
            Invoke(nameof(Respawn), respawnDelay);
        }
        else
        {
            GameOver();
        }

        healthBar.value = currentHealth; // ← Update bar

        if (damageFlash != null)
        {
            damageFlash.Flash();
        }
    }

    void Respawn()
    {
        transform.position = spawnPoint;
    }

    void GameOver()
    {
        Debug.Log("Player Died");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
