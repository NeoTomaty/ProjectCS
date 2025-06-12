//====================================================
// �X�N���v�g���FDrawScore
// �쐬�ҁF����
// ���e�F�X�R�A��\������
// �ŏI�X�V���F06/12
// 
// [Log]
// 06/12 ���� �X�N���v�g�쐬
//====================================================
using UnityEngine;

public class DrawScore : MonoBehaviour
{
    [SerializeField] private NumberSpriteManager[] NumberSprite = new NumberSpriteManager[5];

    public void SetScore(int score)
    {
        // 0�ȏ�99999�ȉ��ɐ���
        score = Mathf.Clamp(score, 0, 99999);

        // �������v�Z�i��: 1234 �� 4���j
        int digitCount = score == 0 ? 1 : (int)Mathf.Floor(Mathf.Log10(score)) + 1;

        for (int i = 0; i < NumberSprite.Length; i++)
        {
            // 5���ڂ��珇�ɏ����i�z��0��5���ځj
            int place = (int)Mathf.Pow(10, NumberSprite.Length - 1 - i);
            int digit = score / place;
            score %= place;

            NumberSprite[i].ChangeNumberSprite(digit);

            // ��ʌ��i�s�����j�͔������ɁA����ȊO�͕s������
            if (i < NumberSprite.Length - digitCount)
            {
                NumberSprite[i].SetAlpha(0.25f); // ������
            }
            else
            {
                NumberSprite[i].SetAlpha(1.0f); // �ʏ�
            }
        }
    }
}
