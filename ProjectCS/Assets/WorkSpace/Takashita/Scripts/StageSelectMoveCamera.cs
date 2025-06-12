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

// ステージ選択画面において、カメラを2つの視点間で補間移動・回転させる制御クラス
public class StageSelectMoveCamera : MonoBehaviour
{
    // カメラが追従する対象（プレイヤーなど）
    public Transform PlayerObject;

    [Header("Offset & Rotation 1 (通常時)")]
    // 通常時のカメラ位置オフセット（プレイヤーからの相対位置）
    public Vector3 Offset1 = new Vector3(0, 40, -60);
    // 通常時のカメラ回転（角度）
    public Vector3 RotationEuler1 = new Vector3(20f, 0f, 0f);

    [Header("Offset & Rotation 2 (切替時)")]
    // 切替時のカメラ位置オフセット（例：選択ステージの近くへズーム）
    public Vector3 Offset2 = new Vector3(-5, 10, -14);
    // 切替時のカメラ回転（ステージを斜めに見るなど）
    public Vector3 RotationEuler2 = new Vector3(0f, 45f, 0f);

    [Header("設定")]
    // 補間スピード（大きいほど速く補間される）
    public float LerpSpeed = 3.0f;

    // 補間係数（0=通常, 1=切替）
    private float t = 0f;

    // 現在どちらの状態に向かっているか（false=通常, true=切替）
    private bool IsSwitched = false;

    // 現在補間中かどうか
    private bool IsInterpolating = false;

    void Update()
    {
        // t を補間（IsSwitched に応じて 0→1 or 1→0）
        t = Mathf.MoveTowards(t, IsSwitched ? 1f : 0f, Time.deltaTime * LerpSpeed);

        // カメラ位置の補間（プレイヤー位置 + オフセット）
        Vector3 offset = Vector3.Lerp(Offset1, Offset2, t);
        transform.position = PlayerObject.position + offset;

        // カメラ回転の補間
        Quaternion rotation1 = Quaternion.Euler(RotationEuler1);
        Quaternion rotation2 = Quaternion.Euler(RotationEuler2);
        transform.rotation = Quaternion.Slerp(rotation1, rotation2, t);

        // 補間が完了したらフラグを下ろす
        if (IsInterpolating && (t == 0f || t == 1f))
        {
            IsInterpolating = false;
        }
    }

    // 現在カメラが補間中かどうかを取得する
    public bool GetIsInterpolating()
    {
        return IsInterpolating;
    }

    // カメラ状態を切り替える（true:切替状態へ、false:通常状態へ）
    public void SetIsSwitched(bool isSwitched)
    {
        IsSwitched = isSwitched;
        IsInterpolating = true;
    }

    // 現在の状態（通常 or 切替）を取得する
    public bool GetIsSwitched()
    {
        return IsSwitched;
    }
}