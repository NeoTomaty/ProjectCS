//======================================================
// [TriangleGaugeController]
// 作成者：荒井修
// 最終更新日：04/26
// 
// [Log]
// 04/26　荒井　ゲージが自動で増減するように実装
//======================================================
using UnityEngine;

// ゲージが増減を繰り返す挙動のスクリプト
// どのオブジェクトにアタッチしてもOK
public class TriangleGaugeController : MonoBehaviour
{
    // ゲージのUIとしてのパラメータ
    [SerializeField] private RectTransform GaugeRectTransform;

    // 親オブジェクト
    [SerializeField] private RectTransform ParentRectTransform;

    // ゲージ量
    private float GaugeValue = 0.0f;

    // ゲージの増減速度
    [SerializeField] private float GaugeSpeed = 0.5f;

    // ゲージの横幅
    private const float GaugeWidth = 100.0f;

    // ゲージ増減のモード
    private enum GaugeMode
    {
        Increase,
        Decrease,
    }

    private GaugeMode CurrentGaugeMode = GaugeMode.Increase;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ゲージの増減処理
        switch (CurrentGaugeMode)
        {
            case GaugeMode.Increase:                        // 増加モード
                GaugeValue += Time.deltaTime * GaugeSpeed;
                if (GaugeValue >= 1.0f)
                {
                    GaugeValue = 1.0f;
                    CurrentGaugeMode = GaugeMode.Decrease;
                }
                break;

            case GaugeMode.Decrease:                        // 減少モード
                GaugeValue -= Time.deltaTime * GaugeSpeed;
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
        ParentRectTransform.localPosition = new Vector3(GaugeValue * GaugeWidth, ParentRectTransform.localPosition.y, ParentRectTransform.localPosition.z);
    }
}
