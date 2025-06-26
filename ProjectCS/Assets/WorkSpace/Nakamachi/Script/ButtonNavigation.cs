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

    //�e�{�^���̌��̃X�P�[����ۑ�����z��(�g��E�k���̊)
    private Vector3[] OriginalScales;

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


        //�ŏ��̃{�^����I����Ԃɐݒ�
        EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);

        //�ŏ��̃{�^�����g��\��(�I�𒆂̋���)
        EnlargeButton(Buttons[CurrentIndex]);
    }

    void Update()
    {
        //���L�[�������ꂽ��O�̃{�^���Ɉړ�
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //SE�Đ�
            PlayNavigationSE();

            //���݂̃{�^���̊g�������
            ResetButton(Buttons[CurrentIndex]);

            //�C���f�b�N�X��O�Ɉړ�(���[�v)
            CurrentIndex = (CurrentIndex - 1 + Buttons.Length) % Buttons.Length;

            //�V�����{�^����I��
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);

            //�V�����{�^�����g��\��
            EnlargeButton(Buttons[CurrentIndex]);
        }

        //���L�[�������ꂽ�玟�̃{�^���Ɉړ�
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //SE�Đ�
            PlayNavigationSE();

            //���݂̃{�^���̊g�������
            ResetButton(Buttons[CurrentIndex]);

            //�C���f�b�N�X�����Ɉړ�(���[�v)
            CurrentIndex = (CurrentIndex + 1) % Buttons.Length;

            //�V�����{�^����I��
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);

            //�V�����{�^�����g��\��
            EnlargeButton(Buttons[CurrentIndex]);
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

    //�I�𒆂̃{�^�����g��\�����鏈��(�摜���܂߂Ċg��)
    void EnlargeButton(Button button)
    {
        button.transform.localScale = OriginalScales[CurrentIndex] * 1.2f;
    }

    //�I���������ꂽ�{�^���̃X�P�[�������ɖ߂�����
    void ResetButton(Button button)
    {
        //�z����̃C���f�b�N�X���擾���Č��̃X�P�[���ɖ߂�
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
