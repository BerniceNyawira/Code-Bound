using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LevelCompletedUI : MonoBehaviour
{
    [Header("Level-Specific UI Elements")]
    public Image[] levelItemIcons; // Pre-placed icons for THIS level only
    public Sprite[] collectedSprites; // Sprites for collected state
    public Sprite missingSprite; // Sprite for missing items

    [Header("Text Elements")]
    public TMP_Text collectedText;

    [Header("Navigation")]
    public Button nextButton;

    [Header("Animation Settings")]
    public float typingSpeed = 0.05f;
    public float postTextDelay = 0.5f;

    [Header("Colors")]
    public Color collectedColor = Color.white;
    public Color missingColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip typingSound;
    public AudioClip itemAppearSound;

    [Header("Effects")]
    public ParticleSystem itemAppearEffect;

    private void OnEnable()
    {
        // Ensure audio source is ready
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.enabled = true;
        audioSource.playOnAwake = false;

        if (!audioSource.gameObject.activeInHierarchy)
            audioSource.gameObject.SetActive(true);

        StartCoroutine(ShowCollectedItems());
    }

    private IEnumerator ShowCollectedItems()
    {
        // Get items collected in this level
        int collectedThisLevel = InventoryManager.Instance.GetCollectedItems().Count;
        int totalForThisLevel = levelItemIcons.Length;

        // Initialize all icons as missing
        foreach (Image icon in levelItemIcons)
        {
            icon.sprite = missingSprite;
            icon.color = missingColor;
        }

        // Display message
        yield return StartCoroutine(TypeText(
            $"You collected: {collectedThisLevel}/{totalForThisLevel} computer parts!"
        ));

        // Show collected items with effects
        for (int i = 0; i < collectedThisLevel; i++)
        {
            if (i < levelItemIcons.Length)
            {
                levelItemIcons[i].sprite = collectedSprites[i];
                levelItemIcons[i].color = collectedColor;

                PlayItemAppearEffects(levelItemIcons[i].transform.position);
                yield return new WaitForSeconds(0.2f);
            }
        }

        nextButton.interactable = true;
    }

    private IEnumerator TypeText(string text)
    {
        collectedText.text = "";

        foreach (char c in text)
        {
            collectedText.text += c;

            if (audioSource != null && audioSource.enabled && audioSource.gameObject.activeInHierarchy && typingSound != null)
                audioSource.PlayOneShot(typingSound);

            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(postTextDelay);
    }

    private void PlayItemAppearEffects(Vector3 position)
    {
        if (audioSource != null && audioSource.enabled && audioSource.gameObject.activeInHierarchy && itemAppearSound != null)
            audioSource.PlayOneShot(itemAppearSound);

        if (itemAppearEffect != null)
        {
            itemAppearEffect.transform.position = position;
            itemAppearEffect.Play();
        }
    }
}
