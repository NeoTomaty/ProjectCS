//======================================================
// [GenerateSpark]
// �쐬�ҁF�r��C
// �ŏI�X�V���F04/18
// 
// [Log]
// 04/13�@�r��@�Փˎ��ɉΉԂ𐶐�����悤�Ɏ���
// 04/13�@�r��@�v���C���[�̑��x��臒l���ׂ������ɐF���ς��悤�Ɏ���
// 04/16�@�r��@�p�[�e�B�N���̃p�����[�^��ݒ�ł���悤�ɕύX
// 04/17�@�r��@�p�[�e�B�N���̌p�����Ԃ̎w���p�~
// 04/18�@�r��@���n�Ƃ̋�ʕ��@��ύX
//======================================================

using UnityEngine;

// �v���C���[�ɃA�^�b�`
public class GenerateSpark : MonoBehaviour
{

    [SerializeField] private GameObject SparkPrefab;
    [SerializeField] private PlayerSpeedManager PlayerSpeedManager;

    // �p�[�e�B�N���̐ݒ�
    [SerializeField] private float ParticleSpeed = 30.0f;       // �p�[�e�B�N���̑��x
    [SerializeField] private float ParticleSize = 0.2f;         // �p�[�e�B�N���̃T�C�Y

    // �F�̐ݒ�
    [SerializeField] private Color LowSpeedColor = Color.blue;      // �ᑬ��
    [SerializeField] private Color MiddleSpeedColor = Color.yellow; // ������
    [SerializeField] private Color HighSpeedColor = Color.red;      // ������

    // ���x��臒l
    [SerializeField] private float LowToMiddleSpeedThreshold = 200.0f;  // �ᑬ���璆��
    [SerializeField] private float MiddleToHighSpeedThreshold = 400.0f; // �������獂��

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (SparkPrefab == null) return;

        // �Փ˒n�_�̍��W�Ɩ@�����擾
        ContactPoint ContactPoint = collision.contacts[0];
        Vector3 HitPosition = ContactPoint.point;
        Vector3 HitNormal = ContactPoint.normal;

        // �Փ˒n�_�̖@����������Ȃ璅�n�����ɂ���
        if (HitNormal == Vector3.up) return;

        // �ΉԂ𐶐�
        GameObject SparkEffect = Instantiate(SparkPrefab, HitPosition, Quaternion.LookRotation(HitNormal));

        // �v���C���[�̑��x���擾
        float PlayerSpeed = PlayerSpeedManager.GetPlayerSpeed;

        // �F�̐ݒ�
        Color SparkColor;
        if (PlayerSpeed < LowToMiddleSpeedThreshold)
        {
            SparkColor = LowSpeedColor; // �ᑬ���̐F
        }
        else if (PlayerSpeed < MiddleToHighSpeedThreshold)
        {
            SparkColor = MiddleSpeedColor; // �������̐F
        }
        else
        {
            SparkColor = HighSpeedColor; // �������̐F
        }

        // �F��K�p
        ParticleSystem.MainModule EffectMainModule = SparkEffect.GetComponent<ParticleSystem>().main;
        EffectMainModule.startSpeed = ParticleSpeed;
        EffectMainModule.startSize = ParticleSize;
        EffectMainModule.startColor = SparkColor;

        // ��莞�Ԍ�ɉΉԂ�����
        Destroy(SparkEffect, 1.0f);
    }
}
