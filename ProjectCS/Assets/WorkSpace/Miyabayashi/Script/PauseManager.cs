//======================================================
// PauseManager �X�N���v�g
// �쐬�ҁF�{��
// �ŏI�X�V���F5/28
// 
// [Log]5/5 �{�с@�|�[�Y��ʂ�����
// 5/28�@�����@���j���[�J��SE����
//======================================================
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    //�|�[�Y���j���[��UI�I�u�W�F�N�g
    [SerializeField] private GameObject pauseUI;

    //�I�v�V�������j���[��UI�I�u�W�F�N�g
    [SerializeField] private GameObject optionUI;

    //�|�[�Y���j���[�ōŏ��ɑI�������{�^��
    [SerializeField] private GameObject firstPauseButton;

    //�I�v�V�������j���[�ōŏ��ɑI�������{�^��
    [SerializeField] private GameObject firstOptionButton;

    [Header("SE Settings")]

    //���ʉ����Đ����邽�߂�AudioSource
    [SerializeField] private AudioSource audioSource;

    //�|�[�Y���j���[���J�����Ƃ��ɖ炷���ʉ�
    [SerializeField] private AudioClip OpenSE;

    //�|�[�Y���j���[������Ƃ��ɖ炷���ʉ�
    [SerializeField] private AudioClip CloseSE;

    //���݃|�[�Y�����ǂ����������t���O
    private bool isPaused = false;

    //�v���C���[�̓��͂��Ǘ�����R���|�[�l���g
    private PlayerInput playerInput;

    //�uPause�v�A�N�V����(Esc�L�[)���擾���邽�߂̕ϐ�
    private InputAction pauseAction;

    //�Q�[���J�n���ɌĂ΂��(����������)
    private void Awake()
    {
        //PlayerInput�R���|�[�l���g���擾
        playerInput = GetComponent<PlayerInput>();

        if (playerInput != null)
        {
            //���̓A�N�V�����}�b�v����uPause�v�A�N�V�������擾
            pauseAction = playerInput.actions["Pause"];
        }
        else
        {
            Debug.LogError("PlayerInput��������܂���I");
        }
    }

    //�I�u�W�F�N�g���L���ɂȂ����Ƃ��ɌĂ΂��
    private void OnEnable()
    {
        if (pauseAction != null)
        {
            //Pause�A�N�V���������s���ꂽ�Ƃ��̃C�x���g��o�^
            pauseAction.performed += OnPausePerformed;
            pauseAction.Enable();
        }
    }

    //�I�u�W�F�N�g�������ɂȂ����Ƃ��ɌĂ΂��
    private void OnDisable()
    {
        if (pauseAction != null && playerInput.actions != null)
        {
            //�C�x���g�o�^������
            pauseAction.performed -= OnPausePerformed;
            pauseAction.Disable();
        }
    }

    //Pause�A�N�V���������s���ꂽ�Ƃ��ɌĂ΂�鏈��
    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        Debug.Log("esc");

        //���͂��������Ȃ��Ƃ��͏������Ȃ�
        if (!context.performed) return;

        // �I�v�V�������J���Ă�����|�[�Y�؂�ւ�����
        if (optionUI != null && optionUI.activeSelf) return;

        //�|�[�Y��ԂłȂ���΃|�[�Y���J�n
        if (!isPaused)
        {
            //�Q�[�����̎��Ԃ��~
            Time.timeScale = 0f;

            //�|�[�YUI��\��
            pauseUI.SetActive(true);
            
            //UI�̑I����Ԃ����Z�b�g���āA�ŏ��̃{�^����I��
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstPauseButton);

            //�|�[�Y��Ԃɐݒ�
            isPaused = true;

            //�J���Ƃ��̌��ʉ����Đ�
            PlaySE(OpenSE);
        }
        else
        {
            //�|�[�Y������
            ResumeGame();
        }
    }

    //�I�v�V�������j���[���J������
    public void OpenOption()
    {
        //�I�v�V����UI��\�����A�|�[�YUI���\����
        optionUI.SetActive(true);
        pauseUI.SetActive(false);

        //�ŏ��ɑI�������I�v�V�����{�^����I����Ԃɂ���
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstOptionButton);
    }

    //�|�[�Y���������ăQ�[�����ĊJ���鏈��
    public void ResumeGame()
    {
        //�Q�[�����̎��Ԃ��ĊJ
        Time.timeScale = 1f;

        //�|�[�YUI���\����
        pauseUI.SetActive(false);

        //�|�[�Y��Ԃ�����
        isPaused = false;

        //����Ƃ��̌��ʉ����Đ�
        PlaySE(CloseSE);
    }

    //���݃|�[�Y�����ǂ������O������擾���邽�߂̊֐�
    public bool IsPaused()
    {
        return isPaused;
    }

    //�|�[�YUI�̕\���E��\����؂�ւ���֐�
    public void SetPauseUIVisible(bool visible)
    {
        pauseUI.SetActive(visible);
    }

    //���ʉ����Đ����鋤�ʊ֐�
    private void PlaySE(AudioClip clip)
    {
        if(audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}