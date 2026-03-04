using UnityEngine;

public class SpawnCylinder : MonoBehaviour
{
    public GameObject cylinderPrefab;   // Assign this from the Project window!
    public Transform playerHead;        // Usually the XR Camera (Main Camera)

    public float distanceInFront = 2f;

    public void Spawn()
    {
        if (cylinderPrefab == null || playerHead == null)
        {
            Debug.LogWarning("Prefab or Player Head not assigned.");
            return;
        }

        Vector3 spawnPosition = playerHead.position + playerHead.forward * distanceInFront;
        Quaternion spawnRotation = Quaternion.identity;

        GameObject instance = Instantiate(cylinderPrefab, spawnPosition, spawnRotation);

        instance.SetActive(true);

   
    }
}
