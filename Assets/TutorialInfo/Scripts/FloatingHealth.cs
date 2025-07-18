using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FloatingHealth : MonoBehaviour
{
    public GameObject[] hearts; // Heart models in the scene
    public int currentHealth;
    public GameObject heartPopPrefab;
    private bool isInvincible = false;
    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        currentHealth = hearts.Length;
    }

    
public void TakeDamage()
{
    if (isInvincible || currentHealth <= 0) return;

    isInvincible = true;
    currentHealth--;
    UpdateHearts();
    StartCoroutine(DamageCooldown());

    if (currentHealth <= 0) StartCoroutine(DelayedGameOver());
}

IEnumerator DamageCooldown()
{
    yield return new WaitForSeconds(2f); // 2 second immunity
    isInvincible = false;
}

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            bool shouldBeVisible = i < currentHealth;

            if (!shouldBeVisible && hearts[i] != null && hearts[i].activeSelf)
            {
                Debug.Log("Popping heart " + i);

                Animator anim = hearts[i].GetComponent<Animator>();
                if (anim != null)
                    anim.Play("HeartPop", 0, 0f);

                if (heartPopPrefab != null)
                    Instantiate(heartPopPrefab, hearts[i].transform.position, Quaternion.identity);

                // ⏳ Wait to destroy heart so pop effect shows
                StartCoroutine(DestroyHeartAfterDelay(hearts[i], 1.5f));
            }
        }
    }

    System.Collections.IEnumerator DestroyHeartAfterDelay(GameObject heart, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(heart);
    }

    System.Collections.IEnumerator DelayedGameOver()
    {
        yield return new WaitForSeconds(4.5f); // ⏳ longer delay for full die animation
        SceneManager.LoadScene("GameOver");
    }

    public void KillImmediately()
    {
        currentHealth = 0;
        UpdateHearts();

        if (playerMovement != null)
            playerMovement.PlayDieAnimation();
    }
}
