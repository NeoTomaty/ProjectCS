//====================================================
// �X�N���v�g���FNumberTextManager
// �쐬�ҁF����
// ���e�F���lUI�̃e�L�X�g���Ǘ�����X�N���v�g
// �ŏI�X�V���F06/20
// 
// [Log]
// 06/20 ���� �X�N���v�g�쐬
//====================================================
using UnityEngine;
using UnityEngine.UI;

public class NumberTextManager : MonoBehaviour
{
    private Text NumText;

    void Start()
    {
        NumText = GetComponent<Text>();
    }
    public void ChangeNumberSprite(int num)
    {
        // �͈͊O�̐������n���ꂽ�ꍇ�� 0 �Ƀt�H�[���o�b�N
        if (num > 9 || num < 0)
        {
            num = 0;
            Debug.Log("ChangeNumberSprite��0����9�ȊO���n���ꂽ����0�ɕϊ�");
        }

        NumText.text = num.ToString();
    }

    // �����X�v���C�g�̓����x�i�A���t�@�l�j��ύX����
    /// <param name="alpha">�����x�i0=���S����, 1=�s�����j</param>
    public void SetAlpha(float alpha)
    {
        Color color = NumText.color;
        color.a = alpha;
        NumText.color = color;
    }
}
