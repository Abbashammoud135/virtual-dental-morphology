using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectMerge : MonoBehaviour
{
    public GameObject mergeButtonPrefab; // Assign a World Space Canvas Button prefab here
    private GameObject currentButton;
    private Transform mergeTarget;

    void OnTriggerEnter(Collider other)
    {
        // Only trigger if the other object is a mergeable type
        if (other.CompareTag("Mergeable") && currentButton == null)
        {
            mergeTarget = other.transform;
            ShowMergeButton();
        }
    }

    void ShowMergeButton()
    {
        currentButton = Instantiate(mergeButtonPrefab,
                                     (transform.position + mergeTarget.position) / 2,
                                     Quaternion.identity);

        currentButton.transform.LookAt(Camera.main.transform); // Face the player
        Button btn = currentButton.GetComponentInChildren<Button>();
        btn.onClick.AddListener(MergeObjects);

        // Auto-hide after 3 seconds if not clicked
        StartCoroutine(HideButtonAfterSeconds(3f));
    }

    IEnumerator HideButtonAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (currentButton != null)
            Destroy(currentButton);
    }

    void MergeObjects()
    {
        mergeTarget.SetParent(transform); // Parent the other to this one
        mergeTarget.localPosition = Vector3.zero; // Optional: move inside
        Destroy(currentButton);
    }
}
