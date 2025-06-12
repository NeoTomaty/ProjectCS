//====================================================
// スクリプト名：NumberSpriteManager
// 作成者：高下
// 内容：数値UIのスプライトを管理するスクリプト
// 最終更新日：06/12
// 
// [Log]
// 06/12 高下 スクリプト作成
//====================================================
using UnityEngine;
using UnityEngine.UI;

// UI上で数字（0～9）をスプライトで表示・制御するクラス
public class NumberSpriteManager : MonoBehaviour
{
    // 0～9のスプライト（インスペクターで設定）
    [SerializeField] private Sprite[] NumberSprites = new Sprite[10];

    // このGameObjectにアタッチされたImageコンポーネント
    private Image NumberImage;

    // 初期化処理：Imageコンポーネントを取得
    void Start()
    {
        NumberImage = GetComponent<Image>();
    }

    // 指定された数値に対応するスプライトをImageに設定
    /// <param name="num">表示する数字（0～9）</param>
    public void ChangeNumberSprite(int num)
    {
        // 範囲外の数字が渡された場合は 0 にフォールバック
        if (num > 9 || num < 0)
        {
            num = 0;
            Debug.Log("ChangeNumberSpriteに0から9以外が渡されたため0に変換");
        }

        // 該当スプライトをImageに設定
        NumberImage.sprite = NumberSprites[num];
    }

    // 数字スプライトの透明度（アルファ値）を変更する
    /// <param name="alpha">透明度（0=完全透明, 1=不透明）</param>
    public void SetAlpha(float alpha)
    {
        Color color = NumberImage.color;
        color.a = alpha;
        NumberImage.color = color;
    }
}
