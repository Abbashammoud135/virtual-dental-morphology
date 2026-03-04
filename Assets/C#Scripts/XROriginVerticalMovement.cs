using UnityEngine;
using UnityEngine.InputSystem;

public class XROriginVerticalMovement : MonoBehaviour
{
    [Header("XR Origin Movement Settings")]
    public float moveSpeed = 0.2f;               // Speed of vertical movement

    [Header("XR Input Actions")]
    public InputActionProperty leftPrimaryButton;   // Move up (X)
    public InputActionProperty leftSecondaryButton; // Move down (Y)

    private void Awake()
    {
        // Enable input actions
        leftPrimaryButton.action?.Enable();
        leftSecondaryButton.action?.Enable();
    }

    private void OnDisable()
    {
        leftPrimaryButton.action?.Disable();
        leftSecondaryButton.action?.Disable();
    }

    void Update()
    {
        // Move player up
        if (leftPrimaryButton.action.IsPressed())
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        }

        // Move player down
        if (leftSecondaryButton.action.IsPressed())
        {
            transform.position -= Vector3.up * moveSpeed * Time.deltaTime;
        }
    }
}
