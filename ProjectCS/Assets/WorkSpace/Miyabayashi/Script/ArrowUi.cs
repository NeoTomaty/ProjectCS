using UnityEngine;
using UnityEngine.UI;

public class ArrowUi : MonoBehaviour
{
    public Transform target;
    public Camera cam;
    public Image leftArrow;
    public Image rightArrow;

    void Update()
    {
        Vector3 toTarget = (target.position - cam.transform.position).normalized;
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;

        // �^�[�Q�b�g��������ɂ��邩�i��ʂɉf���Ă邩�j
        Vector3 viewportPos = cam.WorldToViewportPoint(target.position);
        bool isVisible = viewportPos.z > 0 &&
                         viewportPos.x >= 0 && viewportPos.x <= 1 &&
                         viewportPos.y >= 0 && viewportPos.y <= 1;

        if (isVisible)
        {
            leftArrow.enabled = false;
            rightArrow.enabled = false;
        }
        else
        {
            float dot = Vector3.Dot(camRight, toTarget);

            if (dot < 0)
            {
                // ����
                leftArrow.enabled = true;
                rightArrow.enabled = false;
            }
            else
            {
                // �E��
                leftArrow.enabled = false;
                rightArrow.enabled = true;
            }
        }
    }
}
