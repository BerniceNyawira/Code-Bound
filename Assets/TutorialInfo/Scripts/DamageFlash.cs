using UnityEngine;
using UnityEngine.UI;

public class DamageFlash : MonoBehaviour
{
    public Image flashImage;
    public float flashDuration = 0.2f;
    public Color flashColor = new Color(1, 0, 0, 0.4f);

    private Color originalColor;
    private float flashTimer;
    private bool isFlashing;

    void Start()
    {
        if (flashImage == null)
            flashImage = GetComponent<Image>();

        originalColor = flashImage.color;
        flashImage.color = Color.clear;
        
    }

    void Update()
    {
        if (isFlashing)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0)
            {
                flashImage.color = Color.clear;
                isFlashing = false;
            }
        }
    }

    public void Flash()
    {
        flashImage.color = flashColor;
        flashTimer = flashDuration;
        isFlashing = true;
        
    }
}

