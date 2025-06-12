//====================================================
// �X�N���v�g���FBezierMover
// �쐬�ҁF����
// ���e�F�Ȑ���Ƀv���C���[�𓮂�����֐�
// �ŏI�X�V���F06/01
// 
// [Log]
// 06/01 ���� �X�N���v�g�쐬
//====================================================
using UnityEngine;

// �x�W�F�Ȑ��ɉ����ăI�u�W�F�N�g���ړ��E��]������R���|�[�l���g
public class BezierMover : MonoBehaviour
{
    // �x�W�F�Ȑ����v�Z����X�N���v�g�i�K�{�j
    [SerializeField] private CubicBezierCurve curve;

    // �ړ��ɂ����鍇�v���ԁi�b�j
    [Range(0.1f, 10f)]
    [SerializeField] private float moveDuration = 3f;

    // ��]�����錩���ڃ��f���i���̃I�u�W�F�N�g����]�j
    [SerializeField] private Transform RotatingModel;

    // ���a�i�]����p�x�̌v�Z�Ɏg�p�j
    [SerializeField] private float radius = 0.01f;

    // �ړ������i�t�Đ����ǂ����j
    private bool IsReverse = false;

    // �ړ��p�̃^�C�}�[
    private float timer = 0f;

    // ���݈ړ������ǂ����̃t���O
    private bool isMoving = false;

    // �O�t���[���̈ʒu�i�i�s���������߂邽�߂Ɏg�p�j
    private Vector3 previousPosition;

    void Update()
    {
        // �ړ����L���łȂ���Ή������Ȃ�
        if (!isMoving || curve == null) return;

        // ���Ԃ�i�߂�i�t�Đ����͌��Z�j
        timer += IsReverse ? -Time.deltaTime : Time.deltaTime;

        // ���Ԃ� 0�`1 �ɐ��K��
        float t = Mathf.Clamp01(timer / moveDuration);

        // ���݈ʒu���x�W�F�Ȑ��ォ��擾���Ĉړ�
        Vector3 currentPosition = curve.CalculateBezierPoint(t);
        transform.position = currentPosition;

        // �i�s�����̃x�N�g���i�O�t���[���Ƃ̍����j
        Vector3 delta = currentPosition - previousPosition;
        float distance = delta.magnitude;

        // �����̍X�V�i�i�s������Y������ɉ�]�j
        if (delta != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(delta.normalized, Vector3.up);
            RotatingModel.rotation = lookRotation;
        }

        // ��]�^���̉��o�i�n�ʂ�]����悤�ɉ�]�j
        if (distance > 0f && radius > 0f)
        {
            // �i�s�����Ə�����̊O�ςŉ�]���𐶐�
            Vector3 rotationAxis = Vector3.Cross(delta.normalized, Vector3.up);

            // ��]�p�x�i���� �� ���a �� ���W�A�� �� �x�ɕϊ��j
            float angleDeg = Mathf.Rad2Deg * (distance / radius);

            // ���f������]
            RotatingModel.Rotate(rotationAxis, angleDeg, Space.World);
        }

        // ���t���[���̍����v�Z�̂��߂Ɍ��݈ʒu��ۑ�
        previousPosition = currentPosition;

        // �ړ���������i���ԃI�[�o�[�܂��͋t�Đ��Ŏ��ԃ[���j
        if ((!IsReverse && timer >= moveDuration) || (IsReverse && timer <= 0f))
        {
            isMoving = false;
        }
    }

    // �x�W�F�Ȑ��ړ����J�n����i�J�n�����ƑΏۋȐ����w��j
    /// <param name="isReverse">�t�Đ����邩</param>
    /// <param name="cubicBezierCurve">�ړ��Ώۂ̃x�W�F�Ȑ�</param>
    public void StartMove(bool isReverse, CubicBezierCurve cubicBezierCurve)
    {
        IsReverse = isReverse;
        curve = cubicBezierCurve;
        isMoving = true;

        // ����ړ��Ȃ�^�C�}�[���J�n�_�܂��͏I���_�ɐݒ�
        if (timer <= 0f || timer >= moveDuration)
        {
            timer = isReverse ? moveDuration : 0f;
        }
    }

    // �����I�Ɉʒu�����ݒ肵�����Ƃ��Ɏg�p
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    // �ړ������ǂ������擾
    public bool GetIsMoving()
    {
        return isMoving;
    }
}
