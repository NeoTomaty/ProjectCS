//======================================================
// [ChangeCameraFOV]
// 作成者：荒井修
// 最終更新日：05/02
// 
// [Log]
// 04/12　荒井　プレイヤーの速度に応じてカメラのFOVが変化するように実装
// 05/02　高下　視野角度を速度割合で変化できるように修正
//======================================================

using UnityEngine;

// プレイヤーを追跡するカメラにアタッチ
public class ChangeCameraFOV : MonoBehaviour
{

    [SerializeField] private Camera Camera;
    [SerializeField] private PlayerSpeedManager PlayerSpeedManager;

    // FOVの設定
    [Header("視野角度の設定")]
    [Tooltip("速度最小時の視野角度")]
    [SerializeField] private float MinFOV = 60.0f;  // 最小
    [Tooltip("速度最大時の視野角度")]
    [SerializeField] private float MaxFOV = 110.0f; // 最大
    [Tooltip("視野角度の補完速度")]
    [SerializeField] private float FOVLerpSpeed = 1.0f; // FOVの変化速度

    void Start()
    {
        Camera.fieldOfView = MinFOV; // 初期値を設定
    }

    void Update()
    {
        if (Camera == null || PlayerSpeedManager == null) return;

        // FOVの計算
        float FOV = Mathf.Lerp(MinFOV, MaxFOV, PlayerSpeedManager.GetSpeedRatio());

        // 視野角の適用
        Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, FOV, Time.deltaTime * FOVLerpSpeed);
    }
}
