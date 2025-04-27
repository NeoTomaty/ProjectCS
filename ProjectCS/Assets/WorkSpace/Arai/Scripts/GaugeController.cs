//======================================================
// [GaugeController]
// 作成者：荒井修
// 最終更新日：04/27
// 
// [Log]
// 04/26　荒井　ゲージが自動で増減するように実装
// 04/27　荒井　キー・ボタン入力で止められるように変更
// 04/27　荒井　ゲージの中心がオブジェクトの座標とずれないように修正
// 04/27　荒井　他スクリプトと合わせて動作するように修正
//======================================================
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

// ゲージが増減を繰り返す挙動のスクリプト
// GuageUIにアタッチ
public class GaugeController : MonoBehaviour
{
    // ゲージのUIとしてのパラメータ
    [SerializeField] private RectTransform GaugeRectTransform;

    // 親オブジェクト
    [SerializeField] private RectTransform ParentRectTransform;

    // ゲージ量
    private float GaugeValue = 0.0f;
    public float GetGaugeValue => GaugeValue;

    // ゲージの増減速度
    [SerializeField] private float GaugeSpeed = 0.5f;

    // ゲージの横幅
    private const float GaugeWidth = 100.0f;

    // ゲージの増減を止める入力
    [SerializeField] private KeyCode StopKey = KeyCode.Return;              // キー
    [SerializeField] private KeyCode StopBottun = KeyCode.JoystickButton1;  // ボタン

    private bool IsStop = false; // ゲージ増減を止めるフラグ

    // ゲージ増減のモード
    private enum GaugeMode
    {
        Increase,
        Decrease,
    }

    private GaugeMode CurrentGaugeMode = GaugeMode.Increase;

    public void SetGaugeValue(float Value)
    {
        GaugeValue = Value; // ゲージを初期化
    }

    public void Play()
    {
        GaugeValue = 0.0f; // ゲージを初期化
        CurrentGaugeMode = GaugeMode.Increase; // 増加モードに設定

        // ゲージを表示
        this.gameObject.SetActive(true); // ゲージを表示
        IsStop = false;
    }

    public void Stop()
    {
        // ゲージを非表示
        this.gameObject.SetActive(false); // ゲージを非表示
        Time.timeScale = 1.0f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsStop) return;

        if (Input.GetKeyDown(StopKey) || Input.GetKeyDown(StopBottun))
        {
            // ゲージの増減を止める
            IsStop = true;
            Stop();
        }

        // ゲージの増減処理
        switch (CurrentGaugeMode)
        {
            case GaugeMode.Increase:                        // 増加モード
                GaugeValue += Time.unscaledDeltaTime * GaugeSpeed;
                if (GaugeValue >= 1.0f)
                {
                    GaugeValue = 1.0f;
                    CurrentGaugeMode = GaugeMode.Decrease;
                }
                break;

            case GaugeMode.Decrease:                        // 減少モード
                GaugeValue -= Time.unscaledDeltaTime * GaugeSpeed;
                if (GaugeValue <= 0.0f)
                {
                    GaugeValue = 0.0f;
                    CurrentGaugeMode = GaugeMode.Increase;
                }
                break;
        }

        // ゲージ量に応じてUIの座標を変更
        // X座標のみ変化
        GaugeRectTransform.localPosition = new Vector3((1 - GaugeValue) * GaugeWidth, GaugeRectTransform.localPosition.y, GaugeRectTransform.localPosition.z);

        // 左端揃えで増減するように親の座標を調整
        // X座標のみ変化
        ParentRectTransform.localPosition = new Vector3((GaugeValue * GaugeWidth) - GaugeWidth, ParentRectTransform.localPosition.y, ParentRectTransform.localPosition.z);
    }
}
