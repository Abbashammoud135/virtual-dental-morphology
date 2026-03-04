using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ConeManipulator : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private bool isSelected = false;

    public float rotationSpeed = 50f;
    public float scaleStep = 0.1f;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        grabInteractable.selectExited.RemoveListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        isSelected = true;
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        isSelected = false;
    }

    void Update()
    {
        if (!isSelected) return;

        // Rotate with number keys
        if (Input.GetKey(KeyCode.Alpha2))
            transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.Alpha8))
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.Alpha5))
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // Scale radius (x & z)
        if (Input.GetKey(KeyCode.Alpha4))
            transform.localScale -= new Vector3(scaleStep, 0, scaleStep) * Time.deltaTime;

        if (Input.GetKey(KeyCode.Alpha6))
            transform.localScale += new Vector3(scaleStep, 0, scaleStep) * Time.deltaTime;

        // Scale height (y)
        if (Input.GetMouseButton(0)) // Left click
            transform.localScale += new Vector3(0, scaleStep, 0) * Time.deltaTime;

        if (Input.GetMouseButton(1)) // Right click
            transform.localScale -= new Vector3(0, scaleStep, 0) * Time.deltaTime;
    }
}
