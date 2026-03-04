using UnityEngine;

public class StretchableUShape : MonoBehaviour
{
    public Transform leftCap;
    public Transform middle;
    public Transform rightCap;

    public float defaultMiddleLength = 1f; 
    public float stretchAmount = 2f;      

    void Update()
    {
        middle.localScale = new Vector3(1f, 1f, stretchAmount);

        middle.localPosition = new Vector3(0f, 0f, 0f);

        float halfLength = (defaultMiddleLength * stretchAmount) / 2f;
        leftCap.localPosition = new Vector3(0f, 0f, -halfLength);
        rightCap.localPosition = new Vector3(0f, 0f, halfLength);
    }
}
