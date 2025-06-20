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
        //�{�^���̃X�P�[����1.2�{�Ɋg��
        transform.localScale = OriginalScale * 1.2f;
    }

    public void OnPointerExit(PointerEventData EventData)
    {
        transform.localScale = OriginalScale;
    }

    public void OnSelect(BaseEventData EventData)
    {
        transform.localScale = OriginalScale * 1.2f;
    }

    public void OnDeselect(BaseEventData EventData)
    {
        transform.localScale = OriginalScale;
    }
}
