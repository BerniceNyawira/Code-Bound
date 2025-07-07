using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Needed for the Image type

public class ComponentCombiner : MonoBehaviour
{
    public GameObject[] partsToReveal; // Assign 9 parts in order
    public GameObject fullComputer; // Assign the final computer model
    public GameObject retrieveCodeButton; // Assign the button (set inactive by default)
    public GameObject popEffectPrefab; // Assign particle effect prefab
    public AudioSource computerAppearSound; // Sound when full computer appears
    public Image screenFlash; // UI Image used for screen flash
    public GameObject instructionTextObject; // TextMeshPro GameObject
    public GameObject combineButtonObject;   // Combine Button


  public void StartCombination()
{
    // Hide text and button when starting
    if (instructionTextObject != null)
        instructionTextObject.SetActive(false);

    if (combineButtonObject != null)
        combineButtonObject.SetActive(false);

    StartCoroutine(RevealSequence());
}


    IEnumerator RevealSequence()
    {
        // Reveal each part slowly
        foreach (GameObject part in partsToReveal)
        {
            part.SetActive(true);
            yield return new WaitForSeconds(2f);
        }

        yield return new WaitForSeconds(1f);

        // Flash the screen
        StartCoroutine(FlashScreen());

        // Play pop + hide each part
        foreach (GameObject part in partsToReveal)
        {
            if (popEffectPrefab != null)
                Instantiate(popEffectPrefab, part.transform.position, Quaternion.identity);

            part.SetActive(false);
        }

        // Show full computer
        fullComputer.SetActive(true);

        if (computerAppearSound != null)
            computerAppearSound.Play();

        yield return new WaitForSeconds(1f);

        // Show retrieve code button
        retrieveCodeButton.SetActive(true);
    }

    IEnumerator FlashScreen()
    {
        if (screenFlash == null) yield break;

        Color c = screenFlash.color;

        // Flash in
        c.a = 1;
        screenFlash.color = c;
        yield return new WaitForSeconds(0.1f);

        // Fade out
        while (c.a > 0)
        {
            c.a -= Time.deltaTime * 2f;
            screenFlash.color = c;
            yield return null;
        }
    }
}
