//====================================================
// �X�N���v�g���FPlayModeManager
// �쐬�ҁF����
// ���e�F�v���C���[�h���Ǘ�����N���X
// �ŏI�X�V���F04/21
// 
// [Log]
// 04/21 ���� �X�N���v�g�쐬 
// 04/21 ���� CPU�̃C���v�b�g�����؂�ւ��鏈����ǉ�
// 04/23 ���� �R���g���[���[�̊��蓖�ď�����ǉ�
//====================================================
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

//***** �}���`�v���C�Ή��菇 *****//
//  1. PlayModeManager�v���n�u���q�G�����L�[�ɓ����
//  2. 2�ڂ̃v���C���[�I�u�W�F�N�g(�܂���CPU)�ƃv���C���[2�̃J�������쐬����
//  3. Player1��Player2�i�܂���CPU�j�̃I�u�W�F�N�g��PlayerObject1��PlayerObject2�ɂ��ꂼ��ݒ肷��
//  4. PlayerCamera1��PlayerCamera2�̃I�u�W�F�N�g��PlayerCamera1��PlayerCamera2�ɂ��ꂼ��ݒ肷��
//  5. Mode��Solo(1�l�v���C)�܂���TwoPlayer(2�l�v���C)�ɐݒ�
//  6. Player1�I�u�W�F�N�g��Player2�I�u�W�F�N�g��PlayerInputComponent��ǉ��i�v���C���[�I�u�W�F�N�g���̐ݒ�j
//  7. PlayerInput��Actions�ɁuPlayerInputActions�v��ݒ�i�v���C���[�I�u�W�F�N�g���̐ݒ�j
//  8. PlayerInput��Behavior�ɁuInvokeUnityEvents�v��ݒ�i�v���C���[�I�u�W�F�N�g���̐ݒ�j
//  9. Player1Input��Player2Input���ꂼ��̃v���C���[��PlayerInput��ݒ�

public class PlayModeManager : MonoBehaviour
{
    public enum PlayMode
    {
        Solo,       // �\���v���C
        TwoPlayer,  // 2�l�v���C
    }

    [Tooltip("�v���C���[1�̃I�u�W�F�N�g���A�^�b�`")]
    [SerializeField]
    private GameObject PlayerObject1;
    [Tooltip("�v���C���[2�܂���CPU�̃I�u�W�F�N�g���A�^�b�`")]
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
        if (!PlayerObject1)
        {
            Debug.LogError("�v���C���[1���A�^�b�`����Ă܂���");
        }
        if (!PlayerObject2)
        {
            Debug.LogError("�v���C���[2�܂���CPU���A�^�b�`����Ă܂���");
        }
        if (!PlayerCamera1)
        {
            Debug.LogError("�v���C���[�J����1���A�^�b�`����Ă܂���");
        }
        if (!PlayerCamera2)
        {
            Debug.LogError("�v���C���[�J����2���A�^�b�`����Ă܂���");
        }

        // PlayerInputComponent�����ꂼ��擾
        PlayerInput Player1Input = PlayerObject1.GetComponent<PlayerInput>();
        PlayerInput Player2Input = PlayerObject2.GetComponent<PlayerInput>();

        // �R���g���[���[�擾
        var gamepads = Gamepad.all;

        // �ڑ��R���g���[���[0��A����2�l���[�h�̏ꍇ
        // �����I�Ƀ\�����[�h�ɐ؂�ւ��i���j
        if (gamepads.Count == 0 && Mode == PlayMode.TwoPlayer)
        {
            Mode = PlayMode.Solo;
            Debug.Log("�R���g���[���[�̐ڑ����m�F����Ȃ����߁A�\�����[�h�ɐ؂�ւ��܂���");
        }

