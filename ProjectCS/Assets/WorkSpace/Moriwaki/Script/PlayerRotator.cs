//======================================================
// [PlayerRotator]
// 作成者：森脇
// 最終更新日：05/29
//
// [Log]
// 05/29　森脇 回転
//======================================================

using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    [SerializeField] private PlayerSpeedManager speedManager; // PlayerSpeedManagerへの参照（インスペクターで設定）
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // 回転軸（デフォルトはY軸）
    [SerializeField] private float rotationMultiplier = 1.0f; // 回転スピードの倍率

    private void Update()
    {
        if (speedManager == null) return;

        // PlayerSpeedを取得
        float speed = speedManager.GetPlayerSpeed;

        // 回転量を計算
        float rotationAmount = speed * rotationMultiplier * Time.deltaTime;

        // 回転
        transform.Rotate(rotationAxis, rotationAmount);
    }
}