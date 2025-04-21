//====================================================
// �X�N���v�g���FPlayModeManager
// �쐬�ҁF����
// ���e�F�v���C���[�h���Ǘ�����N���X
// �ŏI�X�V���F04/21
// 
// [Log]
// 04/21 ���� �X�N���v�g�쐬 
// 04/21 ���� CPU�̃C���v�b�g�����؂�ւ��鏈����ǉ�
// 
//====================================================
using UnityEngine;

//***** �}���`�v���C�Ή��菇 *****//
// 1. PlayModeManager�v���n�u���q�G�����L�[�ɓ����
// 2. 2�ڂ̃v���C���[�I�u�W�F�N�g(�܂���CPU1)�ƃJ�������쐬����
// 3. PlayerCamera1�ɂ̓v���C���[1�̃J�������Z�b�g
// 4. PlayerCamera2�ɂ̓v���C���[2(�܂���CPU1)�̃J�������Z�b�g
// 5. Mode��Solo(1�l�v���C)�܂���TwoPlayer(2�l�v���C)�ɐݒ�
// 6. PlayerObject2��Player2���Z�b�g����

public class PlayModeManager : MonoBehaviour
{
    public enum PlayMode
    {
        Solo,       // �\���v���C
        TwoPlayer,  // 2�l�v���C
    }

    [Tooltip("�v���C���[2�̃I�u�W�F�N�g���A�^�b�`")]
    [SerializeField]
    private GameObject PlayerObject2;
    [Tooltip("�v���C���[1�̃J�����I�u�W�F�N�g���A�^�b�`")]
    [SerializeField]
    private GameObject PlayerCamera1;
    [Tooltip("�v���C���[2�̃J�����I�u�W�F�N�g���A�^�b�`")]
    [SerializeField]
    private GameObject PlayerCamera2;
    [Tooltip("�I�����[�h")]
    [SerializeField]
    private PlayMode Mode;

    void Start()
    {
        if (!PlayerObject2)
        {
            Debug.LogError("�v���C���[2���A�^�b�`����Ă܂���");
        }
        if (!PlayerCamera1)
        {
            Debug.LogError("�v���C���[�J����1���A�^�b�`����Ă܂���");
        }
        if (!PlayerCamera2)
        {
            Debug.LogError("�v���C���[�J����2���A�^�b�`����Ă܂���");
        }
       
        // PlayerCamera1��CameraComponent
        Camera Camera1 = PlayerCamera1.gameObject.GetComponent<Camera>();

        // PlayerCamera2��CameraComponent
        Camera Camera2 = PlayerCamera2.gameObject.GetComponent<Camera>();

        // Player2�̊e����nComponent
        LRMovePlayer LRMovePlayer2 = PlayerObject2.GetComponent<LRMovePlayer>();
        JumpPlayer JumpPlayer2 = PlayerObject2.GetComponent<JumpPlayer>();
        PlayerDeceleration DecelerationPlayer2 = PlayerObject2.GetComponent<PlayerDeceleration>();

        switch (Mode)
        {
            // �\����p���[�h(�r���[�|�[�g����ʑS�̂Ɋg��)
            case PlayMode.Solo:
                this.PlayerCamera2.SetActive(false); // PlayerCamera2�͔�A�N�e�B�u�ɂ��Ă���

                // �\���v���C�̏ꍇ�v���C���[2��CPU�����Ȃ̂�
                // ����֘A�̃A�N�e�B�u��Ԃ�false�ɂ��Ă���
                LRMovePlayer2.enabled = false;
                JumpPlayer2.enabled = false;
                DecelerationPlayer2.enabled = false;

                Camera1.rect = new Rect(0f, 0f, 1f, 1f); // ��ʑS��
                break;

            // 2�l�v���C��p���[�h(�r���[�|�[�g�����E�ɕ���)
            case PlayMode.TwoPlayer:
                this.PlayerCamera2.SetActive(true); // PlayerCamera2���A�N�e�B�u�ɂ���

                // �v���C���[2�̑���֘A�̃A�N�e�B�u��Ԃ�true�ɂ��Ă���
                LRMovePlayer2.enabled = true;
                JumpPlayer2.enabled = true;
                DecelerationPlayer2.enabled = true;

                Camera1.rect = new Rect(0f, 0f, 0.5f, 1f);   // ������
                Camera2.rect = new Rect(0.5f, 0f, 0.5f, 1f); // �E����
                break;

            default:
                break;
        }

        // �I�u�W�F�N�g�j��
        Destroy(gameObject);
    }

    
    void Update()
    {
        
    }
}
