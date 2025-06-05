//====================================================
// �X�N���v�g���FSpeedEffectArea
// �쐬�ҁF����
// ���e�F�v���C���[���ꎞ�I�Ɍ�������G���A�𐧌䂷��N���X
// �ŏI�X�V���F06/04
// 
// [Log]
// 05/08 ���� �X�N���v�g�쐬 
// 06/04 ���� ����SE����
// 06/05 �|�� ���������̎d�l��ύX�i�^�C�vB�ǉ��j
// 06/05 �|�� ���������̎d�l��ǉ��i�^�C�vC�ǉ��j
// 06/05 �|�� SpeedEffectArea�ɉ���
//====================================================
using UnityEngine;
// �����̃X�N���v�g���A�^�b�`���Ă��錸���G���A�̓v���n�u�ō���Ă܂�

public enum SpeedEffectType
{
    TypeA, // �ꎞ���� �i������ɑ��x�͖߂�j
    TypeB, // �p������ �i������ɑ��x�͖߂�Ȃ��j
    TypeC  // ����     �i������ɑ��x�͖߂�Ȃ��j
}

public class SpeedEffectArea : MonoBehaviour
{
    [SerializeField] private GameObject Player; // �v���C���[�I�u�W�F�N�g

    [Header("�������i0.0�`1.0�Őݒ�j")]
    [SerializeField] private float DecelerationRatio = 0.1f;
    
    //�����̊���ő呬�x�ɂ��邩�ǂ���
    [Header("true:�ő呬�x���猸���l������ false:���݂̑��x���猸���l������")]
    [SerializeField] private bool IsDecelerationBasedOnMaxSpeed = true;

    [Header("���x��⊮���鎞�ԁi�b�j")]
    [SerializeField] private float InterpolationDuration = 1.0f;

    //�������ɖ炷�T�E���h�G�t�F�N�g
    [Header("SE�ݒ�")]
    [Tooltip("�������ɍĐ�����SE")]
    [SerializeField] private AudioClip DecelerationSE;

    [Header("�g�p����������^�C�v A:�ꎞ���� B:���� C:����")]
    [SerializeField] private SpeedEffectType effectType = SpeedEffectType.TypeA;

    [Header("�ǂꂭ�炢�̊Ԋu�ŉ���������̂�")]
    [SerializeField] private float TickInterval = 0.5f;         // B/C�p�F����I�ɉ���������Ԋu

    [Header("�ǂꂭ�炢����������̂�")]
    [SerializeField] private float SpeedAmount = 0.5f;   // B/C�p�F1�񂠂���̕ω���

    private float tickTimer = 0f;           // B/C�p�F�o�ߎ��ԋL�^
    private bool isPlayerInside = false;    // B/C�p�F�G���A������

    //SE�Đ��p��AudioSource
    private AudioSource audioSource;

    private PlayerSpeedManager SpeedManager; // �v���C���[��PlayerSpeedManager�R���|�[�l���g
    private float TempPlayerSpeed = 0f;      // �����O�̑��x
    private float StartSpeed = 0f;           // �⊮�J�n���̑��x
    private float TargetSpeed = 0f;          // �⊮�I�����̖ڕW���x
    private float InterpolationTimer = 0f;   // �⊮���̌o�ߎ���
    private bool IsInterpolating = false;    // �⊮�����ǂ���

    void Start()
    {
        //�v���C���[���ݒ肳��Ă��Ȃ��Ƃ��̓G���[���o��
        if (!Player)
        {
            Debug.LogError("�v���C���[�I�u�W�F�N�g���ݒ肳��Ă��܂���");
            return;
        }

        //�v���C���[�̑��x�Ǘ��R���|�[�l���g���擾
        SpeedManager = Player.GetComponent<PlayerSpeedManager>();

        //AudioSource�����̃I�u�W�F�N�g�ɒǉ����ASE�Đ��p�ɐݒ�
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {

        // A�^�C�v�̕⊮����
        if (effectType == SpeedEffectType.TypeA && IsInterpolating)
        {
            InterpolationTimer += Time.deltaTime;
            float t = Mathf.Clamp01(InterpolationTimer / InterpolationDuration);
            float newSpeed = Mathf.Lerp(StartSpeed, TargetSpeed, t);
            SpeedManager.SetSpeed(newSpeed);

            if (t >= 1.0f)
                IsInterpolating = false;
        }

        // B/C�^�C�v��Tick����
        if ((effectType == SpeedEffectType.TypeB || effectType == SpeedEffectType.TypeC) && isPlayerInside)
        {
            tickTimer += Time.deltaTime;
            if (tickTimer >= TickInterval)
            {
                tickTimer = 0f;

                float currentSpeed = SpeedManager.GetPlayerSpeed;
                float newSpeed = currentSpeed;

                if (effectType == SpeedEffectType.TypeB)
                    newSpeed = Mathf.Max(SpeedManager.GetMinSpeed(), currentSpeed - SpeedAmount);
                else if (effectType == SpeedEffectType.TypeC)
                    newSpeed = Mathf.Min(SpeedManager.GetMaxSpeed(), currentSpeed + SpeedAmount);

                SpeedManager.SetSpeed(newSpeed);
            }
        }
    }


    // �v���C���[���G���A�ɓ������Ƃ��̏���
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            Debug.Log("�G���A�ɓ���܂���");

            // SE���ݒ肳��Ă���΍Đ�
            if (DecelerationSE != null) audioSource.PlayOneShot(DecelerationSE);

            // �v���C���[���G���A���ɂ����Ԃɐݒ�iB/C�p�̔���p�j
            isPlayerInside = true;

            // A�^�C�v
            if (effectType == SpeedEffectType.TypeA)
            {
                // ���łɕ⊮���Ȃ炻�̖ڕW���x���ꎞ�ۑ��A����ȊO�͌��݂̑��x��ۑ�
                TempPlayerSpeed = IsInterpolating ? TargetSpeed : SpeedManager.GetPlayerSpeed;

                // �⊮�̊J�n���x���L�^
                StartSpeed = TempPlayerSpeed;

                // �����̊���x������i�ő呬�x or ���ݑ��x�j
                float baseSpeed = IsDecelerationBasedOnMaxSpeed ? SpeedManager.GetMaxSpeed() : TempPlayerSpeed;

                // �����ʂ��Z�o
                float decelerationValue = baseSpeed * DecelerationRatio;

                // �ŏ����x�������Ȃ��悤�ɒ���
                TargetSpeed = Mathf.Max(SpeedManager.GetMinSpeed(), TempPlayerSpeed - decelerationValue);

                // �⊮�����̏�����
                InterpolationTimer = 0f;
                IsInterpolating = true;
            }
            else
            {
                // B/C�^�C�v�p�F�^�C�}�[������
                tickTimer = 0f;
            }
        }
    }

    // �v���C���[���G���A����o���Ƃ��̏���
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            Debug.Log("�G���A����o�܂���");

            // �v���C���[���G���A�O�ɏo����Ԃɂ���iB/C�p�̔���p�j
            isPlayerInside = false;

            // A�^�C�v
            if (effectType == SpeedEffectType.TypeA)
            {
                // �⊮�̊J�n���x�����݂̑��x�ɐݒ�
                StartSpeed = SpeedManager.GetPlayerSpeed;

                // �⊮�̖ڕW���x���G���A�N���O�̑��x�ɐݒ�
                TargetSpeed = TempPlayerSpeed;

                // �⊮�����̏�����
                InterpolationTimer = 0f;
                IsInterpolating = true;
            }
        }
    }

}
