using UnityEngine;
using UnityEngine.SceneManagement;

public class FloatingHealth : MonoBehaviour
{
    public GameObject[] hearts; // Heart models in the scene
    public int currentHealth;
    public GameObject heartPopPrefab;

    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        currentHealth = hearts.Length;
    }

    public void TakeDamage()
    {
        Debug.Log("Player took damage");

        currentHealth--;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHearts();

        if (currentHealth == 0)
        {
            if (playerMovement != null)
            {
                playerMovement.PlayDieAnimation();
            }

            Debug.Log("No hearts left — Game Over!");
            StartCoroutine(DelayedGameOver()); // ✅ delay scene change
        }
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
