using UnityEngine;

public class Arrow3DManager : MonoBehaviour
{
    public Transform target;           // 追う対象
    public Camera mainCamera;          // 矢印の方向判定に使うカメラ
    public Camera arrowCamera;         // 矢印を表示する専用カメラ

    public GameObject leftArrow;       // 左矢印
    public GameObject rightArrow;      // 右矢印

    [Header("arrowCamera基準のオフセット")]
    public Vector3 leftArrowOffset = new Vector3(-0.5f, 0f, 2f);
    public Vector3 rightArrowOffset = new Vector3(0.5f, 0f, 2f);

    void Update()
    {
        Vector3 toTarget = (target.position - mainCamera.transform.position).normalized;
        Vector3 camForward = mainCamera.transform.forward;
        Vector3 camRight = mainCamera.transform.right;

        float forwardDot = Vector3.Dot(camForward, toTarget);

        // 正面に近いなら表示しない
        if (forwardDot > 0.7f)
        {
            leftArrow.SetActive(false);
            rightArrow.SetActive(false);
            return;
        }

        float rightDot = Vector3.Dot(camRight, toTarget);

        bool showLeft = rightDot < 0;
        bool showRight = rightDot > 0;

        leftArrow.SetActive(showLeft);
        rightArrow.SetActive(showRight);

        if (showLeft)
        {
            leftArrow.transform.position = arrowCamera.transform.position + arrowCamera.transform.rotation * leftArrowOffset;
            leftArrow.transform.rotation = Quaternion.LookRotation(arrowCamera.transform.forward, Vector3.up);
        }

        if (showRight)
        {
            rightArrow.transform.position = arrowCamera.transform.position + arrowCamera.transform.rotation * rightArrowOffset;
            rightArrow.transform.rotation = Quaternion.LookRotation(arrowCamera.transform.forward, Vector3.up);
        }
    }
}
