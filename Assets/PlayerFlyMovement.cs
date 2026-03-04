using UnityEngine;

public class PlayerFlyMovement : MonoBehaviour
{
    public float minX = -5f, maxX = 5f;
    public float minY = 0f, maxY = 3f;
    public float minZ = -5f, maxZ = 5f;

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        transform.position = pos;
    }
}
