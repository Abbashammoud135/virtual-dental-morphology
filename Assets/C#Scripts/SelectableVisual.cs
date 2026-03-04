using UnityEngine;

public class SelectableVisual : MonoBehaviour
{
    public Color highlightColor = Color.yellow;

    private Renderer rend;
    private Color originalColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            originalColor = rend.material.color;
        }
    }

    public void OnSelect()
    {
        if (rend != null)
            rend.material.color = highlightColor;
    }

    public void OnDeselect()
    {
        if (rend != null)
            rend.material.color = originalColor;
    }
}
