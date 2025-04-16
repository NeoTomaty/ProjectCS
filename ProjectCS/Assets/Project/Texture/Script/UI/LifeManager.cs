//====================================================
// スクリプト名：LifeManager
// 作成者：竹内
// 内容：ライフ管理
// 最終更新日：04/08
// 
// [Log]
// 04/08 竹内 スクリプト作成
//====================================================
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    public int Life = 5;        // ライフ
    public Text LifeText;       // テキストUI

    void Start()
    {
        // テキストの更新
        UpdateLifeText();
    }

    public void DecreaseLife()
    {
        // ライフは０いかにならない
        Life = Mathf.Max(Life - 1, 0);
        // テキストの更新
        UpdateLifeText();
    }

    void UpdateLifeText()
    {
        // ライフ値の更新
        LifeText.text = "Life: " + Life.ToString();
    }
}
