//======================================================
// [ChangeCameraFOV]
// 作成者：荒井修
// 最終更新日：04/12
// 
// [Log]
// 04/12　荒井　プレイヤーの速度に応じてカメラのFOVが変化するように実装
//======================================================

using UnityEngine;

public class ChangeCameraFOV : MonoBehaviour
{
    // プレイヤーを追跡するカメラにアタッチ

    [SerializeField] private Camera Camera;
    [SerializeField] private PlayerSpeedManager PlayerSpeedManager;

    // FOVの設定
    [SerializeField] private float MinFOV = 90.0f; // 最小
    [SerializeField] private float MaxFOV = 110.0f; // 最大
    [SerializeField] private float FOVLerpSpeed = 1.0f; // FOVの変化速度

    // 速度の設定
    [SerializeField] private float MinSpeed = 120.0f; // 最小速度
    [SerializeField] private float MaxSpeed = 500.0f; // 最大速度

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Camera.fieldOfView = MinFOV; // 初期値を設定
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera == null || PlayerSpeedManager == null) return;

        // プレイヤーの速度を取得
        float PlayerSpeed = PlayerSpeedManager.GetPlayerSpeed;

        // FOVの計算
        float FOV = Mathf.Lerp(MinFOV, MaxFOV, (PlayerSpeed - MinSpeed) / (MaxSpeed - MinSpeed));

        Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, FOV, Time.deltaTime * FOVLerpSpeed);
    }
}
