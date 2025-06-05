//====================================================
// スクリプト名：AutoSpeedDown
// 作成者：竹内
// 内容：時間経過によるプレイヤーの減速処理
// 最終更新日：06/03
// 
// [Log]
// 06/03 竹内 スクリプト作成 
//====================================================
using UnityEngine;

public class AutoSpeedDown : MonoBehaviour
{
    [SerializeField]
    [Header("何秒ごとに減速するのか")]
    private float time = 1f;                // 減速するまでの間隔

    [SerializeField]
    [Header("どれくらい減速するのか")]
    private float decelerationAmount = 1f; // 減速量

    // 時間
    private float timer = 0f;

    // プレイヤースピード管理スクリプト
    private PlayerSpeedManager speedManager;

    // 初期化
    private void Start()
    {
        // スクリプトを探す
        speedManager = GetComponent<PlayerSpeedManager>();
        if (speedManager == null)
        {
            Debug.LogError("PlayerSpeedManagerが見つかりません。");
        }
    }

    // 更新
    private void Update()
    {
        if (speedManager == null) return;

        // 毎フレーム時間計測
        timer += Time.deltaTime;

        // 設定した時間が経ったら
        if (timer >= time)
        {
            // 減速
            timer = 0f;
            speedManager.SetDecelerationValue(decelerationAmount);
        }
    }
}