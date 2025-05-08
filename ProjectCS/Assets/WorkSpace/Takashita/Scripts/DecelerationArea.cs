//====================================================
// �X�N���v�g���FDecelerationArea
// �쐬�ҁF����
// ���e�F�v���C���[���ꎞ�I�Ɍ�������G���A�𐧌䂷��N���X
// �ŏI�X�V���F05/08
// 
// [Log]
// 05/08 ���� �X�N���v�g�쐬 
// 
//====================================================
using UnityEngine;
// �����̃X�N���v�g���A�^�b�`���Ă��錸���G���A�̓v���n�u�ō���Ă܂�

public class DecelerationArea : MonoBehaviour
{
    [SerializeField] private GameObject Player; // �v���C���[�I�u�W�F�N�g

    [Tooltip("�������i0.0�`1.0�Őݒ�j")]
    [SerializeField] private float DecelerationRatio = 0.1f;

    [Tooltip("true:�ő呬�x���猸���l������ false:���݂̑��x���猸���l������")]
    [SerializeField] private bool IsDecelerationBasedOnMaxSpeed = true;

    [Tooltip("���x��⊮���鎞�ԁi�b�j")]
    [SerializeField] private float InterpolationDuration = 1.0f;

    private PlayerSpeedManager SpeedManager; // �v���C���[��PlayerSpeedManager�R���|�[�l���g
    private float TempPlayerSpeed = 0f;      // �����O�̑��x
    private float StartSpeed = 0f;           // �⊮�J�n���̑��x
    private float TargetSpeed = 0f;          // �⊮�I�����̖ڕW���x
    private float InterpolationTimer = 0f;   // �⊮���̌o�ߎ���
    private bool IsInterpolating = false;    // �⊮�����ǂ���

    void Start()
    {
        if (!Player)
        {
            Debug.LogError("�v���C���[�I�u�W�F�N�g���ݒ肳��Ă��܂���");
            return;
        }

        SpeedManager = Player.GetComponent<PlayerSpeedManager>();
    }

    void Update()
    {
        // �⊮����
        if (IsInterpolating)
        {
            InterpolationTimer += Time.deltaTime;

            // �⊮�̐i�s�x���N�����v
            float t = Mathf.Clamp01(InterpolationTimer / InterpolationDuration);

            // ���ɐݒ肷�鑬�x
            float newSpeed = Mathf.Lerp(StartSpeed, TargetSpeed, t);

            // ���x�𔽉f
            SpeedManager.SetSpeed(newSpeed);

            if (t >= 1.0f)
            {
                IsInterpolating = false; // �⊮�I��
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            Debug.Log("�����G���A�ɓ���܂���");

            // �⊮���Ȃ�ڕW���x���A�����łȂ���Ό��݂̃v���C���[���x���ꎞ�ۑ�
            TempPlayerSpeed = IsInterpolating ? TargetSpeed : SpeedManager.GetPlayerSpeed;

            // �J�n���x
            StartSpeed = TempPlayerSpeed;

            // �����l�̊���x������itrue�Ȃ�ő呬�x�Afalse�Ȃ猻�݂̑��x�j
            float baseSpeed = IsDecelerationBasedOnMaxSpeed ? SpeedManager.GetMaxSpeed() : TempPlayerSpeed;

            // �����l�v�Z
            float decelerationValue = baseSpeed * DecelerationRatio;

            // �ڕW���x���ŏ����x�������Ȃ��悤�ɒ���
            TargetSpeed = Mathf.Max(SpeedManager.GetMinSpeed(), TempPlayerSpeed - decelerationValue);

            InterpolationTimer = 0f;
            IsInterpolating = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            Debug.Log("�����G���A����o�܂���");

            // �J�n���x
            StartSpeed = SpeedManager.GetPlayerSpeed;

            // �G���A�ɓ��������̑��x��ݒ�
            TargetSpeed = TempPlayerSpeed;            

            InterpolationTimer = 0f;
            IsInterpolating = true;
        }
    }
}
