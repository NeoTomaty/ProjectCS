//======================================================
// SpeedGaugeUIスクリプト
// 作成者：宮林
// 最終更新日：5/7
// 
// [Log]5/7 宮林　スピードゲージを動かす処理
//      6/6 宮林　ゲージの増減の補間を追加
//======================================================
using UnityEngine;
using UnityEngine.UI;

public class SpeedGaugeUI : MonoBehaviour
{
    [SerializeField] private PlayerSpeedManager playerSpeedManager;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Text speedText;

    [SerializeField] private float smoothSpeed = 5f; // 補間の速さ（大きいほど速く追従）

    private float currentGaugeValue = 0f;

    void Start()
    {
        // 初期化（現在の速度に合わせておく）
        currentGaugeValue = playerSpeedManager.GetSpeedRatio();
        speedSlider.value = currentGaugeValue;
    }

    void Update()
    {
        float targetValue = playerSpeedManager.GetSpeedRatio();

        // スムーズに補間する
        currentGaugeValue = Mathf.Lerp(currentGaugeValue, targetValue, Time.deltaTime * smoothSpeed);

        // ゲージ更新
        speedSlider.value = currentGaugeValue;

        // 数値表示
        float currentSpeed = playerSpeedManager.GetPlayerSpeed;
        speedText.text = "Speed: " + Mathf.RoundToInt(currentSpeed).ToString();
    }
}