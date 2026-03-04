using UnityEngine;

public class CylinderStretchController : MonoBehaviour
{
    public Transform cylinder;
    public Transform handleTop;
    public Transform handleBottom;
    public Transform handleLeft;
    public Transform handleRight;

    private float initialHeight;
    private float initialRadius;
    private float baseScaleY;
    private float baseScaleX;

    void Start()
    {
        // Assume the cylinder is vertically aligned (Y axis is up)
        baseScaleY = cylinder.localScale.y; // Original height scale
        baseScaleX = cylinder.localScale.x; // Original radius scale

        initialHeight = Vector3.Distance(handleTop.position, handleBottom.position);
        initialRadius = Vector3.Distance(handleLeft.position, handleRight.position) / 2f;
    }

    void Update()
    {
        // Update height (Y)
        float currentHeight = Vector3.Distance(handleTop.position, handleBottom.position);
        float heightRatio = currentHeight / initialHeight;
        Vector3 scale = cylinder.localScale;
        scale.y = baseScaleY * heightRatio;

        // Update radius (X/Z)
        float currentRadius = Vector3.Distance(handleLeft.position, handleRight.position) / 2f;
        float radiusRatio = currentRadius / initialRadius;
        scale.x = baseScaleX * radiusRatio;
        scale.z = baseScaleX * radiusRatio;

        cylinder.localScale = scale;

        // Move the cylinder center between the top and bottom handles
        cylinder.position = (handleTop.position + handleBottom.position) / 2f;
    }
}
