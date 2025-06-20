//====================================================
// スクリプト名：DrawScoreText
// 作成者：高下
// 内容：スコアを表示する
// 最終更新日：06/20
// 
// [Log]
// 06/20 高下 スクリプト作成
//====================================================
using UnityEngine;

public class DrawScoreText : MonoBehaviour
{
    [SerializeField] private NumberTextManager[] NumberText = new NumberTextManager[5];
    public void SetScore(int score)
    {
        // 0以上99999以下に制限
        score = Mathf.Clamp(score, 0, 99999);

        // 桁数を計算（例: 1234 → 4桁）
        int digitCount = score == 0 ? 1 : (int)Mathf.Floor(Mathf.Log10(score)) + 1;

        for (int i = 0; i < NumberText.Length; i++)
        {
            // 5桁目から順に処理（配列0が5桁目）
            int place = (int)Mathf.Pow(10, NumberText.Length - 1 - i);
            int digit = score / place;
            score %= place;

            NumberText[i].ChangeNumberSprite(digit);

            // 上位桁（不足分）は半透明に、それ以外は不透明に
            if (i < NumberText.Length - digitCount)
            {
                NumberText[i].SetAlpha(0.25f); // 半透明
            }
            else
            {
                NumberText[i].SetAlpha(1.0f); // 通常
            }
        }
    }
}
