//MenuController.cs
//�쐬��:��������
//�ŏI�X�V��:2025/05/22
//�A�^�b�`:StartButton�ɃA�^�b�`
//[Log]
//05/22�@�����@�^�C�g���̃L�[�{�[�h����ƃR���g���[���[���쏈��
//06/17�@�����@�{�^���I�����ƌ��莞��SE����

using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    //�X�^�[�g�{�^���A�I���{�^���A�I�v�V�����{�^�����C���X�y�N�^�[����ݒ�
    public Button StartButton;
    public Button ExitButton;
    public Button OptionButton;

    //�{�^����z��ŊǗ�
    private Button[] Buttons;

    //���ݑI������Ă���{�^���̃C���f�b�N�X
    private int SelectedIndex = 0;

    //���͂̊Ԋu(�A�����͂�h�����߂̃f�B���C)
    public float InputDelay = 0.3f;
    private float InputTimer = 0.0f;

    //SE�p��AudioClip��AudioSource��ǉ�
    public AudioClip SelectSE;
    public AudioClip SubmitSE;
    private AudioSource audioSource;

    void Start()
    {
        //�{�^����z��Ɋi�[
        Buttons = new Button[] 
        {
            StartButton, ExitButton, OptionButton
        };

        //AudioSource���擾
        audioSource = GetComponent<AudioSource>();

        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        //�����I����Ԃ��X�V
        UpdateButtonSelection();
    }

    void Update()
    {
        //���̓^�C�}�[������������
        InputTimer -= Time.deltaTime;

        //���������̓��͂��擾(�L�[�{�[�h�̍��E�L�[��Q�[���p�b�h�̃X�e�B�b�N)
        float HorizontalInput = Input.GetAxis("Horizontal");

        //���̓f�B���C���o�߂��Ă���ꍇ�̂ݏ���
        if (InputTimer <= 0f)
        {
            int PreviousIndex = SelectedIndex;
            int NextIndex = SelectedIndex;

            //�E����:���̃{�^���ֈړ�
            if (HorizontalInput > 0.5f)
            {
                if (SelectedIndex == 0)
                {
                    NextIndex = 1;
                }
                else if (SelectedIndex == 1)
                {
                    NextIndex = 2;
                }
            }
            //������:�O�̃{�^���ֈړ�
            else if (HorizontalInput < -0.5f)
            {
                if (SelectedIndex == 2)
                {
                    NextIndex = 1;
                }
                else if (SelectedIndex == 1)
                {
                    NextIndex = 0;
                }
            }

            //�I�����ς�����Ƃ��A�����ڂ��X�V
            if (NextIndex != SelectedIndex)
            {
                //�O�̃{�^���̃X�P�[�������Z�b�g
                ResetButtonScale(SelectedIndex);

                //�V�����I���ɍX�V
                SelectedIndex = NextIndex;

                //���̓f�B���C�����Z�b�g
                InputTimer = InputDelay;

                //�{�^���̌����ڂ��X�V
                UpdateButtonSelection();

                //�I������SE���Đ�
                PlaySE(SelectSE);
            }
        }

        //����{�^���������ꂽ��A���ݑI�𒆂̃{�^���̃N���b�N�C�x���g�����s
        if (Input.GetButtonDown("Submit"))
        {
            Buttons[SelectedIndex].onClick.Invoke();

            //���莞��SE�Đ�
            PlaySE(SubmitSE);
        }
    }

    //�{�^���̃X�P�[�������ɖ߂�(�I��������)
    void ResetButtonScale(int index)
    {
        Text ButtonText = Buttons[index].GetComponentInChildren<Text>();

        if (ButtonText != null)
        {
            ButtonText.transform.localScale = Vector3.one;
        }
    }

    //�{�^���̑I����Ԃɉ����ĐF��X�P�[����ύX
    void UpdateButtonSelection()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Image ButtonImage = Buttons[i].GetComponent<Image>();
            Text ButtonText = Buttons[i].GetComponentInChildren<Text>();

            if (i == SelectedIndex)
            {
                //�I�𒆂̃{�^��:������Ԃ��A�傫������
                ButtonImage.color = Color.white;

                if (ButtonText != null)
                {
                    ButtonText.color = Color.red;
                    ButtonText.transform.localScale = Vector3.one * 1.3f;
                }
            }
            else
            {
                //��I���̃{�^��:�����������A�ʏ�T�C�Y�ɖ߂�
                ButtonImage.color = Color.white;

                if (ButtonText != null)
                {
                    ButtonText.color = Color.black;
                    ButtonText.transform.localScale = Vector3.one;
                }
            }
        }
    }

    void PlaySE(AudioClip clip)
    {
        if(clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
