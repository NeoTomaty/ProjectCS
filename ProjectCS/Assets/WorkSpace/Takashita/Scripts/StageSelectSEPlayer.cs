//====================================================
// �X�N���v�g���FStageSelectSEPlayer
// �쐬�ҁF����
// ���e�F�X�e�[�W�I����ʐ�p��SE�Đ��X�N���v�g
// �ŏI�X�V���F06/17
// 
// [Log]
// 06/17 ���� �X�N���v�g�쐬
//====================================================
using UnityEngine;

public class StageSelectSEPlayer : MonoBehaviour
{
    public enum StageSelectSE
    {
        Confirm,  // ����
        Cancel,   // �߂�
        Select    // �J�[�\���ړ�
    }

    [Header("����SE")]
    [SerializeField] private AudioClip ConfirmSE;
    [Header("�߂�SE")]
    [SerializeField] private AudioClip CancelSE;
    [Header("�J�[�\���ړ�SE")]
    [SerializeField] private AudioClip SelectSE;

    private AudioSource AudioSourceComponent;

    void Start()
    {
        AudioSourceComponent = GetComponent<AudioSource>();
    }

    public void PlaySE(StageSelectSE se)
    {
        switch(se)
        {
            case StageSelectSE.Confirm:
                PlaySE(ConfirmSE);
                break;
            case StageSelectSE.Cancel:
                PlaySE(CancelSE);
                break;
            case StageSelectSE.Select:
                PlaySE(SelectSE);
                break;
        }
        
    }

    private void PlaySE(AudioClip clip)
    {
        if (clip != null && AudioSourceComponent != null)
        {
            AudioSourceComponent.PlayOneShot(clip);
        }
    }
}
