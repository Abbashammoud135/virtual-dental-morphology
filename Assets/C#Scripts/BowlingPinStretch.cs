using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
public class BowlingPinStretch : MonoBehaviour
{
    public enum Axis { X, Y, Z }

    [Header("Visual Target (child mesh ONLY for scaling if you want)")]
    public Transform visualRoot;                 // drag Cylinder (mesh child) here
    public Axis stretchAxis = Axis.Z;            // the "height" axis for stretching

    [Header("Stretch Settings")]
    public float stretchSpeed = 1f;
    [Range(0.01f, 10f)] public float minScale = 0.5f;
    [Range(0.01f, 10f)] public float maxScale = 3f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 90f;

    [Header("XR Input Actions")]
    public InputActionProperty leftTrigger;        // increase height
    public InputActionProperty rightTrigger;       // decrease height
    public InputActionProperty rightThumbstick;    // rotation (Vector2)

    [Header("Selection / Highlight")]
    public BowlingVisual visual;                   // leave empty to auto-find

    [Header("Radius (Circular)")]
    public float radiusStretchSpeed = 0.5f;

    [Header("Button Actions (Radius)")]
    public InputActionProperty rightAButton;       // increase radius
    public InputActionProperty rightBButton;       // decrease radius

    private bool isSelected = false;               // toggled by grab
    private Vector3 initialScale;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Awake()
    {
        if (visualRoot == null) visualRoot = transform;
        if (visual == null) visual = GetComponent<BowlingVisual>();

        initialScale = visualRoot.localScale;

        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    void OnEnable()
    {
        leftTrigger.action?.Enable();
        rightTrigger.action?.Enable();
        rightThumbstick.action?.Enable();
        rightAButton.action?.Enable();
        rightBButton.action?.Enable();
    }

    void OnDisable()
    {
        leftTrigger.action?.Disable();
        rightTrigger.action?.Disable();
        rightThumbstick.action?.Disable();
        rightAButton.action?.Disable();
        rightBButton.action?.Disable();
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // Toggle selection ON/OFF on each grab
        isSelected = !isSelected;

        // Keep highlighted while selected, even after release
        visual?.SetHighlighted(isSelected);
    }

    void Update()
    {
        if (!isSelected) return;

        Vector3 s = visualRoot.localScale;

        // --- Main axis stretch (height) using triggers ---
        float inc = leftTrigger.action.ReadValue<float>();
        float dec = rightTrigger.action.ReadValue<float>();

        float heightDelta = 0f;
        if (inc > 0.1f) heightDelta += stretchSpeed * Time.deltaTime;
        if (dec > 0.1f) heightDelta -= stretchSpeed * Time.deltaTime;

        if (Mathf.Abs(heightDelta) > 0f)
        {
            int main = AxisIndex(stretchAxis);
            s[main] = Mathf.Clamp(s[main] + heightDelta, initialScale[main] * minScale, initialScale[main] * maxScale);
        }

        // --- Radius stretch (CIRCULAR): scale BOTH perpendicular axes equally ---
        float radialDelta = 0f;
        if (rightAButton.action.IsPressed()) radialDelta += radiusStretchSpeed * Time.deltaTime;
        if (rightBButton.action.IsPressed()) radialDelta -= radiusStretchSpeed * Time.deltaTime;

        if (Mathf.Abs(radialDelta) > 0f)
        {
            int main = AxisIndex(stretchAxis);

            // the other two axes are the radius axes
            int a = (main + 1) % 3;
            int b = (main + 2) % 3;

            s[a] = Mathf.Clamp(s[a] + radialDelta, initialScale[a] * minScale, initialScale[a] * maxScale);
            s[b] = Mathf.Clamp(s[b] + radialDelta, initialScale[b] * minScale, initialScale[b] * maxScale);
        }

        // Apply final scale
        visualRoot.localScale = s;

        // --- Rotate parent so collider rotates too ---
        Vector2 r = rightThumbstick.action.ReadValue<Vector2>();

        // X and Z rotation (no Y):
        transform.Rotate(Vector3.right, -r.y * rotationSpeed * Time.deltaTime, Space.Self);   // X
        transform.Rotate(Vector3.forward, r.x * rotationSpeed * Time.deltaTime, Space.Self);  // Z
    }

    private int AxisIndex(Axis a) => a == Axis.X ? 0 : (a == Axis.Y ? 1 : 2);
}