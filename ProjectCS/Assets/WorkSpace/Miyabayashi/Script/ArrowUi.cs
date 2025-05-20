//======================================================
// ArrowUi�X�N���v�g
// �쐬�ҁF�{��
// �ŏI�X�V���F5/7
// 
// [Log]�T/7 �{�с@�X�i�b�N�����ւ̖��\��
//======================================================
using UnityEngine;
using UnityEngine.UI;

public class ArrowUi : MonoBehaviour
{
    public Transform target;
    public Camera cam;
    public Image leftArrow;
    public Image rightArrow;
    public Image upArrow;

    void Update()
    {
        Vector3 toTarget = (target.position - cam.transform.position).normalized;
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;

        Vector3 viewportPos = cam.WorldToViewportPoint(target.position);

        // �S�Ă̖����\���ɂ��Ă�������`�F�b�N
        leftArrow.enabled = false;
        rightArrow.enabled = false;
        upArrow.enabled = false;

        // --- �w�� or ���� ---
        bool isBehind = viewportPos.z < 0;
        bool isInXRange = viewportPos.x >= 0 && viewportPos.x <= 1;
        bool isInYRange = viewportPos.y >= 0 && viewportPos.y <= 1;

        if (!isBehind && isInXRange && isInYRange)
        {
            // ���S�ɉ�ʓ��Ȃ��\���̂܂�
            return;
        }

        // --- ���E�����̔��� ---
        float dot = Vector3.Dot(camRight, toTarget);

        if (isBehind)
        {
            // �w��ɂ��� �� ���E�����͔��]�i�J�������猩�����Ε����j
            if (dot < 0)
            {
                rightArrow.enabled = true;
            }
            else
            {
                leftArrow.enabled = true;
            }
        }
        else
        {
            // ���ʂ�����ʊO
            if (!isInXRange)
            {
                if (dot < 0)
                {
                    leftArrow.enabled = true;
                }
                else
                {
                    rightArrow.enabled = true;
                }
            }
            else if (!isInYRange && viewportPos.y > 1.0f)
            {
                upArrow.enabled = true;
            }
        }
    }
}

