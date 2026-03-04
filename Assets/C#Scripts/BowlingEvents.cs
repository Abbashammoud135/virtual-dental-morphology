using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class BowlingEvents : MonoBehaviour
{
    [Header("Delete UI")]
    public GameObject deletingCanvasPrefab;
    public float hideDelay = 10f;
    public float abovePadding = 0.05f;

    private GameObject deleteButtonObject;
    private Coroutine hideCoroutine;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
        if (interactable == null)
        {
            Debug.LogError($"BowlingEvents on {name}: No XRBaseInteractable found. Add XR Grab Interactable to this object.");
            enabled = false;
            return;
        }

        interactable.selectEntered.AddListener(OnSelect);
        interactable.selectExited.AddListener(OnDeselect);
    }

    void Update()
    {
        if (deleteButtonObject != null && deleteButtonObject.activeSelf)
            deleteButtonObject.transform.position = GetAbovePoint();
    }

    private void OnSelect(SelectEnterEventArgs args)
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        ShowDeleteButton();
    }

    private void OnDeselect(SelectExitEventArgs args)
    {
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideDeleteButtonAfterDelay());
    }

    private IEnumerator HideDeleteButtonAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);
        HideDeleteButton();
    }

    private Vector3 GetAbovePoint()
    {
        Collider col = GetComponentInChildren<Collider>();
        if (col != null)
        {
            Bounds b = col.bounds;
            return new Vector3(b.center.x, b.max.y + abovePadding, b.center.z);
        }

        Renderer r = GetComponentInChildren<Renderer>();
        if (r != null)
        {
            Bounds b = r.bounds;
            return new Vector3(b.center.x, b.max.y + abovePadding, b.center.z);
        }

        return transform.position + Vector3.up * (0.5f + abovePadding);
    }

    private void ShowDeleteButton()
    {
        if (deletingCanvasPrefab == null) return;

        Vector3 above = GetAbovePoint();

        if (deleteButtonObject == null)
            deleteButtonObject = Instantiate(deletingCanvasPrefab);

        deleteButtonObject.transform.position = above;
        deleteButtonObject.SetActive(true);

        DeleteButtonUI ui = deleteButtonObject.GetComponent<DeleteButtonUI>();
        if (ui != null)
            ui.Show(above, gameObject); // deletes the ROOT (BowlingPin_Interactable)
    }

    private void HideDeleteButton()
    {
        if (deleteButtonObject != null)
            deleteButtonObject.SetActive(false);
    }
}
