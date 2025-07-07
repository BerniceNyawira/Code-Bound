using UnityEngine;
using System.Collections;

public class LongPressAnimator : MonoBehaviour
{
    [Header("Animation Settings")]
    public Animator animator;
    public string loopAnimationState = "Loop";
    public string exitAnimationState = "Exit";
    public float pressDurationThreshold = 0.3f;
    
    [Header("Feedback")]
    public ParticleSystem pressEffect;
    public AudioSource pressSound;

    private bool isPressing = false;
    private float pressStartTime;
    private Coroutine animationRoutine;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartPress();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndPress();
        }
    }

    void StartPress()
    {
        isPressing = true;
        pressStartTime = Time.time;
        animationRoutine = StartCoroutine(PressAnimationRoutine());
        
        // Play initial press effects
        if (pressEffect != null) pressEffect.Play();
        if (pressSound != null) pressSound.Play();
    }

    void EndPress()
    {
        isPressing = false;
        
        if (animationRoutine != null)
        {
            StopCoroutine(animationRoutine);
            animationRoutine = null;
        }
        
        // Trigger exit animation if we were in loop state
        if (Time.time - pressStartTime >= pressDurationThreshold)
        {
            animator.Play(exitAnimationState);
        }
    }

    IEnumerator PressAnimationRoutine()
    {
        // Wait for threshold before starting loop
        while (isPressing && Time.time - pressStartTime < pressDurationThreshold)
        {
            yield return null;
        }

        // Only start loop if still pressing
        if (isPressing)
        {
            animator.Play(loopAnimationState);
            
            // Keep animation looping while pressed
            while (isPressing)
            {
                // If animation finished (for non-looping clips), restart it
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    animator.Play(loopAnimationState, -1, 0f);
                }
                yield return null;
            }
        }
    }
}