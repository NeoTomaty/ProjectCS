//======================================================
// [ChangeCameraFOV]
// �쐬�ҁF�r��C
// �ŏI�X�V���F05/02
// 
// [Log]
// 04/12�@�r��@�v���C���[�̑��x�ɉ����ăJ������FOV���ω�����悤�Ɏ���
// 05/02�@�����@����p�x�𑬓x�����ŕω��ł���悤�ɏC��
//======================================================

using UnityEngine;

// �v���C���[��ǐՂ���J�����ɃA�^�b�`
public class ChangeCameraFOV : MonoBehaviour
{

    [SerializeField] private Camera Camera;
    [SerializeField] private PlayerSpeedManager PlayerSpeedManager;

    // FOV�̐ݒ�
    [Header("����p�x�̐ݒ�")]
    [Tooltip("���x�ŏ����̎���p�x")]
    [SerializeField] private float MinFOV = 60.0f;  // �ŏ�
    [Tooltip("���x�ő厞�̎���p�x")]
    [SerializeField] private float MaxFOV = 110.0f; // �ő�
    [Tooltip("����p�x�̕⊮���x")]
    [SerializeField] private float FOVLerpSpeed = 1.0f; // FOV�̕ω����x

    void Start()
    {
        Camera.fieldOfView = MinFOV; // �����l��ݒ�
    }

    void Update()
    {
        if (Camera == null || PlayerSpeedManager == null) return;

        // FOV�̌v�Z
        float FOV = Mathf.Lerp(MinFOV, MaxFOV, PlayerSpeedManager.GetSpeedRatio());

        // ����p�̓K�p
        Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, FOV, Time.deltaTime * FOVLerpSpeed);
    }
}
