using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdvancedTerminalTypewriter : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] private float charDelay = 0.05f;
    [SerializeField] private float lineDelay = 0.1f;
    [SerializeField] private TMP_Text terminalText;
    [SerializeField] private float glitchLineProbability = 0.15f;
    
    [Header("Cursor Settings")]
    [SerializeField] private RectTransform cursor;
    [SerializeField] private float cursorBlinkSpeed = 0.5f;
    [SerializeField] private Vector2 cursorOffset = new Vector2(10, 0);
    
    [Header("Error System")]
    [SerializeField] private float errorProbability = 0.02f;
    [SerializeField] private List<ErrorProfile> errorProfiles;
    [SerializeField] private Transform screenEffectsParent;
    
    [Header("Audio")]
    [SerializeField] private AudioClip typingSound;
    [SerializeField] private AudioClip lineCompleteSound;
    [SerializeField] private AudioSource audioSource;
    
    [Header("Button Settings")]
    [SerializeField] private Button runButton;
    [SerializeField] private float buttonFadeInTime = 1f;

    private string[] originalCodeLines = new string[]
    {
        "> ACCESSING CORE MODE...",
        "",
        "> validating.components(): OK",
        "> compiling_escapeProtocol_ur/",
        "",
        "> route://digimaze/exit/sequence/init",
        "> unlock_exitGate()",
        "",
        "> transmit_code(\"DIGIMAZ3-UNLOCK\")",
        "",
        "> STATUS: ESCAPE ROUTE ENABLED",
        "> player.transfer -> OUTSIDE_NET"
    };

    private List<string> modifiedCodeLines;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private int currentErrorProfileIndex = 0;

    [System.Serializable]
    public class ErrorProfile
    {
        public string name;
        public Color glitchColor = Color.red;
        public string glitchChars = "�#%&@$*";
        public AudioClip errorSound;
        public float errorDuration = 0.3f;
        public GameObject visualEffectPrefab;
        public float screenShakeIntensity = 0f;
    }

    void Start()
    {
        modifiedCodeLines = new List<string>(originalCodeLines);
        terminalText.text = "";
        runButton.gameObject.SetActive(false);
        runButton.onClick.AddListener(OnRunButtonClicked);
        
        if (cursor != null)
        {
            cursor.gameObject.SetActive(false);
            StartCoroutine(BlinkCursor());
        }
        
        // Randomly corrupt some lines
        CorruptRandomLines();
        
        typingCoroutine = StartCoroutine(TypeCode());
    }

    void CorruptRandomLines()
    {
        for (int i = 0; i < modifiedCodeLines.Count; i++)
        {
            if (!string.IsNullOrEmpty(modifiedCodeLines[i]) && Random.value < glitchLineProbability)
            {
                modifiedCodeLines[i] = ApplyErrorProfile(modifiedCodeLines[i], 
                    errorProfiles[Random.Range(0, errorProfiles.Count)]);
            }
        }
    }

    string ApplyErrorProfile(string original, ErrorProfile profile)
    {
        int errorsToAdd = Random.Range(1, original.Length / 4);
        char[] corrupted = original.ToCharArray();
        
        for (int i = 0; i < errorsToAdd; i++)
        {
            int pos = Random.Range(0, corrupted.Length);
            corrupted[pos] = profile.glitchChars[Random.Range(0, profile.glitchChars.Length)];
        }
        
        return $"<color=#{ColorUtility.ToHtmlStringRGBA(profile.glitchColor)}>{new string(corrupted)}</color>";
    }

    IEnumerator TypeCode()
    {
        isTyping = true;
        
        foreach (string line in modifiedCodeLines)
        {
            if (string.IsNullOrEmpty(line))
            {
                terminalText.text += "\n\n";
                yield return new WaitForSeconds(lineDelay * 2);
                continue;
            }
            
            terminalText.text += "\n";
            
            for (int i = 0; i < line.Length; i++)
            {
                // Random error effect
                if (Random.value < errorProbability && errorProfiles.Count > 0)
                {
                    ErrorProfile profile = errorProfiles[currentErrorProfileIndex];
                    yield return StartCoroutine(TriggerErrorEffect(profile));
                    
                    // Cycle through error profiles
                    currentErrorProfileIndex = (currentErrorProfileIndex + 1) % errorProfiles.Count;
                }
                
                terminalText.text += line[i];
                UpdateCursorPosition();
                
                if (typingSound != null)
                {
                    audioSource.pitch = Random.Range(0.9f, 1.1f);
                    audioSource.PlayOneShot(typingSound);
                }
                
                yield return new WaitForSeconds(charDelay);
            }
            
            if (lineCompleteSound != null && !string.IsNullOrEmpty(line))
            {
                audioSource.pitch = 1f;
                audioSource.PlayOneShot(lineCompleteSound);
            }
            
            yield return new WaitForSeconds(lineDelay);
        }
        
        isTyping = false;
        if (cursor != null) cursor.gameObject.SetActive(false);
        yield return StartCoroutine(ShowRunButton());
    }

    IEnumerator TriggerErrorEffect(ErrorProfile profile)
    {
        // Visual effect
        if (profile.visualEffectPrefab != null && screenEffectsParent != null)
        {
            GameObject effect = Instantiate(profile.visualEffectPrefab, screenEffectsParent);
            Destroy(effect, profile.errorDuration * 2);
        }
        
        // Screen shake
        if (profile.screenShakeIntensity > 0)
        {
            StartCoroutine(ScreenShake(profile.errorDuration, profile.screenShakeIntensity));
        }
        
        // Audio
        if (profile.errorSound != null)
        {
            audioSource.PlayOneShot(profile.errorSound);
        }
        
        // Text distortion
        string glitchChar = profile.glitchChars[Random.Range(0, profile.glitchChars.Length)].ToString();
        terminalText.text += $"<color=#{ColorUtility.ToHtmlStringRGBA(profile.glitchColor)}>{glitchChar}</color>";
        
        yield return new WaitForSeconds(profile.errorDuration);
    }

    IEnumerator ScreenShake(float duration, float intensity)
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            float x = originalPos.x + Random.Range(-1f, 1f) * intensity;
            float y = originalPos.y + Random.Range(-1f, 1f) * intensity;
            transform.position = new Vector3(x, y, originalPos.z);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.position = originalPos;
    }

    void UpdateCursorPosition()
    {
        if (cursor == null || terminalText == null) return;

        terminalText.ForceMeshUpdate();
        Canvas.ForceUpdateCanvases();

        int lastVisibleIndex = terminalText.text.Length - 1;
        if (lastVisibleIndex < 0) return;

        TMP_CharacterInfo charInfo = terminalText.textInfo.characterInfo[lastVisibleIndex];
        Vector3 charBottomRight = new Vector3(
            charInfo.bottomRight.x,
            charInfo.baseLine,
            0
        );

        Vector3 worldPos = terminalText.transform.TransformPoint(charBottomRight);
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)terminalText.transform.parent,
            RectTransformUtility.WorldToScreenPoint(null, worldPos),
            null,
            out localPos
        );

        cursor.anchoredPosition = localPos + cursorOffset;
        
        if (!cursor.gameObject.activeSelf)
            cursor.gameObject.SetActive(true);
    }

    IEnumerator BlinkCursor()
    {
        while (true)
        {
            if (cursor != null && !isTyping)
            {
                cursor.gameObject.SetActive(!cursor.gameObject.activeSelf);
            }
            yield return new WaitForSeconds(cursorBlinkSpeed);
        }
    }

    IEnumerator ShowRunButton()
    {
        runButton.gameObject.SetActive(true);
        
        CanvasGroup canvasGroup = runButton.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = runButton.gameObject.AddComponent<CanvasGroup>();
        
        float timer = 0;
        while (timer < buttonFadeInTime)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, timer / buttonFadeInTime);
            yield return null;
        }
        
        canvasGroup.alpha = 1;
    }

    void OnRunButtonClicked()
    {
        Debug.Log("Terminal command executed!");
        // Add your button functionality here
    }

    void OnDestroy()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
    }
}