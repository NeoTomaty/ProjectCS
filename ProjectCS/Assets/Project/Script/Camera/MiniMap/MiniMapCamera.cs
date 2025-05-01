//====================================================
// �X�N���v�g���FMiniMapCamera
// �쐬�ҁF�|��
// ���e�F����̃I�u�W�F�N�g���~�j�}�b�v�ɏ�ɕ\��������
// �@�@�@�~�j�}�b�v�ƂȂ�J�����ɃA�^�b�`����
//
// [Log]
// 04/16 �|�� �X�N���v�g�쐬
// 04/26 �|�� �X�N���v�g�����{�I�Ɏd�l�ύX
// 
//====================================================
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    [Header("�\���Ώ�")]
    [SerializeField] private Transform target;                          // �Ǐ]�Ώ�
    [Header("�J�����̑��΋���")]
    [SerializeField] private Vector3 offset = new Vector3(0, 10, 0);    // �Ǐ]���̑��Έʒu
    [Header("�~�j�}�b�v��\������܂ł̍���")]
    [SerializeField] private float toggleHeight = 100;    // �J������\�������鍂��

    private Camera miniMapCamera;    // ���̃X�N���v�g���A�^�b�`����Ă���~�j�}�b�v�J����

    // ������
    void Start()
    {
        // �^�[�Q�b�g���A�^�b�`����Ă��Ȃ����
        if (target == null)
        {
            Debug.LogError("MiniMapCamera�FTarget�����ݒ�ł�");
            enabled = false;
            return;
        }

        // �~�j�}�b�v�ƂȂ�J�������Ȃ����
        miniMapCamera = GetComponent<Camera>();
        if (miniMapCamera == null)
        {
            Debug.LogError("MiniMapCamera�FCamera�R���|�[�l���g������܂���");
            enabled = false;
            return;
        }
    }

    // �Ō�ɌĂяo�����Update����
    void LateUpdate()
    {
        FollowTarget();
        ToggleMiniMap();
    }

    // �^�[�Q�b�g��Ǐ]����
    private void FollowTarget()
    {
        transform.position = target.position + offset;
        transform.rotation = Quaternion.Euler(0f, 90f, 0f); // �^������̊p�x
    }

    // �����ɉ����ă~�j�}�b�v��ON/OFF����
    private void ToggleMiniMap()
    {
        if (target.position.y > toggleHeight)
        {
            miniMapCamera.enabled = true;
        }
        else
        {
            miniMapCamera.enabled = false;
        }
    }
}