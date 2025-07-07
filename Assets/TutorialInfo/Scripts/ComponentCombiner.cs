using System.Collections; // ADD THIS LINE TO FIX THE ERROR
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComponentCombiner : MonoBehaviour
{
    [Header("Part Animation")]
    public GameObject[] partsToReveal;
    public GameObject fullComputer;
    public GameObject popEffectPrefab;
    public AudioSource computerAppearSound;
    public Image screenFlash;

    [Header("UI Elements")]
    public GameObject instructionTextObject;
    public GameObject combineButtonObject;
    public GameObject retrieveBUButton;
    public TMP_Text completionText;

    [Header("Timing")]
    public float partRevealDelay = 2f;
    public float preCombineDelay = 1f;
    public float postCombineDelay = 1f;

    private void Start()
    {
        retrieveBUButton.SetActive(false);
        fullComputer.SetActive(false);
        
        foreach (GameObject part in partsToReveal)
        {
            part.SetActive(false);
        }
    }

    public void StartCombination()
    {
        if (instructionTextObject != null)
            instructionTextObject.SetActive(false);

        if (combineButtonObject != null)
            combineButtonObject.SetActive(false);

        StartCoroutine(RevealSequence());
    }

    IEnumerator RevealSequence()
    {
        // Phase 1: Reveal parts
        foreach (GameObject part in partsToReveal)
        {
            part.SetActive(true);
            yield return new WaitForSeconds(partRevealDelay);
        }

        yield return new WaitForSeconds(preCombineDelay);

        // Phase 2: Combine animation
        StartCoroutine(FlashScreen());

        foreach (GameObject part in partsToReveal)
        {
            if (popEffectPrefab != null)
                Instantiate(popEffectPrefab, part.transform.position, Quaternion.identity);
            
            part.SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }

        // Phase 3: Final reveal
        fullComputer.SetActive(true);
        
        if (computerAppearSound != null)
            computerAppearSound.Play();

        if (completionText != null)
        {
            completionText.text = "COMPUTER ASSEMBLED!";
            completionText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(postCombineDelay);
        retrieveBUButton.SetActive(true);
    }

    IEnumerator FlashScreen()
    {
        if (screenFlash == null) yield break;

        Color c = screenFlash.color;
        c.a = 1;
        screenFlash.color = c;
        
        yield return new WaitForSeconds(0.1f);
        
        while (c.a > 0)
        {
            c.a -= Time.deltaTime * 2f;
            screenFlash.color = c;
            yield return null;
        }
    }
}