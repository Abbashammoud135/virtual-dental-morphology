using UnityEngine;

public class BowlingVisual : MonoBehaviour
{
    public Color highlightColor = Color.yellow;

    private Renderer rend;
    private Color originalColor;

    void Awake()
    {
        // Renderer is usually on a child (FBX -> Cylinder)
        rend = GetComponentInChildren<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;
    }

    public void SetHighlighted(bool highlighted)
    {
        if (rend == null) return;
        rend.material.color = highlighted ? highlightColor : originalColor;
    }
}
