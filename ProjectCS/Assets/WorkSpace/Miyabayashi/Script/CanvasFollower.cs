using UnityEngine;

public class CanvasFollower : MonoBehaviour
{
    public Transform cameraTransform;
    public float distance = 2f;

    void LateUpdate()
    {
        transform.position = cameraTransform.position + cameraTransform.forward * distance;
        transform.rotation = Quaternion.LookRotation(cameraTransform.forward);
    }
}
