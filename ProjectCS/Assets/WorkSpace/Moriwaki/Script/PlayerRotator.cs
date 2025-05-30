//======================================================
// [PlayerRotator]
// �쐬�ҁF�X�e
// �ŏI�X�V���F05/29
//
// [Log]
// 05/29�@�X�e ��]
//======================================================

using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    [SerializeField] private PlayerSpeedManager speedManager; // PlayerSpeedManager�ւ̎Q�Ɓi�C���X�y�N�^�[�Őݒ�j
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // ��]���i�f�t�H���g��Y���j
    [SerializeField] private float rotationMultiplier = 1.0f; // ��]�X�s�[�h�̔{��

    private void Update()
    {
        if (speedManager == null) return;

        // PlayerSpeed���擾
        float speed = speedManager.GetPlayerSpeed;

        // ��]�ʂ��v�Z
        float rotationAmount = speed * rotationMultiplier * Time.deltaTime;

        // ��]
        transform.Rotate(rotationAxis, rotationAmount);
    }
}