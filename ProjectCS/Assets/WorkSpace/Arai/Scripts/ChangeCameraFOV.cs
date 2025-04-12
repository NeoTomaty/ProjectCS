//======================================================
// [ChangeCameraFOV]
// �쐬�ҁF�r��C
// �ŏI�X�V���F04/12
// 
// [Log]
// 04/12�@�r��@�v���C���[�̑��x�ɉ����ăJ������FOV���ω�����悤�Ɏ���
//======================================================

using UnityEngine;

public class ChangeCameraFOV : MonoBehaviour
{
    // �v���C���[��ǐՂ���J�����ɃA�^�b�`

    [SerializeField] private Camera Camera;
    [SerializeField] private PlayerSpeedManager PlayerSpeedManager;

    // FOV�̐ݒ�
    [SerializeField] private float MinFOV = 90.0f; // �ŏ�
    [SerializeField] private float MaxFOV = 110.0f; // �ő�
    [SerializeField] private float FOVLerpSpeed = 1.0f; // FOV�̕ω����x

    // ���x�̐ݒ�
    [SerializeField] private float MinSpeed = 120.0f; // �ŏ����x
    [SerializeField] private float MaxSpeed = 500.0f; // �ő呬�x

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Camera.fieldOfView = MinFOV; // �����l��ݒ�
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera == null || PlayerSpeedManager == null) return;

        // �v���C���[�̑��x���擾
        float PlayerSpeed = PlayerSpeedManager.GetPlayerSpeed;

        // FOV�̌v�Z
        float FOV = Mathf.Lerp(MinFOV, MaxFOV, (PlayerSpeed - MinSpeed) / (MaxSpeed - MinSpeed));

        Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, FOV, Time.deltaTime * FOVLerpSpeed);
    }
}