        // �ڑ��R���g���[���[1��ȏ�A���\�����[�h
        if(gamepads.Count >= 1 && Mode == PlayMode.Solo)
        {
            // �R���g���[���[����ɐݒ�
            AssignInputToPlayer(Player1Input, gamepads[0], "Gamepad", "�v���C���[(�\��)");
        }
        // �ڑ��R���g���[���[0��A���\�����[�h
        else if (gamepads.Count == 0 && Mode == PlayMode.Solo)
        {
            // �L�[�{�[�h����ɐݒ�
            AssignInputToPlayer(Player1Input, Keyboard.current, "Keyboard", "�v���C���[(�\��)");
        }

        // �ڑ��R���g���[���[2��ȏ�A����2�l���[�h
        if (gamepads.Count >= 2 && Mode == PlayMode.TwoPlayer)
        {
            // �v���C���[1�ƃv���C���[2�Ƃ��ɃR���g���[���[����
            AssignInputToPlayer(Player1Input, gamepads[0], "Gamepad", "�v���C���[1");
            AssignInputToPlayer(Player2Input, gamepads[1], "Gamepad", "�v���C���[2");
        }
        // �ڑ��R���g���[���[1��A����2�l���[�h
        else if (gamepads.Count == 1 && Mode == PlayMode.TwoPlayer)
        {
            // �v���C���[1�̓R���g���[���[����A�v���C���[2�̓L�[�{�[�h����
            AssignInputToPlayer(Player1Input, gamepads[0], "Gamepad", "�v���C���[1");
            AssignInputToPlayer(Player2Input, Keyboard.current, "Keyboard", "�v���C���[2");
        }
       
        // PlayerCamera1��CameraComponent
        Camera Camera1 = PlayerCamera1.gameObject.GetComponent<Camera>();

        // PlayerCamera2��CameraComponent
        Camera Camera2 = PlayerCamera2.gameObject.GetComponent<Camera>();

        // Player2�̊e����nComponent
        LRMovePlayer LRMovePlayer2 = PlayerObject2.GetComponent<LRMovePlayer>();
        JumpPlayer JumpPlayer2 = PlayerObject2.GetComponent<JumpPlayer>();
        PlayerDeceleration DecelerationPlayer2 = PlayerObject2.GetComponent<PlayerDeceleration>();
        CameraFunction CameraFunction2 = PlayerCamera2.GetComponent<CameraFunction>();

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
                CameraFunction2.enabled = false;

                Camera1.rect = new Rect(0f, 0f, 1f, 1f); // ��ʑS��
                break;

            // 2�l�v���C��p���[�h(�r���[�|�[�g�����E�ɕ���)
            case PlayMode.TwoPlayer:
                this.PlayerCamera2.SetActive(true); // PlayerCamera2���A�N�e�B�u�ɂ���

                // �v���C���[2�̑���֘A�̃A�N�e�B�u��Ԃ�true�ɂ��Ă���
                LRMovePlayer2.enabled = true;
                JumpPlayer2.enabled = true;
                DecelerationPlayer2.enabled = true;
                CameraFunction2.enabled = true;

                Camera1.rect = new Rect(0f, 0f, 0.5f, 1f);   // ������
                Camera2.rect = new Rect(0.5f, 0f, 0.5f, 1f); // �E����
                break;

            default:
                break;
        }

        // �I�u�W�F�N�g�j��
        Destroy(gameObject);
    }

    private void AssignInputToPlayer(PlayerInput playerInput, InputDevice device, string controlScheme, string playerLabel)
    {
        if (playerInput == null || device == null)
        {
            Debug.LogWarning($"{playerLabel} �̓��͊��蓖�ĂɎ��s�F�f�o�C�X�� null");
            return;
        }

        // ���͂���U������
        playerInput.DeactivateInput();

        // ���݂�InputUser�ƃf�o�C�X���y�A�����O
        InputUser.PerformPairingWithDevice(device, playerInput.user);

        // ControlScheme�𖾎��I�Ɏw��
        playerInput.SwitchCurrentControlScheme(controlScheme, device);

        // ���͂��ėL����
        playerInput.ActivateInput();

        Debug.Log($"{playerLabel} �� {controlScheme} ���͂����蓖�Ă��܂���");
    }
}