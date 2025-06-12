//====================================================
// �X�N���v�g���FNumberSpriteManager
// �쐬�ҁF����
// ���e�F���lUI�̃X�v���C�g���Ǘ�����X�N���v�g
// �ŏI�X�V���F06/12
// 
// [Log]
// 06/12 ���� �X�N���v�g�쐬
//====================================================
using UnityEngine;
using UnityEngine.UI;

// UI��Ő����i0�`9�j���X�v���C�g�ŕ\���E���䂷��N���X
public class NumberSpriteManager : MonoBehaviour
{
    // 0�`9�̃X�v���C�g�i�C���X�y�N�^�[�Őݒ�j
    [SerializeField] private Sprite[] NumberSprites = new Sprite[10];

    // ����GameObject�ɃA�^�b�`���ꂽImage�R���|�[�l���g
    private Image NumberImage;

    // �����������FImage�R���|�[�l���g���擾
    void Start()
    {
        NumberImage = GetComponent<Image>();
    }

    // �w�肳�ꂽ���l�ɑΉ�����X�v���C�g��Image�ɐݒ�
    /// <param name="num">�\�����鐔���i0�`9�j</param>
    public void ChangeNumberSprite(int num)
    {
        // �͈͊O�̐������n���ꂽ�ꍇ�� 0 �Ƀt�H�[���o�b�N
        if (num > 9 || num < 0)
        {
            num = 0;
            Debug.Log("ChangeNumberSprite��0����9�ȊO���n���ꂽ����0�ɕϊ�");
        }

        // �Y���X�v���C�g��Image�ɐݒ�
        NumberImage.sprite = NumberSprites[num];
    }

    // �����X�v���C�g�̓����x�i�A���t�@�l�j��ύX����
    /// <param name="alpha">�����x�i0=���S����, 1=�s�����j</param>
    public void SetAlpha(float alpha)
    {
        Color color = NumberImage.color;
        color.a = alpha;
        NumberImage.color = color;
    }
}
