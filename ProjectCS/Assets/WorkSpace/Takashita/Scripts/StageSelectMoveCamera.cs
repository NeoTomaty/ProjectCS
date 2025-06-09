//====================================================
// �X�N���v�g���FStageSelectMoveCamera
// �쐬�ҁF����
// ���e�F�X�e�[�W�Z���N�g��ʂ̃J�����̈ړ�
// �ŏI�X�V���F05/29
// 
// [Log]
// 05/29 ���� �X�N���v�g�쐬
//====================================================
using UnityEngine;

public class StageSelectMoveCamera : MonoBehaviour
{
    public Transform PlayerObject;

    [Header("Offset & Rotation 1 (�ʏ펞)")]
    public Vector3 Offset1 = new Vector3(0, 40, -60);
    public Vector3 RotationEuler1 = new Vector3(20f, 0f, 0f);  // �v���C���[�������]�Ȃ�

    [Header("Offset & Rotation 2 (�ؑ֎�)")]
    public Vector3 Offset2 = new Vector3(-5, 10, -14);
    public Vector3 RotationEuler2 = new Vector3(0f, 45f, 0f);

    [Header("�ݒ�")]
    public float LerpSpeed = 3.0f;

    private float t = 0f;
    private bool IsSwitched = false;
    private bool IsInterpolating = false;

    void Update()
    {
        // ��ԌW���X�V
        t = Mathf.MoveTowards(t, IsSwitched ? 1f : 0f, Time.deltaTime * LerpSpeed);

        // �I�t�Z�b�g�⊮�i�ʒu�j
        Vector3 offset = Vector3.Lerp(Offset1, Offset2, t);
        transform.position = PlayerObject.position + offset;

        // ��]�⊮
        Quaternion rotation1 = Quaternion.Euler(RotationEuler1);
        Quaternion rotation2 = Quaternion.Euler(RotationEuler2);
        transform.rotation = Quaternion.Slerp(rotation1, rotation2, t);

        if (IsInterpolating && (t == 0f || t == 1f))
        {
            IsInterpolating = false;
        }
    }

    public bool GetIsInterpolating()
    {
        return IsInterpolating;
    }

    public void SetIsSwitched(bool isSwitched)
    {
        IsSwitched = isSwitched;
        IsInterpolating = true;
    }

    public bool GetIsSwitched()
    {
        return IsSwitched;
    }
}
