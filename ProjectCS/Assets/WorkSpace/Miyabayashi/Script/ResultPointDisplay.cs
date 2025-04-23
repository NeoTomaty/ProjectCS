//======================================================
// ResultpointDisplay スクリプト
// 作成者：宮林
// 最終更新日：4/23
// 
// [Log]4/23 宮林　ポイント表示を追加
//                 
//======================================================

using UnityEngine;
using UnityEngine.UI;

public class ResultPointDisplay : MonoBehaviour
{
    public Text pointText;

    // 仮で外部から受け取るポイント値
    public int receivedPoint =200;

    void Start()
    {
        // ポイントがまだ渡されていなければデフォルト表示
        if (receivedPoint < 0)
        {
            pointText.text = "ポイント: 取得中...";
        }
        else
        {
            DisplayPoint();
        }
    }

    // 他スクリプトから呼び出せるようにしておく
    public void SetPoint(int point)
    {
        receivedPoint = point;
        DisplayPoint();
    }

    void DisplayPoint()
    {
        pointText.text = "ポイント: " + receivedPoint.ToString();
    }
}
