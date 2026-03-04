using UnityEngine;

public class UShapeGenerator : MonoBehaviour
{
    public GameObject segmentPrefab;   // Cylinder or pipe segment
    public int segmentCount = 40;
    public float radius = 1.0f;
    public float totalAngle = 270f;    // U = 180 degrees

    void Start()
    {
        float angleStep = totalAngle / (segmentCount - 1);

        for (int i = 0; i < segmentCount; i++)
        {
            float angle = -totalAngle / 2 + i * angleStep; // From -90 to +90
            float rad = Mathf.Deg2Rad * angle;

            // Position along arc
            float x = Mathf.Cos(rad) * radius;
            float z = Mathf.Sin(rad) * radius;

            // Instantiate segment
            GameObject segment = Instantiate(segmentPrefab, transform);
            segment.transform.localPosition = new Vector3(x, 0, z);

            // Rotate to face next direction along arc
            segment.transform.rotation = Quaternion.Euler(0, -angle, 0);
        }
    }
}
