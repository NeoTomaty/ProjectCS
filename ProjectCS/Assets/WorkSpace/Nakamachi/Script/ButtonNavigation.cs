//ButtonNavigation.cs
//�쐬��:��������
//�ŏI�X�V��:2025/05/11
//�A�^�b�`:Start�{�^���ɃA�^�b�`
//[Log]
//05/11�@�����@���j���[�I��&���菈��

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

    //���ʉ��Đ��p��AudioSource
    private AudioSource audioSource;

    //���ݑI�𒆂̃{�^���̃C���f�b�N�X
    private int CurrentIndex = 0;

    //�e�L�X�g�̌��̃X�P�[��(�g��E�k���p)
    private Vector3 OriginalScale;

    //�e�L�X�g�̌��̐F(�I��F�؂�ւ��p)
    private Color OriginalColor;

    void Start()
    {
        //AudioSource�����̃I�u�W�F�N�g�ɒǉ�(SE�Đ��p)
        audioSource = gameObject.AddComponent<AudioSource>();

        //�ŏ��̃{�^����I����Ԃɐݒ�
        EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);

        //�I�𒆃{�^���̃e�L�X�g�̌��̃X�P�[���ƐF��ۑ�
        OriginalScale = Buttons[CurrentIndex].GetComponentInChildren<Text>().transform.localScale;
        OriginalColor = Buttons[CurrentIndex].GetComponentInChildren<Text>().color;

        //�ŏ��̃{�^���̃e�L�X�g�������\��(�g��{�F�ύX)
        EnlargeText(Buttons[CurrentIndex]);
        ChangeTextColor(Buttons[CurrentIndex], Color.red);
    }

    void Update()
    {
        //���L�[�������ꂽ��O�̃{�^���Ɉړ�
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //SE�Đ�
            PlayNavigationSE();

            //���݂̃{�^���̋���������
            ResetText(Buttons[CurrentIndex]);

            //�C���f�b�N�X��O�Ɉړ�(���[�v)
            CurrentIndex = (CurrentIndex - 1 + Buttons.Length) % Buttons.Length;

            //�V�����{�^����I��
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);

            //�V�����{�^��������
            EnlargeText(Buttons[CurrentIndex]);
            ChangeTextColor(Buttons[CurrentIndex], Color.red);
        }

        //���L�[�������ꂽ�玟�̃{�^���Ɉړ�
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //SE�Đ�
            PlayNavigationSE();

            //���݂̃{�^���̋���������
            ResetText(Buttons[CurrentIndex]);

            //�C���f�b�N�X�����Ɉړ�(���[�v)
            CurrentIndex = (CurrentIndex + 1) % Buttons.Length;

            //�V�����{�^����I��
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);

            //�V�����{�^��������
            EnlargeText(Buttons[CurrentIndex]);
            ChangeTextColor(Buttons[CurrentIndex], Color.red);
        }

        //Enter�L�[�������ꂽ�猻�݂̃{�^�������s(�N���b�N)
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //����SE���Đ�
            PlayDecisionSE();

            //�{�^���̃N���b�N�C�x���g���Ăяo��
            Buttons[CurrentIndex].onClick.Invoke();
        }
    }

    //�e�L�X�g���g��\�����鏈��
    void EnlargeText(Button button)
    {
        button.GetComponentInChildren<Text>().transform.localScale = OriginalScale * 1.2f;
    }

    //�e�L�X�g�̊g��ƐF�����ɖ߂�����
    void ResetText(Button button)
    {
        button.GetComponentInChildren<Text>().transform.localScale = OriginalScale;
        button.GetComponentInChildren<Text>().color = OriginalColor;
    }

    //�e�L�X�g�̐F��ύX���鏈��
    void ChangeTextColor(Button button, Color color)
    {
        button.GetComponentInChildren<Text>().color = color;
    }

    //�{�^���ړ����̌��ʉ����Đ����鏈��
    void PlayNavigationSE()
    {
        if(NavigationSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(NavigationSE);
        }
    }

    //�{�^�����莞�̌��ʉ����Đ����鏈��
    void PlayDecisionSE()
    {
        if(DecisionSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(DecisionSE);
        }
    }
}
