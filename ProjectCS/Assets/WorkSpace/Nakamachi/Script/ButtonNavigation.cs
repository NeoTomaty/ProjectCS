// ButtonNavigation.cs
// �쐬��: ��������
// �ŏI�X�V��: 2025/05/11
// �A�^�b�`�Ώ�: Start�{�^���Ȃǂ�UI�I�u�W�F�N�g
// [Log]
// 05/11 ���� ���j���[�I�������菈��

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonNavigation : MonoBehaviour
{
    //���j���[�Ƃ��Ďg�p����{�^���̔z��
    public Button[] Buttons;

    //�{�^���ړ����ɍĐ�������ʉ�(AudioClip)
    public AudioClip NavigationSE;

    //�{�^�����莞�ɍĐ�������ʉ�(AudioClip)
    public AudioClip DecisionSE;

    //���ʉ����Đ����邽�߂�AudioSource(�X�N���v�g���Ŏ����ǉ�)
    private AudioSource audioSource;

    //���ݑI�𒆂̃{�^���̃C���f�b�N�X(�z��̉��Ԗڂ�)
    private int CurrentIndex = 0;

    //�e�{�^���̌��̃X�P�[����ۑ�����z��(�g��E�k���̊)
    private Vector3[] OriginalScales;

    //�X�e�B�b�N����̃N�[���_�E������(�b)
    private float StickCoolDown = 1.0f;

    //�N�[���_�E���^�C�}�[(���Ԍo�߂Ō���)
    private float StickTimer = 0.0f;

    //�X�e�B�b�N���j���[�g�����ɖ߂������ǂ����̔���
    private bool StickReleased = true;

    void Start()
    {
        //AudioSource�����̃I�u�W�F�N�g�ɒǉ�(SE�Đ��p)
        audioSource = gameObject.AddComponent<AudioSource>();

        //�e�{�^���̌��̃X�P�[����ۑ�(��Ō��ɖ߂�����)
        OriginalScales = new Vector3[Buttons.Length];
        for (int i = 0; i < Buttons.Length; i++)
        {
            OriginalScales[i] = Buttons[i].transform.localScale;
        }

        //�ŏ��̃{�^����I����Ԃɐݒ�(EventSystem��UI�̑I����Ԃ��Ǘ�)
        EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);

        //�ŏ��̃{�^�����g��\��(�I�𒆂̋���)
        EnlargeButton(Buttons[CurrentIndex]);
    }

    void Update()
    {
        //�N�[���_�E���^�C�}�[������������
        StickTimer -= Time.unscaledDeltaTime;

        //���X�e�B�b�N�̉������̓��͒l���擾
        float Horizontal = Input.GetAxis("Horizontal");

        //�X�e�B�b�N���j���[�g�����ɖ߂�����t���O�𗧂Ă�
        if (Mathf.Abs(Horizontal) < 0.2f)
        {
            StickReleased = true;
        }

        //A�{�^��(Submit)�܂���Enter�L�[�Ō���
        if (Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.Return))
        {
            //����SE���Đ�
            PlayDecisionSE();

            //�{�^���̃N���b�N�C�x���g���Ăяo��
            Buttons[CurrentIndex].onClick.Invoke();
        }

        //���L�[�܂��͍��X�e�B�b�N���őO�̃{�^���Ɉړ�
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || (Horizontal < -0.5f && StickTimer <= 0.0f && StickReleased)))
        {
            StickReleased = false;
            StickTimer = StickCoolDown;

            int nextIndex = CurrentIndex - 1;

            //Option(1)��Start(0)�̈ړ����֎~
            if (nextIndex < 0 || (CurrentIndex == 1 && nextIndex == 0)) return;

            //�ړ�SE���Đ�
            PlayNavigationSE();

            //���݂̃{�^���̊g�������
            ResetButton(Buttons[CurrentIndex]);
            CurrentIndex = nextIndex;

            //�V�����{�^����I��
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);
            EnlargeButton(Buttons[CurrentIndex]);
        }

        //���L�[�܂��͍��X�e�B�b�N�E�Ŏ��̃{�^���Ɉړ�
        if ((Input.GetKeyDown(KeyCode.RightArrow) || (Horizontal > 0.5f && StickTimer <= 0.0f && StickReleased)))
        {
            StickReleased = false;
            StickTimer = StickCoolDown;

            int nextIndex = CurrentIndex + 1;

            //Start(0)��Option(1)�̈ړ����֎~
            if (nextIndex >= Buttons.Length || (CurrentIndex == 0 && nextIndex == 1)) return;

            //�ړ�SE���Đ�
            PlayNavigationSE();

            //���݂̃{�^���̊g�������
            ResetButton(Buttons[CurrentIndex]);
            CurrentIndex = nextIndex;

            //�V�����{�^����I��
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);

            //�V�����{�^�����g��\��
            EnlargeButton(Buttons[CurrentIndex]);
        }
    }

    //�I�𒆂̃{�^�����g��\�����鏈��(�摜���܂߂Ċg��)
    void EnlargeButton(Button button)
    {
        int index = System.Array.IndexOf(Buttons, button);
        if (index >= 0)
        {
            button.transform.localScale = OriginalScales[index] * 1.2f;
        }
    }

    //�I���������ꂽ�{�^���̃X�P�[�������ɖ߂�����
    void ResetButton(Button button)
    {
        int index = System.Array.IndexOf(Buttons, button);
        if (index >= 0)
        {
            button.transform.localScale = OriginalScales[index];
        }
    }

    //�{�^���ړ����̌��ʉ����Đ����鏈��
    void PlayNavigationSE()
    {
        if (NavigationSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(NavigationSE);
        }
    }

    //�{�^�����莞�̌��ʉ����Đ����鏈��
    void PlayDecisionSE()
    {
        if (DecisionSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(DecisionSE);
        }
    }
}
