//======================================================
// [SpeedLineController]
// 作成者：荒井修
// 最終更新日：04/16
// 
// [Log]
// 04/13　荒井　プレイヤーの速度に応じて集中線のスケールが変化するように実装
// 04/16　荒井　テクスチャのUVアニメーションを実装
//======================================================

using UnityEngine;
using UnityEngine.UI;

// 集中線オブジェクトにアタッチ
public class SpeedLineController : MonoBehaviour
{
    [SerializeField] private PlayerSpeedManager PlayerSpeedManager;

    // UVアニメーション設定
    [SerializeField] private RawImage SpeedLineImage;           // 集中線のRawImageコンポーネント
    [SerializeField] private int TotalFrame = 6;                // 総フレーム数
    [SerializeField] private float AnimationDuration = 0.1f;    // アニメーション進行ペース
    private int CurrentFrame = 0; // 現在のフレーム数
    private float Timer = 0.0f;

    // スケールの設定
    [SerializeField] private float LowSpeedScale = 1.5f;      // 低速時
    [SerializeField] private float HighSpeedScale = 0.8f;     // 高速時
    [SerializeField] private float ScaleLerpSpeed = 1.0f;     // スケールの変化速度

    // 速度の設定
    [SerializeField] private float PlayerLowSpeed = 120.0f;         // 最小速度
    [SerializeField] private float PlayerHighSpeed = 500.0f;        // 最大速度

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.localScale = new Vector3(LowSpeedScale, LowSpeedScale, 1.0f); // 初期値を設定

        if (SpeedLineImage == null) return;
        Rect rect = SpeedLineImage.uvRect;
        rect.height = 1.0f / TotalFrame;
        SpeedLineImage.uvRect = rect;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerSpeedManager == null) return;

        // プレイヤーの速度を取得
        float PlayerSpeed = PlayerSpeedManager.GetPlayerSpeed;

        // スケールの計算
        float Scale = Mathf.Lerp(LowSpeedScale, HighSpeedScale, (PlayerSpeed - PlayerLowSpeed) / (PlayerHighSpeed - PlayerLowSpeed));

        // なめらかに変化させる
        Scale = Mathf.Lerp(transform.localScale.x, Scale, Time.deltaTime * ScaleLerpSpeed);

        // スケールを設定
        transform.localScale = new Vector3(Scale, Scale, 1.0f);


        // UVアニメーション
        if (SpeedLineImage == null) return;
        Timer += Time.deltaTime;
        // 一定時間ごとにアニメーション進行
        if (Timer >= AnimationDuration)
        {
            Timer = 0.0f;   // タイマーリセット
            CurrentFrame++; //アニメーションフレーム加算
            CurrentFrame %= TotalFrame;

            // 表示箇所切り換え
            Rect rect = SpeedLineImage.uvRect;
            rect.y = rect.height * CurrentFrame;
            SpeedLineImage.uvRect = rect;
        }
    }
}
