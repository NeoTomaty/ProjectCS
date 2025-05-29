//====================================================
// スクリプト名：StageSelectMoveCamera
// 作成者：高下
// 内容：ステージセレクト画面のカメラの移動
// 最終更新日：05/29
// 
// [Log]
// 05/29 高下 スクリプト作成
//====================================================
using UnityEngine;

public class StageSelectMoveCamera : MonoBehaviour
{
    [SerializeField] private Transform PlayerObject;     // 追従対象（プレイヤー）
    [SerializeField] private Vector3 Offset = new Vector3(0f, 5f, -10f); // プレイヤーからの相対位置

    void LateUpdate()
    {
        if (PlayerObject == null) return;

        // プレイヤーの位置に対してオフセットを加え、カメラの位置を毎フレーム直接設定（回転は変えない）
        transform.position = PlayerObject.position + Offset;
    }
}
