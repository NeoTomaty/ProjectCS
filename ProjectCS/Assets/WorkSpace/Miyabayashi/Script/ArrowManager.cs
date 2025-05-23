using UnityEngine;

public class Arrow3DManager : MonoBehaviour
{
    public Transform target;           // �ǂ��Ώ�
    public Camera mainCamera;          // ���̕�������Ɏg���J����
    public Camera arrowCamera;         // ����\�������p�J����

    public GameObject leftArrow;       // �����
    public GameObject rightArrow;      // �E���

    [Header("arrowCamera��̃I�t�Z�b�g")]
    public Vector3 leftArrowOffset = new Vector3(-0.5f, 0f, 2f);
    public Vector3 rightArrowOffset = new Vector3(0.5f, 0f, 2f);

    void Update()
    {
        Vector3 toTarget = (target.position - mainCamera.transform.position).normalized;
        Vector3 camForward = mainCamera.transform.forward;
        Vector3 camRight = mainCamera.transform.right;

        float forwardDot = Vector3.Dot(camForward, toTarget);

        // ���ʂɋ߂��Ȃ�\�����Ȃ�
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
