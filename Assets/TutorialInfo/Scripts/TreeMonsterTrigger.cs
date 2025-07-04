using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TreeMonsterTrigger : MonoBehaviour
{
    public GameObject treeModel;       // Normal tree
    public GameObject treeMonster;     // Monster version
    public Animator treeAnimator;      // Animator on monster
    public Material glowMaterial;      // Red glow material
    public Camera mainCamera;          // Assign Main Camera here

    private Material originalMaterial;
    private Renderer treeRenderer;
    private bool hasTriggered = false;

    void Start()
    {
        if (treeModel != null)
            treeRenderer = treeModel.GetComponent<Renderer>();

        if (treeRenderer != null)
            originalMaterial = treeRenderer.material;

        if (treeMonster != null)
            treeMonster.SetActive(false); // hide monster by default
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Triggered!");
            hasTriggered = true;
            StartCoroutine(TreeSequence(other.gameObject));
        }
    }

    private IEnumerator TreeSequence(GameObject player)
    {
        // 1. Camera Shake
        if (mainCamera != null)
        {
            CameraShake shake = mainCamera.GetComponent<CameraShake>();
            if (shake != null)
                yield return StartCoroutine(shake.Shake(1f, 0.15f));
        }

        // 2. Glow the tree
        if (treeRenderer != null && glowMaterial != null)
            treeRenderer.material = glowMaterial;

        yield return new WaitForSeconds(1f); // hold glow

        // 3. Transform into monster
        if (treeModel != null) treeModel.SetActive(false);
        if (treeMonster != null) treeMonster.SetActive(true);

        if (treeAnimator != null)
            treeAnimator.Play("MonsterAttacck");

        // 4. Wait for animation to complete
        float animLength = GetAnimationLength("MonsterAttacck");
        yield return new WaitForSeconds(animLength);

        // 5. Kill player
        FloatingHealth fh = player.GetComponent<FloatingHealth>();
        if (fh != null)
            fh.KillImmediately();

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameOver");
    }

    float GetAnimationLength(string clipName)
    {
        if (treeAnimator == null || treeAnimator.runtimeAnimatorController == null)
            return 2f;

        foreach (AnimationClip clip in treeAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
                return clip.length;
        }

        return 2f;
    }
}
