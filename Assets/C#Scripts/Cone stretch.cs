using UnityEngine;

public class ConeStretch : MonoBehaviour
{
    public float stretchAmount = 2f;   // How much to stretch
    public float stretchSpeed = 5f;    // Speed of stretching
    public KeyCode stretchKey = KeyCode.Space; // Key to trigger stretch

    private Vector3 originalScale;
    private bool isStretching = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Detect key press
        if (Input.GetKeyDown(stretchKey))
        {
            isStretching = !isStretching; // Toggle stretching
        }

        // Determine target scale
        Vector3 targetScale = isStretching
            ? new Vector3(originalScale.x, originalScale.y * stretchAmount, originalScale.z)
            : originalScale;

        // Smoothly scale the cylinder/cone
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * stretchSpeed);
    }
}
