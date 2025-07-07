using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AdvancedTerminalWithTransition : MonoBehaviour
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
    [SerializeField] private List<ErrorProfile> errorProfiles = new List<ErrorProfile>();
    [SerializeField] private Transform screenEffectsParent;
    
    [Header("Audio")]
    [SerializeField] private AudioClip typingSound;
    [SerializeField] private AudioClip lineCompleteSound;
    [SerializeField] private AudioSource audioSource;
    
    [Header("Run Button")]
    [SerializeField] private Button runButton;
    [SerializeField] private float buttonFadeInTime = 1f;

    [Header("Scene Transition")]
    [SerializeField] private string nextSceneName = "NextScene";
    [SerializeField] private float fadeOutDuration = 1.5f;
    [SerializeField] private Image fadeOverlay;
    [SerializeField] private Material glitchMaterial;
    [SerializeField] private float glitchDuration = 0.8f;

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
        "> ERROR: UNABLE TO TRANSMIT CODE",
        "> ERROR: UNABLE TO ACCESS EXIT GATE",
        "> ERROR: SYSTEM MALFUNCTION DETECTED",
        "> ERROR: UNABLE TO EXECUTE COMMAND",
        "> ERROR: UNABLE TO ACCESS ESCAPE ROUTE",
        "",
        "RETRYING...",
        "> ACCESSING CORE MODE...",
        "> validating.components(): OK",
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

    void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    void Start()
    {
        // Prepare terminal
        modifiedCodeLines = new List<string>(originalCodeLines);
        terminalText.text = "";

        // Run button hidden until type finishes
        if (runButton != null)
        {
            runButton.gameObject.SetActive(false);
            runButton.onClick.AddListener(OnRunButtonClicked);
        }

        // Cursor setup
        if (cursor != null)
        {
            cursor.gameObject.SetActive(false);
            StartCoroutine(BlinkCursor());
        }

        // Default error profile if empty
        if (errorProfiles.Count == 0)
        {
            errorProfiles.Add(new ErrorProfile { name="Default", glitchColor=Color.red, glitchChars="�#%&@$*", errorDuration=0.3f });
        }

        // Fade overlay setup
        if (fadeOverlay != null)
        {
            fadeOverlay.color = new Color(0,0,0,0);
            fadeOverlay.gameObject.SetActive(false);
        }

        // Start typewriter
        typingCoroutine = StartCoroutine(TypeCode());
    }

    void CorruptRandomLines()
    {
        for (int i = 0; i < modifiedCodeLines.Count; i++)
        {
            if (!string.IsNullOrEmpty(modifiedCodeLines[i]) && Random.value < glitchLineProbability)
            {
                modifiedCodeLines[i] = ApplyErrorProfile(
                    modifiedCodeLines[i],
                    errorProfiles[Random.Range(0, errorProfiles.Count)]
                );
            }
        }
    }

    string ApplyErrorProfile(string original, ErrorProfile profile)
    {
        if (string.IsNullOrEmpty(original)) return original;
        
        int errorsToAdd = Mathf.Clamp(Random.Range(1, original.Length/4), 1, original.Length);
        char[] corrupted = original.ToCharArray();
        for (int e = 0; e < errorsToAdd; e++)
        {
            int pos = Random.Range(0, corrupted.Length);
            if (profile.glitchChars.Length > 0)
                corrupted[pos] = profile.glitchChars[Random.Range(0,profile.glitchChars.Length)];
        }
        return $"<color=#{ColorUtility.ToHtmlStringRGBA(profile.glitchColor)}>{new string(corrupted)}</color>";
    }

    IEnumerator TypeCode()
    {
        isTyping = true;
        CorruptRandomLines();
        
        foreach (string line in modifiedCodeLines)
        {
            if (string.IsNullOrEmpty(line))
            {
                terminalText.text += "\n\n";
                yield return new WaitForSeconds(lineDelay*2);
                continue;
            }
            
            terminalText.text += "\n";
            for (int i=0; i<line.Length; i++)
            {
                if (Random.value < errorProbability && errorProfiles.Count>0)
                {
                    yield return StartCoroutine(TriggerErrorEffect(errorProfiles[currentErrorProfileIndex]));
                    currentErrorProfileIndex = (currentErrorProfileIndex+1)%errorProfiles.Count;
                }

                terminalText.text += line[i];
                UpdateCursorPosition();

                if (typingSound!=null)
                {
                    audioSource.pitch = Random.Range(0.9f,1.1f);
                    audioSource.PlayOneShot(typingSound);
                }
                yield return new WaitForSeconds(charDelay);
            }

            if (lineCompleteSound!=null)
                audioSource.PlayOneShot(lineCompleteSound);

            yield return new WaitForSeconds(lineDelay);
        }

        isTyping = false;
        if (cursor!=null) cursor.gameObject.SetActive(false);
        yield return StartCoroutine(ShowRunButton());
    }

    IEnumerator TriggerErrorEffect(ErrorProfile p)
    {
        if (p.visualEffectPrefab!=null && screenEffectsParent!=null)
        {
            var fx = Instantiate(p.visualEffectPrefab, screenEffectsParent);
            Destroy(fx, p.errorDuration*2);
        }
        if (p.screenShakeIntensity>0) StartCoroutine(ScreenShake(p.errorDuration,p.screenShakeIntensity));
        if (p.errorSound!=null) audioSource.PlayOneShot(p.errorSound);

        if (p.glitchChars.Length>0)
        {
            char gc = p.glitchChars[Random.Range(0,p.glitchChars.Length)];
            terminalText.text += $"<color=#{ColorUtility.ToHtmlStringRGBA(p.glitchColor)}>{gc}</color>";
        }

        yield return new WaitForSeconds(p.errorDuration);
    }

    IEnumerator ScreenShake(float duration, float intensity)
    {
        Vector3 orig = transform.position;
        float t=0;
        while(t<duration)
        {
            transform.position = orig + new Vector3(
                Random.Range(-1f,1f)*intensity,
                Random.Range(-1f,1f)*intensity,
                0
            );
            t+=Time.deltaTime;
            yield return null;
        }
        transform.position = orig;
    }

    void UpdateCursorPosition()
    {
        if (cursor==null || terminalText==null) return;
        terminalText.ForceMeshUpdate();
        Canvas.ForceUpdateCanvases();
        int idx = terminalText.text.Length-1;
        if(idx<0) return;
        var ci = terminalText.textInfo.characterInfo[idx];
        Vector3 br = new Vector3(ci.bottomRight.x, ci.baseLine,0);
        Vector3 world = terminalText.transform.TransformPoint(br);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            terminalText.rectTransform.parent as RectTransform,
            RectTransformUtility.WorldToScreenPoint(null,world),
            null,
            out Vector2 local
        );
        cursor.anchoredPosition = local + cursorOffset;
        if(!cursor.gameObject.activeSelf) cursor.gameObject.SetActive(true);
    }

    IEnumerator BlinkCursor()
    {
        while(true)
        {
            if(!isTyping && cursor!=null)
                cursor.gameObject.SetActive(!cursor.gameObject.activeSelf);
            yield return new WaitForSeconds(cursorBlinkSpeed);
        }
    }

    IEnumerator ShowRunButton()
    {
        if (runButton==null) yield break;
        runButton.gameObject.SetActive(true);
        var cg = runButton.GetComponent<CanvasGroup>() ?? runButton.gameObject.AddComponent<CanvasGroup>();
        float t=0;
        while(t<buttonFadeInTime)
        {
            cg.alpha = Mathf.Lerp(0,1,t/buttonFadeInTime);
            t+=Time.deltaTime;
            yield return null;
        }
        cg.alpha=1;
    }

    private void OnRunButtonClicked()
    {
        // start the transition coroutine
        StartCoroutine(TransitionToNextScene());
    }

    IEnumerator TransitionToNextScene()
    {
        // disable the run button
        if (runButton!=null) runButton.interactable = false;

        // choose fade or glitch
        if (fadeOverlay!=null && glitchMaterial==null)
            yield return StartCoroutine(FadeScreen());
        else if (glitchMaterial!=null)
            yield return StartCoroutine(GlitchTransition());
        else
            yield return new WaitForSeconds(0.5f);

        // finally load the next scene
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        else
            Debug.LogWarning("Next scene name not set!");
    }

    IEnumerator FadeScreen()
    {
        fadeOverlay.gameObject.SetActive(true);
        float t=0;
        while(t<fadeOutDuration)
        {
            fadeOverlay.color = Color.Lerp(new Color(0,0,0,0),Color.black,t/fadeOutDuration);
            t+=Time.deltaTime;
            yield return null;
        }
        fadeOverlay.color = Color.black;
    }

    IEnumerator GlitchTransition()
    {
        fadeOverlay.gameObject.SetActive(true);
        fadeOverlay.material = glitchMaterial;
        float t=0;
        while(t<glitchDuration)
        {
            float p = t/glitchDuration;
            glitchMaterial.SetFloat("_GlitchIntensity",p);
            glitchMaterial.SetFloat("_ScanLineJitter",p*0.5f);
            fadeOverlay.color = new Color(0,0,0,p);
            t+=Time.deltaTime;
            yield return null;
        }
        fadeOverlay.color = Color.black;
    }

    void OnDestroy()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
    }
}
