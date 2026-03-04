using UnityEngine;
using UnityEngine.UI;

public class DeleteButtonUI : MonoBehaviour
{
    private GameObject target;

    private void Start()
    {
        Button btn = GetComponentInChildren<Button>();
        btn.onClick.AddListener(DeleteTarget);

        gameObject.SetActive(false); // hidden by default
    }

    void Update()
    {
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180, 0); // Because by default it might face the wrong direction
        }
    }


    public void Show(Vector3 position, GameObject targetToDelete)
    {
        target = targetToDelete;
        transform.position = position;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void DeleteTarget()
    {
        if (target != null)
        {
            Destroy(target); // destroy the cylinder
        }

        Destroy(gameObject); // destroy the button itself
    }
}
