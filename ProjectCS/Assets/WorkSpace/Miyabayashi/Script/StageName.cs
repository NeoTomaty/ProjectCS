//======================================================
// StageNameスクリプト
// 作成者：宮林
// 最終更新日：5/5
// 
// [Log]5/5 宮林　ステージ名表示
//======================================================
using UnityEngine;
using UnityEngine.UI;

public class MessageDisplay : MonoBehaviour
{
    [SerializeField] private Text StageName;

    // 外部から呼び出して文字列を表示するメソッド
    public void ShowMessage(string message)
    {
        if (StageName != null)
        {
            StageName.text = message;
        }
    }
}
