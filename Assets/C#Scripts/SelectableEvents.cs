using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable))]
public class SelectableEvents : MonoBehaviour
{
    public Color highlightColor = Color.yellow;
    public GameObject deletingCanvasPrefab;
    public float hideDelay = 10f;

    private Renderer rend;
    private Color originalColor;
    private GameObject deleteButtonObject;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;
    private Coroutine hideCoroutine;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;

        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();

        interactable.selectEntered.AddListener(OnSelect);
        interactable.selectExited.AddListener(OnDeselect);
    }

    void Update()
    {
        if (deleteButtonObject != null && deleteButtonObject.activeSelf)
        {
            Vector3 above = transform.position + Vector3.up * 2f;
            deleteButtonObject.transform.position = above;
        }
    }

    private void OnSelect(SelectEnterEventArgs args)
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        Highlight(true);
        ShowDeleteButton();
    }

    private void OnDeselect(SelectExitEventArgs args)
    {
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideDeleteButtonAfterDelay());

        Highlight(false);
    }

    private IEnumerator HideDeleteButtonAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);

        HideDeleteButton();
    }

    private void Highlight(bool highlight)
    {
        if (rend != null)
            rend.material.color = highlight ? highlightColor : originalColor;
    }

    private void ShowDeleteButton()
    {
        if (deleteButtonObject == null && deletingCanvasPrefab != null)
        {
            deleteButtonObject = Instantiate(deletingCanvasPrefab);
            Vector3 above = transform.position + Vector3.up * 2f;
            deleteButtonObject.transform.position = above;

            DeleteButtonUI deleteButton = deleteButtonObject.GetComponent<DeleteButtonUI>();
            deleteButton.Show(above, gameObject);
        }
        else if (deleteButtonObject != null)
        {
            Vector3 above = transform.position + Vector3.up * 2f;
            deleteButtonObject.transform.position = above;
            deleteButtonObject.SetActive(true);
        }
    }

    private void HideDeleteButton()
    {
        if (deleteButtonObject != null)
            deleteButtonObject.SetActive(false);
    }
}
