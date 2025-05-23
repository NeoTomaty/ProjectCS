using UnityEngine;


public class ArrowGuide: MonoBehaviour
{
    [Header("�Q��")]
    public Transform player;          // �v���C���[��Transform
    public Transform target;          // �^�[�Q�b�g��Transform
    public Camera mainCamera;         // ���C���J����
    public Transform arrowObject;     // ����3D�I�u�W�F�N�g

    [Header("�����p�p�����[�^")]
    public float arrowDistance = 2f;          // �v���C���[������܂ł̋���
    public float arrowHeightOffset = 1f;      // ���̍����iY���I�t�Z�b�g�j
    public float rotationSmoothSpeed = 5f;    // ��]��ԑ��x
    public float moveSmoothSpeed = 5f;        // �ړ���ԑ��x
    public float showThresholdDot = 0f;       // ���ʂƂ݂Ȃ�Dot臒l

    Vector3 previousDirection = Vector3.right;

    void Update()
    {
        if (!player || !target || !mainCamera || !arrowObject)
            return;

        Vector3 toTarget = target.position - player.position;
        Vector3 flatToTarget = Vector3.ProjectOnPlane(toTarget, Vector3.up).normalized;

        if (flatToTarget == Vector3.zero)
        {
            arrowObject.gameObject.SetActive(false);
            return;
        }

        Vector3 camForward = Vector3.ProjectOnPlane(mainCamera.transform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.Cross(Vector3.up, camForward).normalized;

        float dotForward = Vector3.Dot(camForward, flatToTarget);

        Vector3 displayDir;
        if (dotForward >= showThresholdDot)
        {
            displayDir = flatToTarget;
            previousDirection = Vector3.Dot(camRight, flatToTarget) >= 0 ? camRight : -camRight;
        }
        else
        {
            float dotRight = Vector3.Dot(camRight, flatToTarget);
            displayDir = dotRight >= 0 ? camRight : -camRight;
            previousDirection = displayDir;
        }

        // �� ���������t���̖ڕW�ʒu���v�Z
        Vector3 targetPosition = player.position + displayDir * arrowDistance;
        targetPosition.y = player.position.y + arrowHeightOffset;

        // �ړ��Ɖ�]�̕��
        arrowObject.position = Vector3.Lerp(arrowObject.position, targetPosition, Time.deltaTime * moveSmoothSpeed);
        Quaternion targetRot = Quaternion.LookRotation(flatToTarget) * Quaternion.Euler(0, 90f, 0);
        arrowObject.rotation = Quaternion.Slerp(arrowObject.rotation, targetRot, Time.deltaTime * rotationSmoothSpeed);

        arrowObject.gameObject.SetActive(true);
    }
}
