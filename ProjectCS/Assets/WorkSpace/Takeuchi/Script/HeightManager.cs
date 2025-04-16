//====================================================
// スクリプト名：HeightManager
// 作成者：竹内
// 内容：Y座標を「Height：○○m」形式でUI表示
// 最終更新日：04/16
// [Log]
// 04/16 竹内 スクリプト作成
//====================================================
using UnityEngine;
using UnityEngine.UI;

public class HeightManager : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private Transform Object;     // 表示するオブジェクトのTransform
    [SerializeField] private Text HeightText;      // 表示するUIテキスト（Legacy UI）

    [Header("設定")]
    [SerializeField] private string Prefix = "Height：";  // 表示する前置きテキスト
    [SerializeField] private string Unit = "m";           // 単位（mやkmなど）

    private float BaseHeight = 0f; // シーン開始時のY座標を保持

    void Start()
    {
        if (Object == null)
        {
            Debug.LogError("プレイヤーが未設定です。");
            enabled = false;
            return;
        }

        // シーン開始時の高さを基準にする
        BaseHeight = Object.position.y;
    }

    void Update()
    {
        if (HeightText == null) return;

        float currentHeight = Object.position.y;
        float relativeHeight = currentHeight - BaseHeight;

        // 小数点1位までの相対高度表示（符号付き）
        HeightText.text = $"{Prefix} {relativeHeight:0.0} {Unit}";
    }

}
