using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class EpilogueManager : MonoBehaviour
{
    [Header("Text Settings")]
    public TMP_Text epilogueText;
    public string message = "You have escaped.... for now";
    public float typingSpeed = 0.1f;
    public float eyesAppearDelay = 1.5f;
    public float eyesDisplayDuration = 3f; // New: How long eyes stay visible
    public float sceneFadeDuration = 2f; // New: Duration for scene fade out

    [Header("Red Eyes")]
    public GameObject redEyes;
    public float eyesFadeDuration = 2f;
    public AudioSource eyesAppearSound;

    [Header("Scene Transition")]
    public string mainMenuScene = "MainMenu"; // Name of your main menu scene
    public CanvasGroup sceneFader; // Assign a full-screen UI panel for fading

    void Start()
    {
        // Start hidden
        epilogueText.text = "";
        redEyes.SetActive(false);
        
        // Initialize scene fader if exists
        if (sceneFader != null)
        {
            sceneFader.alpha = 0;
            sceneFader.blocksRaycasts = false;
        }
        
        // Begin sequence
        StartCoroutine(RunEpilogue());
    }

    IEnumerator RunEpilogue()
    {
        // Type out text
        yield return StartCoroutine(TypeText(message));
        
        // Pause before eyes appear
        yield return new WaitForSeconds(eyesAppearDelay);
        
        // Reveal red eyes
        redEyes.SetActive(true);
        if (eyesAppearSound != null) eyesAppearSound.Play();
        
        // Fade in eyes
        yield return StartCoroutine(FadeInEyes());
        
        // Let eyes sit for a few seconds
        yield return new WaitForSeconds(eyesDisplayDuration);
        
        // Fade out entire scene
        yield return StartCoroutine(FadeOutScene());
        
        // Load main menu
        SceneManager.LoadScene(mainMenuScene);
    }

    IEnumerator TypeText(string fullText)
    {
        for (int i = 0; i < fullText.Length; i++)
        {
            epilogueText.text = fullText.Substring(0, i+1);
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    IEnumerator FadeInEyes()
    {
        CanvasGroup eyesCG = redEyes.GetComponent<CanvasGroup>();
        if (eyesCG == null) eyesCG = redEyes.AddComponent<CanvasGroup>();
        
        float elapsed = 0f;
        while (elapsed < eyesFadeDuration)
        {
            eyesCG.alpha = Mathf.Lerp(0, 1, elapsed/eyesFadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        eyesCG.alpha = 1;
    }

    // New: Fade out the entire scene
    IEnumerator FadeOutScene()
    {
        if (sceneFader == null) yield break;
        
        sceneFader.blocksRaycasts = true;
        float elapsed = 0f;
        
        while (elapsed < sceneFadeDuration)
        {
            sceneFader.alpha = Mathf.Lerp(0, 1, elapsed/sceneFadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        sceneFader.alpha = 1;
    }
}