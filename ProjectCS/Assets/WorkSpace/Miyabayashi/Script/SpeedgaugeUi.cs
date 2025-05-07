//======================================================
// SpeedGaugeUIスクリプト
// 作成者：宮林
// 最終更新日：5/7
// 
// [Log]5/7 宮林　スピードゲージを動かす処理
//======================================================
using UnityEngine;
using UnityEngine.UI;

public class SpeedGaugeUI : MonoBehaviour
{
    [SerializeField] private PlayerSpeedManager playerSpeedManager;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Text speedText; // ← UI Text をここに割り当てる

    void Update()
    {
        // ゲージ更新
        speedSlider.value = playerSpeedManager.GetSpeedRatio();

        // 数値を文字列で表示
        float currentSpeed = playerSpeedManager.GetPlayerSpeed;
        speedText.text = "Speed: " + Mathf.RoundToInt(currentSpeed).ToString();
    }
}
