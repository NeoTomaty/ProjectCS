//======================================================
// [TutorialDisplayTexts]
// �쐬�ҁF�r��C
// �ŏI�X�V���F06/27
// 
// [Log]
// 06/27�@�r��@�`���[�g���A���p��UI��\��������悤�Ɏ���
//======================================================
using UnityEngine;

public class TutorialDisplayTexts : MonoBehaviour
{
    [Header("�`���[�g���A���p��UI")]

    //�\������`���[�g���A��UI(Canvas�Ȃ�)�̔z��
    [SerializeField] private GameObject[] TutorialUI;

    //���ݕ\�����Ă���`���[�g���A���̃C���f�b�N�X
    private int TutorialIndex = 0;

    //UI���\�������ǂ����̃t���O(Update�ł̔���Ɏg�p)
    [System.NonSerialized] public bool IsDisplayUI = false;

    [Header("�Q��")]

    //�v���C���[�̏�Ԃ��Ǘ�����R���|�[�l���g(���t�e�B���O��ԂȂǂ��擾)
    [SerializeField] private PlayerStateManager PlayerStateManagerComponent;

    //���t�e�B���O�p�[�g�ɓ��������ǂ����̃t���O
    private bool IsLiftingPart = false;

    //���[�v�֘A�̃`���[�g���A�������łɕ\�����ꂽ���ǂ���
    private bool IsWarp = false;

    [Header("SE�֘A")]

    //�`���[�g���A���\�����ɍĐ�������ʉ�
    [SerializeField] private AudioClip TutorialSE;

    //���ʉ��̉���(0.0�`1.0)
    [SerializeField, Range(0.0f, 1.0f)] private float SEVolume = 0.5f;

    //���ʉ����Đ����邽�߂�AudioSource
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //�`���[�g���A��UI���\���ɂ���
        foreach (var UI in TutorialUI)
        {
            UI.SetActive(false);
        }

        //AudioSource�̎擾�܂��͒ǉ�
        audioSource = GetComponent<AudioSource>();

        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        //���ʐݒ�
        audioSource.volume = SEVolume;
    }

    // Update is called once per frame
    void Update()
    {
        //UI���\�����̂Ƃ��A�L�[���͂Ŏ��̃`���[�g���A���֐i��
        if (IsDisplayUI)
        {
            if (Input.anyKeyDown)
            {
                //���݂�UI���\���ɂ��A���̃C���f�b�N�X��
                TutorialUI[TutorialIndex].SetActive(false);
                TutorialIndex++;
                IsDisplayUI = false;

                //�Q�[���̎��Ԃ��ĊJ(UI�\�����͒�~���Ă���)
                Time.timeScale = 1f;

                //�`���[�g���A���̐i�s�ɉ����Ď���UI��\��
                switch(TutorialIndex)
                {
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        //4�Ԗڂ�UI��\��
                        DisplayTutorialUI4();
                        break;
                    case 4:
                        break;
                    case 5:
                        //6�Ԗڂ�UI��\��
                        DisplayTutorialUI6();
                        break;
                    case 6:
                        //7�Ԗڂ�UI��\��
                        DisplayTutorialUI7();
                        break;
                    default:
                        break;
                }

            }
        }

        //�v���C���[�����t�e�B���O�p�[�g�ɓ�������A�Ή�����UI��\��
        if (!IsLiftingPart && PlayerStateManagerComponent.GetLiftingState() == PlayerStateManager.LiftingState.LiftingPart)
        {
            IsLiftingPart = true;

            //3�Ԗڂ�UI��\��
            DisplayTutorialUI3();
        }
    }

    //���ʉ����Đ����鏈��
    private void PlaySE()
    {
        if(TutorialSE != null && audioSource != null)
        {
            audioSource.volume = SEVolume;
            audioSource.PlayOneShot(TutorialSE);
        }
    }

    //�ȉ��͂��ꂼ��̃`���[�g���A��UI��\������֐�
    public void DisplayTutorialUI1()
    {
        TutorialUI[0].SetActive(true);
        IsDisplayUI = true;

        //�Q�[�����ꎞ��~
        Time.timeScale = 0f;
        PlaySE();
    }

    public void DisplayTutorialUI2()
    {
        TutorialUI[1].SetActive(true);
        IsDisplayUI = true;
        Time.timeScale = 0f;
        PlaySE();
    }

    public void DisplayTutorialUI3()
    {
        TutorialUI[2].SetActive(true);
        IsDisplayUI = true;
        Time.timeScale = 0f;
        PlaySE();
    }

    public void DisplayTutorialUI4()
    {
        TutorialUI[3].SetActive(true);
        IsDisplayUI = true;
        Time.timeScale = 0f;
        PlaySE();
    }

    public void DisplayTutorialUI5()
    {
        //���[�v�֘A��UI�͈�x�����\�����Ȃ�
        if (IsWarp) return;
        IsWarp = true;
        TutorialUI[4].SetActive(true);
        IsDisplayUI = true;
        Time.timeScale = 0f;
        PlaySE();
    }

    public void DisplayTutorialUI6()
    {
        TutorialUI[5].SetActive(true);
        IsDisplayUI = true;
        Time.timeScale = 0f;
        PlaySE();
    }

    public void DisplayTutorialUI7()
    {
        TutorialUI[6].SetActive(true);
        IsDisplayUI = true;
        Time.timeScale = 0f;
        PlaySE();
    }
}
