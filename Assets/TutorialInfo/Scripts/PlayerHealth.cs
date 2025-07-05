using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public float respawnDelay = 2f;
    public float damageCooldown = 1.5f; // 🛡️ Time before you can take damage again

    public Slider healthBar;

    private Vector3 spawnPoint;
    private DamageFlash damageFlash;
    private bool isInvincible = false;

    void Start()
    {
        currentHealth = maxHealth;
        spawnPoint = transform.position;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        damageFlash = FindFirstObjectByType<DamageFlash>();
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || currentHealth <= 0)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
            healthBar.value = currentHealth;

        if (damageFlash != null)
            damageFlash.Flash();

        if (currentHealth > 0)
        {
            Invoke(nameof(Respawn), respawnDelay);
            StartCoroutine(DamageCooldown());
        }
        else
        {
            GameOver();
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

    System.Collections.IEnumerator DamageCooldown()
    {
        isInvincible = true;
        yield return new WaitForSeconds(damageCooldown);
        isInvincible = false;
    }
}
