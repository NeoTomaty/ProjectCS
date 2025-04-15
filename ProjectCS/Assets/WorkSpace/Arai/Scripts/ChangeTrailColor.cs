//======================================================
// [ChangeTrailColor]
// 作成者：荒井修
// 最終更新日：04/12
// 
// [Log]
// 04/12　荒井　帯の色をスクリプトから変更できるように実装
// 04/12　荒井　プレイヤーの速度が閾値を跨いだ時に色が変わるように実装
//======================================================

using UnityEngine;

// Trail Rendererで作成した帯の色を変更するクラス
// プレイヤーにTrail Rendererと共にアタッチ
public class ChangeTrailColor : MonoBehaviour
{

    [SerializeField] private TrailRenderer TrailRenderer;           // トレイルレンダラーの参照
    [SerializeField] private PlayerSpeedManager PlayerSpeedManager; // プレイヤーの速度管理クラスの参照

    // 色の設定
    [SerializeField] private Color LowSpeedColor = Color.blue;      // 低速時
    [SerializeField] private Color MiddleSpeedColor = Color.yellow; // 中速時
    [SerializeField] private Color HighSpeedColor = Color.red;      // 高速時
    [SerializeField] private float AlphaValue = 1.0f;               // アルファ（透明度）値

    // 速度の閾値
    [SerializeField] private float LowToMiddleSpeedThreshold = 200.0f;  // 低速から中速
    [SerializeField] private float MiddleToHighSpeedThreshold = 400.0f; // 中速から高速

    // グラデーションの設定
    private Gradient gradient;              // グラデーションの参照
    private GradientColorKey[] colorKeys;   // 色のキーフレーム
    private GradientAlphaKey[] alphaKeys;   // アルファ（透明度）のキーフレーム

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (TrailRenderer == null) return;

        gradient = new Gradient();

        // 色のキーフレーム（時間と色）
        colorKeys = new GradientColorKey[2];
        colorKeys[0].color = LowSpeedColor; // 低速時の色
        colorKeys[0].time = 0.0f;           // 時間
        colorKeys[1].color = LowSpeedColor;
        colorKeys[1].time = 1.0f;

        // アルファ（透明度）のキーフレーム
        alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = AlphaValue;    // 不透明度
        alphaKeys[0].time = 0.0f;           // 時間
        alphaKeys[1].alpha = AlphaValue;
        alphaKeys[1].time = 1.0f;

        gradient.SetKeys(colorKeys, alphaKeys);

        // 適用
        TrailRenderer.colorGradient = gradient;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerSpeedManager == null) return;
        if (TrailRenderer == null) return;

        // プレイヤーの速度を取得
        float PlayerSpeed = PlayerSpeedManager.GetPlayerSpeed;

        // プレイヤーの速度に応じて色を変更
        if (PlayerSpeed < LowToMiddleSpeedThreshold)
        {
            colorKeys[0].color = LowSpeedColor;
            colorKeys[1].color = LowSpeedColor;
        }
        else if (PlayerSpeed < MiddleToHighSpeedThreshold)
        {
            colorKeys[0].color = MiddleSpeedColor;
            colorKeys[1].color = MiddleSpeedColor;
        }
        else
        {
            colorKeys[0].color = HighSpeedColor;
            colorKeys[1].color = HighSpeedColor;
        }

        gradient.SetKeys(colorKeys, alphaKeys);

        // 適用
        TrailRenderer.colorGradient = gradient;
    }
}
