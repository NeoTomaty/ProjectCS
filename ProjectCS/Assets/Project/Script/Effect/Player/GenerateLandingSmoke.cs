//======================================================
// [GenerateLandingSmoke]
// �쐬�ҁF�r��C
// �ŏI�X�V���F04/18
// 
// [Log]
// 04/17�@�r��@���n���ɃG�t�F�N�g�𐶐�����悤�Ɏ���
// 04/18�@�r��@���R�Ȍ������ɂȂ�悤�ɃG�t�F�N�g�̐����ʒu��␳����悤�Ɏ���
//======================================================
using UnityEngine;

public class GenerateLandingSmoke : MonoBehaviour
{
    [SerializeField] private GameObject LandingSmokePrefab;
    [SerializeField] private MovePlayer MovePlayer; // �v���C���[�̈ړ��X�N���v�g
    [SerializeField] private PlayerSpeedManager PlayerSpeedManager; // �v���C���[�̑��x�Ǘ��X�N���v�g

    [SerializeField] private float EffectSize = 1.0f; // �G�t�F�N�g�̃T�C�Y

    [Tooltip("�G�t�F�N�g�����ʒu�̕␳�{��")]
    [SerializeField] private float EffectPositionCorrection = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnCollisionEnter(Collision collision)
    {
        if (LandingSmokePrefab == null) return;

        // �Փ˒n�_�̍��W�Ɩ@�����擾
        ContactPoint ContactPoint = collision.contacts[0];
        Vector3 HitPosition = ContactPoint.point;
        Vector3 HitNormal = ContactPoint.normal;

        // �Փ˒n�_�̖@����������Ȃ璅�n�����ɂ���
        if (HitNormal == Vector3.up)
        {
            // ���R�Ȍ������ɂȂ�悤�ɐ����n�_��␳
            float CorrectionAmount = PlayerSpeedManager.GetPlayerSpeed * EffectPositionCorrection; // �v���C���[�̑��x�Ɋ�Â��␳��
            Vector3 SpawnPosition = HitPosition + (MovePlayer.GetMoveDirection * CorrectionAmount);

            // �G�t�F�N�g�𐶐�
            GameObject LandingSmokeEffect = Instantiate(LandingSmokePrefab, SpawnPosition, Quaternion.LookRotation(HitNormal));

            // �G�t�F�N�g�̃T�C�Y��ݒ�
            LandingSmokeEffect.transform.localScale = new Vector3(EffectSize, EffectSize, EffectSize);

            Destroy(LandingSmokeEffect, 2.0f); // ��莞�Ԍ�ɏ���
        }
    }
}
