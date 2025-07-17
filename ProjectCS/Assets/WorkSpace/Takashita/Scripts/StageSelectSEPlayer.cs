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
    [SerializeField] private float ConfirmVolume = 0.5f;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float CancelVolume = 0.5f;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float SelectVolume = 0.5f;

    //SE�Đ��p��AudioSource�R���|�[�l���g
    private AudioSource AudioSourceComponent;

    //����������
    void Start()
    {
        //����GameObject�ɃA�^�b�`����Ă���AudioSource���擾
        AudioSourceComponent = GetComponent<AudioSource>();
    }

    //�w�肳�ꂽ��ނ�SE���Đ�����֐�
    public void PlaySE(StageSelectSE se)
    {
        switch(se)
        {
            case StageSelectSE.Confirm:
                PlaySE(ConfirmSE, ConfirmVolume); //����SE���Đ�
                break;
            case StageSelectSE.Cancel:
                PlaySE(CancelSE, CancelVolume); //�߂�SE���Đ�
                break;
            case StageSelectSE.Select:
                PlaySE(SelectSE, SelectVolume); //�J�[�\���ړ�SE���Đ�
                break;
        }
        
    }

    //���ۂ�AudioClip���Đ���������֐�
    private void PlaySE(AudioClip clip, float volume)
    {
        //AudioClip��AudioSource���L���ł���΁A�w�艹�ʂōĐ�
        if (clip != null && AudioSourceComponent != null)
        {
            AudioSourceComponent.PlayOneShot(clip, volume);
        }
    }

}
