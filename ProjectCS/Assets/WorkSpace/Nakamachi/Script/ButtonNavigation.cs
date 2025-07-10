// ButtonNavigation.cs
// �쐬��: ��������
// �ŏI�X�V��: 2025/06/26
// �A�^�b�`�Ώ�: Start�{�^���Ȃǂ�UI�I�u�W�F�N�g
// [Log]
// 05/11 ���� ���j���[�I��&���菈��
// 06/25 ���� �R���g���[���[�̏C��&�L�[�{�[�h����̏C��&�{�^���̊g��\��
// 06/26 ���� ���j���[�I��&����SE���ʒ�������
// 07/10 ���� ���L�[����WASD�̃L�[����ɕύX

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonNavigation : MonoBehaviour
{
    // ���j���[�Ƃ��Ďg�p����{�^���̔z��
    public Button[] Buttons;

    // �{�^���ړ����ɍĐ�������ʉ�
    public AudioClip NavigationSE;

    // �{�^�����莞�ɍĐ�������ʉ�
    public AudioClip DecisionSE;

    // ���ʉ����Đ����邽�߂�AudioSource
    private AudioSource audioSource;

    // ���ݑI�𒆂̃{�^���̃C���f�b�N�X
    private int CurrentIndex = 0;

    // �e�{�^���̌��̃X�P�[����ۑ�����z��
    private Vector3[] OriginalScales;

    // �X�e�B�b�N����̃N�[���_�E������
    private float StickCoolDown = 0.3f;

    // �N�[���_�E���^�C�}�[
    private float StickTimer = 0.0f;

    // �X�e�B�b�N���j���[�g�����ɖ߂������ǂ���
    private bool StickReleased = true;

    // ���ʉ��̉���
    [Range(0.0f, 1.0f)]
    public float SEVolume = 0.5f;

    [Header("UI Blocks Input")]
    [SerializeField] private GameObject optionUI; // Option UI���C���X�y�N�^�[����ݒ�

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = SEVolume;

        OriginalScales = new Vector3[Buttons.Length];
        for (int i = 0; i < Buttons.Length; i++)
        {
            OriginalScales[i] = Buttons[i].transform.localScale;
        }

        EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);
        EnlargeButton(Buttons[CurrentIndex]);
    }

    void Update()
    {
        // Option��ʂ��A�N�e�B�u�Ȃ�L�[��������ׂĖ�����
        if (optionUI != null && optionUI.activeSelf) return;

        StickTimer -= Time.unscaledDeltaTime;
        float Vertical = Input.GetAxisRaw("Vertical");

        //���L�[�̓��͂𖳎�
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            return;
        }

        if (Mathf.Abs(Vertical) < 0.2f)
        {
            StickReleased = true;
        }

        if (Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.Return))
        {
            PlayDecisionSE();
            Buttons[CurrentIndex].onClick.Invoke();
        }

        if ((Input.GetKeyDown(KeyCode.S) || (Vertical < -0.5f && StickTimer <= 0.0f && StickReleased)))
        {
            StickReleased = false;
            StickTimer = StickCoolDown;
            int nextIndex = CurrentIndex + 1;
            if (nextIndex >= Buttons.Length) return;

            PlayNavigationSE();
            ResetButton(Buttons[CurrentIndex]);
            CurrentIndex = nextIndex;
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);
            EnlargeButton(Buttons[CurrentIndex]);
        }

        if ((Input.GetKeyDown(KeyCode.W) || (Vertical > 0.5f && StickTimer <= 0.0f && StickReleased)))
        {
            StickReleased = false;
            StickTimer = StickCoolDown;
            int nextIndex = CurrentIndex - 1;
            if (nextIndex < 0) return;

            PlayNavigationSE();
            ResetButton(Buttons[CurrentIndex]);
            CurrentIndex = nextIndex;
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);
            EnlargeButton(Buttons[CurrentIndex]);
        }
    }

    void EnlargeButton(Button button)
    {
        int index = System.Array.IndexOf(Buttons, button);
        if (index >= 0)
        {
            button.transform.localScale = OriginalScales[index] * 1.2f;
        }
    }

    void ResetButton(Button button)
    {
        int index = System.Array.IndexOf(Buttons, button);
        if (index >= 0)
        {
            button.transform.localScale = OriginalScales[index];
        }
    }

    void PlayNavigationSE()
    {
        if (NavigationSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(NavigationSE, SEVolume);
        }
    }

    void PlayDecisionSE()
    {
        if (DecisionSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(DecisionSE, SEVolume);
        }
    }

    void ResetAllButtons()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].transform.localScale = OriginalScales[i];
        }
    }
}
