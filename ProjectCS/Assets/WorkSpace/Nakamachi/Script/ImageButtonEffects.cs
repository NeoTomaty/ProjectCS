//ImageButtonEffects.cs
//�쐬��:��������
//�ŏI�X�V��:2025/05/11
//[Log]
//05/11�@�����@�{�^���̊g��\��

using UnityEngine;
using UnityEngine.EventSystems;

//���̃N���X�́AUI�{�^���ɑ΂��ă}�E�X�I�[�o�[��I�����Ɋg�傷�鎋�o���ʂ�^���邽�߂̂���
//IPointerEnterHandler,IPointerExitHandler,ISelectHandler,IDeselectHandler���������邱�ƂŁA�}�E�X��L�[�{�[�h�ɂ��C���^���N�V�����ɑΉ�����
public class ImageButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    //���̃X�P�[��(�T�C�Y)��ێ����邽�߂̕ϐ�
    private Vector3 OriginalScale;

    //�Q�[���I�u�W�F�N�g�̌��̃X�P�[����ۑ�����
    void Start()
    {
        OriginalScale = transform.localScale;
    }

    //�}�E�X�J�[�\�����{�^����ɏ�����Ƃ��ɌĂ΂�鏈��
    public void OnPointerEnter(PointerEventData EventData)
    {
        //�{�^���̃X�P�[����1.2�{�Ɋg�債�āA���o�I�ɋ�������
        transform.localScale = OriginalScale * 1.2f;
    }

    //�}�E�X�J�[�\�����{�^�����痣�ꂽ�Ƃ��ɌĂ΂�鏈��
    public void OnPointerExit(PointerEventData EventData)
    {
        //�X�P�[�������ɖ߂��āA�ʏ�̃T�C�Y�ɖ߂�
        transform.localScale = OriginalScale;
    }

    //�{�^�����I�����ꂽ�Ƃ�(�L�[�{�[�h��Q�[���p�b�h�őI�����ꂽ�Ƃ��Ȃ�)
    public void OnSelect(BaseEventData EventData)
    {
        //�X�P�[����1.2�{�Ɋg�債�āA�I������Ă��邱�Ƃ����o�I�Ɏ���
        transform.localScale = OriginalScale * 1.2f;
    }

    //�{�^���̑I�����������ꂽ�Ƃ��ɌĂ΂�鏈��
    public void OnDeselect(BaseEventData EventData)
    {
        //�X�P�[�������ɖ߂��āA�ʏ�̃T�C�Y�ɖ߂�
        transform.localScale = OriginalScale;
    }
}
