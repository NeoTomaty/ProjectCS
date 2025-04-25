//====================================================
// �X�N���v�g���FInputPlayerManager
// �쐬�ҁF����
// ���e�F�v���C���[�̓��͂��Ǘ�����N���X
// �ŏI�X�V���F04/25
// 
// [Log]
// 04/21 ���� �X�N���v�g�쐬 
// 04/21 ���� CPU�̃C���v�b�g�����؂�ւ��鏈����ǉ�
// 04/23 ���� �R���g���[���[�̊��蓖�ď�����ǉ�
// 04/25 ���� �X�N���v�g���ύX�A1�l���[�h�Œ�ɏC��
//====================================================
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

// ******* ���̃X�N���v�g�̎g���� ******* //
// 1. Player�I�u�W�F�N�g�ɂ��̃X�N���v�g���A�^�b�`
// 2. Player�I�u�W�F�N�g��PlayerInput�R���|�[�l���g��ǉ�
// 3. PlayerInput��Actions�ɁuPlayerInputActions�v��ݒ�
// 4. PlayerInput��Behavior�ɁuInvokeUnityEvents�v��ݒ�

public class InputPlayerManager : MonoBehaviour
{
    void Start()
    {
        // PlayerInputComponent���擾
        PlayerInput PlayerInputComponent = gameObject.GetComponent<PlayerInput>();
       
        // �R���g���[���[�擾
        var gamepads = Gamepad.all;
        
        // �ڑ��R���g���[���[1��ȏ�
        if(gamepads.Count >= 1)
        {
            // �R���g���[���[����ɐݒ�
            AssignInputToPlayer(PlayerInputComponent, gamepads[0], "Gamepad", "�v���C���[");
        }
        // �ڑ��R���g���[���[0��
        else if (gamepads.Count == 0)
        {
            // �L�[�{�[�h����ɐݒ�
            AssignInputToPlayer(PlayerInputComponent, Keyboard.current, "Keyboard", "�v���C���[");
        }
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