//ButtonNavigation.cs
//�쐬��: ��������
//�ŏI�X�V��: 2025/07/11
//�A�^�b�`�Ώ�: Start�{�^���Ȃǂ�UI�I�u�W�F�N�g
//[Log]
//05/11 ���� ���j���[�I��&���菈��
//06/25 ���� �R���g���[���[�̏C��&�L�[�{�[�h����̏C��&�{�^���̊g��\��
//06/26 ���� ���j���[�I��&����SE���ʒ�������
//07/10 ���� ���L�[����WASD�̃L�[����ɕύX
//07/11 ���� �{�^���̊g��\�������₷���C��

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonNavigation : MonoBehaviour
{
    //���j���[�Ƃ��Ďg�p����{�^���̔z��
    public Button[] Buttons;

    //�{�^���ړ����ɍĐ�������ʉ�
    public AudioClip NavigationSE;

    //�{�^�����莞�ɍĐ�������ʉ�
    public AudioClip DecisionSE;

    //���ʉ����Đ����邽�߂�AudioSource
    private AudioSource audioSource;

    //���ݑI�𒆂̃{�^���̃C���f�b�N�X
    private int CurrentIndex = 0;

    //�e�{�^���̌��̃X�P�[����ۑ�����z��
    private Vector3[] OriginalScales;

    //�X�e�B�b�N����̃N�[���_�E������
    private float StickCoolDown = 0.3f;

    //�N�[���_�E���^�C�}�[
    private float StickTimer = 0.0f;

    //�X�e�B�b�N���j���[�g�����ɖ߂������ǂ���
    private bool StickReleased = true;

    //���ʉ��̉���
    [Range(0.0f, 1.0f)]
    public float SEVolume = 0.5f;

    //�I�v�V������ʂ��J���Ă���Ƃ��͑���𖳌���
    [Header("UI Blocks Input")]
    [SerializeField] private GameObject optionUI;

    void Start()
    {
        //AudioSource��ǉ����ĉ��ʂ�ݒ�
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = SEVolume;

        //�e�{�^���̌��̃T�C�Y��ۑ�
        OriginalScales = new Vector3[Buttons.Length];
        for (int i = 0; i < Buttons.Length; i++)
        {
            OriginalScales[i] = Buttons[i].transform.localScale;
        }

        //�ŏ��̃{�^����I����Ԃɂ��A�g��\��
        EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);
        EnlargeButton(Buttons[CurrentIndex]);
    }

    void Update()
    {
        //�I�v�V������ʂ��\������Ă���Ƃ��͑���𖳌���
        if (optionUI != null && optionUI.activeSelf) return;

        //�N�[���_�E���^�C�}�[������
        StickTimer -= Time.unscaledDeltaTime;

        //���������̓���(W/S�L�[��X�e�B�b�N�㉺)
        float Vertical = Input.GetAxisRaw("Vertical");

        //���L�[�̓��͖͂���
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            return;
        }

        //�X�e�B�b�N���j���[�g�����ɖ߂���������
        if (Mathf.Abs(Vertical) < 0.2f)
        {
            StickReleased = true;
        }

        //����L�[(Enter�܂���Submit)�������ꂽ�Ƃ�
        if (Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.Return))
        {
            //���艹���Đ�
            PlayDecisionSE();

            //���݂̃{�^�������s
            Buttons[CurrentIndex].onClick.Invoke();
        }

        //������(S�L�[�܂��̓X�e�B�b�N��)�ɓ��͂��ꂽ�Ƃ�
        if ((Input.GetKeyDown(KeyCode.S) || (Vertical < -0.5f && StickTimer <= 0.0f && StickReleased)))
        {
            StickReleased = false;
            StickTimer = StickCoolDown;
            int nextIndex = CurrentIndex + 1;

            //�͈͊O�Ȃ牽�����Ȃ�
            if (nextIndex >= Buttons.Length) return;

            //�ړ������Đ�
            PlayNavigationSE();

            //���݂̃{�^�������̃T�C�Y�ɖ߂�
            ResetButton(Buttons[CurrentIndex]);

            //�C���f�b�N�X�X�V
            CurrentIndex = nextIndex;

            //�V�����{�^����I��
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);

            //�V�����{�^�����g��\��
            EnlargeButton(Buttons[CurrentIndex]);
        }

        //�����(W�L�[�܂��̓X�e�B�b�N��)�ɓ��͂��ꂽ�Ƃ�
        if ((Input.GetKeyDown(KeyCode.W) || (Vertical > 0.5f && StickTimer <= 0.0f && StickReleased)))
        {
            StickReleased = false;
            StickTimer = StickCoolDown;
            int nextIndex = CurrentIndex - 1;

            //�͈͊O�Ȃ牽�����Ȃ�
            if (nextIndex < 0) return;

            PlayNavigationSE();
            ResetButton(Buttons[CurrentIndex]);
            CurrentIndex = nextIndex;
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);
            EnlargeButton(Buttons[CurrentIndex]);
        }
    }

    //�I�����ꂽ�{�^�����g��\�����鏈��
    void EnlargeButton(Button button)
    {
        int index = System.Array.IndexOf(Buttons, button);
        if (index >= 0)
        {
            button.transform.localScale = OriginalScales[index] * 1.5f;
        }
    }

    //�{�^���̃T�C�Y�����ɖ߂�����
    void ResetButton(Button button)
    {
        int index = System.Array.IndexOf(Buttons, button);
        if (index >= 0)
        {
            button.transform.localScale = OriginalScales[index];
        }
    }

    //�ړ����̌��ʉ����Đ�
    void PlayNavigationSE()
    {
        if (NavigationSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(NavigationSE, SEVolume);
        }
    }

    //���莞�̌��ʉ����Đ�
    void PlayDecisionSE()
    {
        if (DecisionSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(DecisionSE, SEVolume);
        }
    }

    //���ׂẴ{�^���̃T�C�Y�����ɖ߂�
    void ResetAllButtons()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].transform.localScale = OriginalScales[i];
        }
    }
}
