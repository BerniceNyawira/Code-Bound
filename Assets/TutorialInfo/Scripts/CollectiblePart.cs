using UnityEngine;

public class CollectiblePart : MonoBehaviour
{
    public AudioClip pickupSound;
    public GameObject glowEffect;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Play gather animation
            Animator anim = other.GetComponent<Animator>();
            if (anim != null)
                anim.SetTrigger("collect");

            // Visual effect
            if (glowEffect != null)
                Instantiate(glowEffect, transform.position, Quaternion.identity);

            // Sound effect
            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            // Add to inventory
            InventoryManager.Instance.AddItem(name);

            // Destroy object
            Destroy(gameObject);
        }
    }
}

