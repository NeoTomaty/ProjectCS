//ButtonEffects.cs
//�쐬��:��������
//�ŏI�X�V��:2025/05/08
//�A�^�b�`:StartButton�AExitButton�ɃA�^�b�`
//[Log]
//05/08�@�����@�{�^���Ƀ}�E�X�J�[�\�������킷�ƃe�L�X�g�̐F�ƕ�����1.2�{�g�傷�鏈��

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //�{�^���̃e�L�X�g���i�[����ϐ�
    public Text ButtonText;
    //���̃X�P�[�����i�[����ϐ�
    private Vector3 OriginalScale;
    //���̐F���i�[����ϐ�
    private Color OriginalColor;

    //�������������s���֐�
    void Start()
    {
        //�{�^���e�L�X�g�̌��̃X�P�[�����擾
        OriginalScale = ButtonText.transform.localScale;

        //�{�^���e�L�X�g�̌��̐F���擾
        OriginalColor = ButtonText.color;
    }

    //�}�E�X�|�C���^�[���{�^���ɓ������Ƃ��ɌĂяo�����֐�
    public void OnPointerEnter(PointerEventData EventData)
    {
        //�{�^���e�L�X�g�̃X�P�[����1.2�{�Ɋg��
        ButtonText.transform.localScale = OriginalScale * 1.2f;

        //�{�^���e�L�X�g�̐F��ԂɕύX
        ButtonText.color = Color.red;
    }

    //�}�E�X�|�C���^�[���{�^������o���Ƃ��ɌĂяo�����֐�
    public void OnPointerExit(PointerEventData EventData)
    {
        //�{�^���e�L�X�g�̃X�P�[�������ɖ߂�
        ButtonText.transform.localScale = OriginalScale;

        //�{�^���e�L�X�g�̐F�����ɖ߂�
        ButtonText.color = OriginalColor;
    }
}
