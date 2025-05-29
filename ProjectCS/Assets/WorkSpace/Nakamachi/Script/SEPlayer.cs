//SEPlayer.cs
//�쐬��:��������
//�ŏI�X�V��:2025/05/29
//�A�^�b�`:PauseManager�ɃA�^�b�`
//[Log]
//05/29�@�����@���j���[����SE

using UnityEngine;

public class SEPlayer : MonoBehaviour
{
    //SEPlayer�̃C���X�^���X��ێ�����ÓI�ϐ�(�V���O���g���p�^�[��)
    public static SEPlayer Instance;

    //���ʉ����Đ����邽�߂�AudioSource(�C���X�y�N�^�[�Őݒ�)
    [SerializeField] private AudioSource audioSource;

    //�I�����ɍĐ�������ʉ�(�C���X�y�N�^�[�Őݒ�)
    [SerializeField] private AudioClip SelectSE;

    //�I�u�W�F�N�g���������ꂽ�Ƃ��ɌĂяo����郁�\�b�h
    private void Awake()
    {
        //�܂��C���X�^���X�����݂��Ȃ��Ƃ��͂��̃I�u�W�F�N�g���C���X�^���X�Ƃ��Đݒ�
        if(Instance == null)
        {
            Instance = this;

            //���̃I�u�W�F�N�g���V�[�����؂�ւ���Ă��j�����Ȃ��悤�ɐݒ�
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //���łɃC���X�^���X�����݂���Ƃ��͏d��������邽�߂ɂ��̃I�u�W�F�N�g��j��
            Destroy(gameObject);
        }
    }

    //�I�����ʉ����Đ����郁�\�b�h(�{�^���Ȃǂ���Ăяo��)
    public void PlaySelectSE()
    {
        //���ʉ���AudioSource���ݒ肳��Ă���Ƃ��̂ݍĐ�
        if (SelectSE != null && audioSource != null)
        {
            //��񂾂����ʉ����Đ�(�d�˂čĐ��\)
            audioSource.PlayOneShot(SelectSE);
        }
    }
}