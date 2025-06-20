//====================================================
// スクリプト名：NumberTextManager
// 作成者：高下
// 内容：数値UIのテキストを管理するスクリプト
// 最終更新日：06/20
// 
// [Log]
// 06/20 高下 スクリプト作成
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
        // 範囲外の数字が渡された場合は 0 にフォールバック
        if (num > 9 || num < 0)
        {
            num = 0;
            Debug.Log("ChangeNumberSpriteに0から9以外が渡されたため0に変換");
        }

        NumText.text = num.ToString();
    }

    // 数字スプライトの透明度（アルファ値）を変更する
    /// <param name="alpha">透明度（0=完全透明, 1=不透明）</param>
    public void SetAlpha(float alpha)
    {
        Color color = NumText.color;
        color.a = alpha;
        NumText.color = color;
    }
}
