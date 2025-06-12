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

// �X�e�[�W�I����ʂɂ����āA�J������2�̎��_�Ԃŕ�Ԉړ��E��]�����鐧��N���X
public class StageSelectMoveCamera : MonoBehaviour
{
    // �J�������Ǐ]����Ώہi�v���C���[�Ȃǁj
    public Transform PlayerObject;

    [Header("Offset & Rotation 1 (�ʏ펞)")]
    // �ʏ펞�̃J�����ʒu�I�t�Z�b�g�i�v���C���[����̑��Έʒu�j
    public Vector3 Offset1 = new Vector3(0, 40, -60);
    // �ʏ펞�̃J������]�i�p�x�j
    public Vector3 RotationEuler1 = new Vector3(20f, 0f, 0f);

    [Header("Offset & Rotation 2 (�ؑ֎�)")]
    // �ؑ֎��̃J�����ʒu�I�t�Z�b�g�i��F�I���X�e�[�W�̋߂��փY�[���j
    public Vector3 Offset2 = new Vector3(-5, 10, -14);
    // �ؑ֎��̃J������]�i�X�e�[�W���΂߂Ɍ���Ȃǁj
    public Vector3 RotationEuler2 = new Vector3(0f, 45f, 0f);

    [Header("�ݒ�")]
    // ��ԃX�s�[�h�i�傫���قǑ�����Ԃ����j
    public float LerpSpeed = 3.0f;

    // ��ԌW���i0=�ʏ�, 1=�ؑցj
    private float t = 0f;

    // ���݂ǂ���̏�ԂɌ������Ă��邩�ifalse=�ʏ�, true=�ؑցj
    private bool IsSwitched = false;

    // ���ݕ�Ԓ����ǂ���
    private bool IsInterpolating = false;

    void Update()
    {
        // t ���ԁiIsSwitched �ɉ����� 0��1 or 1��0�j
        t = Mathf.MoveTowards(t, IsSwitched ? 1f : 0f, Time.deltaTime * LerpSpeed);

        // �J�����ʒu�̕�ԁi�v���C���[�ʒu + �I�t�Z�b�g�j
        Vector3 offset = Vector3.Lerp(Offset1, Offset2, t);
        transform.position = PlayerObject.position + offset;

        // �J������]�̕��
        Quaternion rotation1 = Quaternion.Euler(RotationEuler1);
        Quaternion rotation2 = Quaternion.Euler(RotationEuler2);
        transform.rotation = Quaternion.Slerp(rotation1, rotation2, t);

        // ��Ԃ�����������t���O�����낷
        if (IsInterpolating && (t == 0f || t == 1f))
        {
            IsInterpolating = false;
        }
    }

    // ���݃J��������Ԓ����ǂ������擾����
    public bool GetIsInterpolating()
    {
        return IsInterpolating;
    }

    // �J������Ԃ�؂�ւ���itrue:�ؑ֏�ԂցAfalse:�ʏ��Ԃցj
    public void SetIsSwitched(bool isSwitched)
    {
        IsSwitched = isSwitched;
        IsInterpolating = true;
    }

    // ���݂̏�ԁi�ʏ� or �ؑցj���擾����
    public bool GetIsSwitched()
    {
        return IsSwitched;
    }
}