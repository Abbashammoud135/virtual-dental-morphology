using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


public class CylinderStretch : MonoBehaviour
{
    [Header("Stretch Settings")]
    public float stretchSpeed = 1f;
    [Range(0.01f, 10f)] public float minScale = 0.005f;
    [Range(0.01f, 10f)] public float maxScale = 200f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 90f;

    [Header("XR Input Actions")]
    public InputActionProperty leftTrigger;   // increase size
    public InputActionProperty rightTrigger;  // decrease size
    public InputActionProperty rightThumbstick; // optional rotation

    private bool isSelected = false;
    private Vector3 initialScale;
    private SelectableVisual visual;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private bool isReleased = false;

    [Header("Extra Movement & Radius")]
    public float moveSpeed = 0.2f;
    public float radiusStretchSpeed = 0.5f;

    [Header("Button Actions")]
    //public InputActionProperty leftXButton;   // move up
    //public InputActionProperty leftYButton;   // move down
    public InputActionProperty rightAButton;  // increase radius
    public InputActionProperty rightBButton;  // decrease radius

    void Start()
    {
        // Ensure collider exists
        if (GetComponent<Collider>() == null)
            gameObject.AddComponent<MeshCollider>().convex = true;

        initialScale = transform.localScale;
        visual = GetComponent<SelectableVisual>();

        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grabInteractable == null)
        {
            grabInteractable = gameObject.AddComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        }

        // Subscribe to grab events
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        // Enable input actions
        leftTrigger.action.Enable();
        rightTrigger.action.Enable();
        rightThumbstick.action.Enable();
        //leftXButton.action.Enable();
        //leftYButton.action.Enable();
        rightAButton.action.Enable();
        rightBButton.action.Enable();

    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // Toggle selection state
        isSelected = !isSelected;
        Renderer rend = GetComponent<Renderer>();
        isReleased = false;
        if (isSelected)
        {
            //rend.material.color = Color.yellow;

            visual?.OnSelect();
            //Debug.Log($"{gameObject.name} Selected (isSelected={isSelected})");
        }
        else
        {
            // Optionally keep color yellow, so don't call OnDeselect
            //Debug.Log($"{gameObject.name} Deselected via grab toggle (isSelected={isSelected})");
        }

    }
    //void Awake()
    //{
    //    Renderer rend = GetComponent<Renderer>();
    //    rend.material = new Material(rend.material); // clone the material
    //}
    private void OnRelease(SelectExitEventArgs args)
    {
        isReleased = true;
    }

    void Update()
    {
        if (!isSelected) return;
        if (isSelected && isReleased)
        {
            Renderer rend = GetComponent<Renderer>();
            rend.material.color = Color.yellow;

        }
        Vector3 scale = transform.localScale;

        // Stretch with triggers
        float leftVal = leftTrigger.action.ReadValue<float>();
        //Debug.Log($"left value = {leftVal}");

        float rightVal = rightTrigger.action.ReadValue<float>();
        if (leftVal > 0.1f) scale.y += stretchSpeed * Time.deltaTime;
        if (rightVal > 0.1f) scale.y -= stretchSpeed * Time.deltaTime;
        // ===== RADIUS STRETCH (X & Z) =====
        if (rightAButton.action.IsPressed())
        {
            scale.x += radiusStretchSpeed * Time.deltaTime;
            scale.z += radiusStretchSpeed * Time.deltaTime;
        }

        if (rightBButton.action.IsPressed())
        {
            scale.x -= radiusStretchSpeed * Time.deltaTime;
            scale.z -= radiusStretchSpeed * Time.deltaTime;
        }

        //Debug.Log($"right value = {rightVal}");

        // Clamp scale
        scale.x = Mathf.Clamp(scale.x, initialScale.x * minScale, initialScale.x * maxScale);
        scale.y = Mathf.Clamp(scale.y, initialScale.y * minScale, initialScale.y * maxScale);
        scale.z = Mathf.Clamp(scale.z, initialScale.z * minScale, initialScale.z * maxScale);

        transform.localScale = scale;

        Vector2 r = rightThumbstick.action.ReadValue<Vector2>();

        // X-axis rotation (pitch) from stick Y
        transform.Rotate(Vector3.right, -r.y * rotationSpeed * Time.deltaTime, Space.Self);

        // Z-axis rotation (roll) from stick X
        transform.Rotate(Vector3.forward, r.x * rotationSpeed * Time.deltaTime, Space.Self);
    }


}
