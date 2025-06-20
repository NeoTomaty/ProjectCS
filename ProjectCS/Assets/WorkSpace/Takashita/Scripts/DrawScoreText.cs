//====================================================
// �X�N���v�g���FDrawScoreText
// �쐬�ҁF����
// ���e�F�X�R�A��\������
// �ŏI�X�V���F06/20
// 
// [Log]
// 06/20 ���� �X�N���v�g�쐬
//====================================================
using UnityEngine;

public class DrawScoreText : MonoBehaviour
{
    [SerializeField] private NumberTextManager[] NumberText = new NumberTextManager[5];
    public void SetScore(int score)
    {
        // 0�ȏ�99999�ȉ��ɐ���
        score = Mathf.Clamp(score, 0, 99999);

        // �������v�Z�i��: 1234 �� 4���j
        int digitCount = score == 0 ? 1 : (int)Mathf.Floor(Mathf.Log10(score)) + 1;

        for (int i = 0; i < NumberText.Length; i++)
        {
            // 5���ڂ��珇�ɏ����i�z��0��5���ځj
            int place = (int)Mathf.Pow(10, NumberText.Length - 1 - i);
            int digit = score / place;
            score %= place;

            NumberText[i].ChangeNumberSprite(digit);

            // ��ʌ��i�s�����j�͔������ɁA����ȊO�͕s������
            if (i < NumberText.Length - digitCount)
            {
                NumberText[i].SetAlpha(0.25f); // ������
            }
            else
            {
                NumberText[i].SetAlpha(1.0f); // �ʏ�
            }
        }
    }
}
