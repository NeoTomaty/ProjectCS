//======================================================
// SelectPlayerAnimationController
// �쐬�ҁF�X�e
// �ŏI�X�V���F6/5
//
// [Log]6/5 �X�e �X�N���v�g�쐬
//======================================================

using UnityEngine;

public class SelectPlayerAnimationController : MonoBehaviour
{
    [Header("���f���؂�ւ�")]
    [SerializeField] private GameObject rotationModel;  // �ړ����̉�]���f��

    [SerializeField] private GameObject model;          // �Î~���̃��f��

    [Header("�ړ��̂������l")]
    [SerializeField] private float moveThreshold = 0.1f;

    [Header("���f���̉�]���x�{��")]
    [SerializeField] private float baseRotationSpeed = 5f;

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // �� ������Raw�łȂ��ʏ�ɕύX
        bool isMoving = Mathf.Abs(horizontal) > moveThreshold;

        // ���f���؂�ւ�
        rotationModel.SetActive(!isMoving);
        model.SetActive(isMoving);

        if (isMoving)
        {
            // ��]������Y�p�x�i�E�F180�x�A���F-180�x�j
            float targetYRotation = horizontal > 0 ? 180f : -180f;

            // �^�[�Q�b�g��]
            Quaternion targetRotation = Quaternion.Euler(0f, targetYRotation, 0f);

            // ��]���x�͈ړ����x�ɉ����ăX�P�[�����O
            float speedFactor = Mathf.Abs(horizontal);
            float dynamicRotationSpeed = baseRotationSpeed * speedFactor;

            // rotationModel ����]������
            rotationModel.transform.rotation = Quaternion.Lerp(
                rotationModel.transform.rotation,
                targetRotation,
                Time.deltaTime * dynamicRotationSpeed
            );
        }
    }
}