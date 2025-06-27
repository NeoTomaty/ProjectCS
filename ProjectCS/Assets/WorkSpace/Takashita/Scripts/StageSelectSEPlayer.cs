//====================================================
// �X�N���v�g���FStageSelectSEPlayer
// �쐬�ҁF����
// ���e�F�X�e�[�W�I����ʐ�p��SE�Đ��X�N���v�g
// �ŏI�X�V���F06/17
// 
// [Log]
// 06/17 ���� �X�N���v�g�쐬
// 06/26 ���� SE���ʒ���
//====================================================
using UnityEngine;

public class StageSelectSEPlayer : MonoBehaviour
{
    //�X�e�[�W�I����ʂŎg�p����SE�̎�ނ��`
    public enum StageSelectSE
    {
        Confirm,  // ����{�^�����������Ƃ���SE
        Cancel,   // �߂�{�^�����������Ƃ���SE
        Select    // �J�[�\���ړ����ړ������Ƃ���SE
    }

    //���莞�ɍĐ�����SE
    [Header("����SE")]
    [SerializeField] private AudioClip ConfirmSE;

    //�߂鎞�ɍĐ�����SE
    [Header("�߂�SE")]
    [SerializeField] private AudioClip CancelSE;

    //�J�[�\���ړ����ɍĐ�����SE
    [Header("�J�[�\���ړ�SE")]
    [SerializeField] private AudioClip SelectSE;

    //SE�̉��ʒ���
    [Header("SE����(0.0�`1.0)")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float SEVolume = 0.5f;

    //SE�Đ��p��AudioSource�R���|�[�l���g
    private AudioSource AudioSourceComponent;

    //����������
    void Start()
    {
        //����GameObject�ɃA�^�b�`����Ă���AudioSource���擾
        AudioSourceComponent = GetComponent<AudioSource>();

        //AudioSource���擾�ł��Ă���΁A���ʂ�ݒ�
        if (AudioSourceComponent != null)
        {
            AudioSourceComponent.volume = SEVolume;
        }
    }

    //�w�肳�ꂽ��ނ�SE���Đ�����֐�
    public void PlaySE(StageSelectSE se)
    {
        switch(se)
        {
            case StageSelectSE.Confirm:
                PlaySE(ConfirmSE); //����SE���Đ�
                break;
            case StageSelectSE.Cancel:
                PlaySE(CancelSE); //�߂�SE���Đ�
                break;
            case StageSelectSE.Select:
                PlaySE(SelectSE); //�J�[�\���ړ�SE���Đ�
                break;
        }
        
    }

    //���ۂ�AudioClip���Đ���������֐�
    private void PlaySE(AudioClip clip)
    {
        //AudioClip��AudioSource���L���ł���΁A�w�艹�ʂōĐ�
        if (clip != null && AudioSourceComponent != null)
        {
            AudioSourceComponent.PlayOneShot(clip,SEVolume);
        }
    }

    //�O������SE���ʂ�ύX���邽�߂̊֐�
    public void SetSEVolume(float volume)
    {
        //0.0�`1.0�͈̔͂ɐ���
        SEVolume = Mathf.Clamp01(volume);

        //AudioSource���L���ł���΁A���ʂ��X�V
        if (AudioSourceComponent != null)
        {
            AudioSourceComponent.volume = SEVolume;
        }
    }
}
