//====================================================
// �X�N���v�g���FInputPlayerManager
// �쐬�ҁF����
// ���e�F�v���C���[�̓��͂��Ǘ�����N���X
// �ŏI�X�V���F05/29
// 
// [Log]
// 04/21 ���� �X�N���v�g�쐬 
// 04/21 ���� CPU�̃C���v�b�g�����؂�ւ��鏈����ǉ�
// 04/23 ���� �R���g���[���[�̊��蓖�ď�����ǉ�
// 04/25 ���� �X�N���v�g���ύX�A1�l���[�h�Œ�ɏC��
// 05/29 ���� UI�I��SE����
// 06/27 ���� UI�I��SE���ʒ�������
//====================================================
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ******* ���̃X�N���v�g�̎g���� ******* //
// 1. Player�I�u�W�F�N�g�ɂ��̃X�N���v�g���A�^�b�`
// 2. Player�I�u�W�F�N�g��PlayerInput�R���|�[�l���g��ǉ�
// 3. PlayerInput��Actions�ɁuPlayerInputActions�v��ݒ�
// 4. PlayerInput��Behavior�ɁuInvokeUnityEvents�v��ݒ�

public class InputPlayerManager : MonoBehaviour
{
    [Header("SE�ݒ�")]

    //���ʉ����Đ�����AudioSource
    [SerializeField] private AudioSource audioSource;

    //UI�ړ����ɍĐ�������ʉ�
    [SerializeField] private AudioClip MoveSE;

    //���莞�ɍĐ�������ʉ�
    [SerializeField] private AudioClip DecideSE;

    //�Ō�ɑI������Ă���UI�I�u�W�F�N�g
    private GameObject LastSelected;

    //���ʒ���(0.0�`1.0)
    [Range(0.0f, 1.0f)]
    [SerializeField] private float SEVolume = 0.5f;

    void Start()
    {
        //AudioSource�̉��ʂ������l�ɐݒ�
        if (audioSource != null)
        {
            audioSource.volume = SEVolume;
        }

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

        //���ݑI������Ă���UI�I�u�W�F�N�g���擾
        LastSelected = EventSystem.current.currentSelectedGameObject;

        //�S�Ẵ{�^���ɃN���b�NSE��ǉ�
        AddClickSEToAllButtons();
    }

    void Update()
    {
        //���ݑI������Ă���UI�I�u�W�F�N�g���擾
        GameObject CurrentSelected = EventSystem.current.currentSelectedGameObject;

        //�I������Ă���UI���O��ƈقȂ�Ƃ�(UI�ړ����������Ƃ�)
        if (CurrentSelected != null && CurrentSelected != LastSelected)
        {
            //���ʉ����Đ�
            PlayMoveSE();

            //���݂̑I�����L�^
            LastSelected = CurrentSelected;
        }
    }

    //���̓f�o�C�X���v���C���[�Ɋ��蓖�Ă鏈��
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

    //UI�ړ����̌��ʉ����Đ����鏈��
    private void PlayMoveSE()
    {
        if(audioSource != null && MoveSE != null)
        {
            if(audioSource != null && MoveSE != null)
            {
                //��񂾂����ʉ����Đ�����
                audioSource.PlayOneShot(MoveSE);
            }
        }
    }

    //UI���莞�ɖ炷SE
    private void PlayDecideSE()
    {
        if(audioSource != null && DecideSE != null)
        {
            audioSource.PlayOneShot(DecideSE);
        }
    }

    //�V�[�����̑S�Ẵ{�^���Ɂu����SE�v��ǉ����鏈��
    private void AddClickSEToAllButtons()
    {
        Button[] buttons = FindObjectsOfType<Button>(true);

        foreach (var button in buttons)
        {
            button.onClick.AddListener(() => PlayDecideSE());
        }
    }
}