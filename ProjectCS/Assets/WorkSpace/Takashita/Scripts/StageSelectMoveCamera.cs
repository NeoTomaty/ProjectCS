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
    public Transform PlayerObject;

    [Header("Offset & Rotation 1 (通常時)")]
    public Vector3 Offset1 = new Vector3(0, 40, -60);
    public Vector3 RotationEuler1 = new Vector3(20f, 0f, 0f);  // プレイヤーを見る回転など

    [Header("Offset & Rotation 2 (切替時)")]
    public Vector3 Offset2 = new Vector3(-5, 10, -14);
    public Vector3 RotationEuler2 = new Vector3(0f, 45f, 0f);

    [Header("設定")]
    public float LerpSpeed = 3.0f;

    private float t = 0f;
    private bool IsSwitched = false;
    private bool IsInterpolating = false;

    void Update()
    {
        // 補間係数更新
        t = Mathf.MoveTowards(t, IsSwitched ? 1f : 0f, Time.deltaTime * LerpSpeed);

        // オフセット補完（位置）
        Vector3 offset = Vector3.Lerp(Offset1, Offset2, t);
        transform.position = PlayerObject.position + offset;

        // 回転補完
        Quaternion rotation1 = Quaternion.Euler(RotationEuler1);
        Quaternion rotation2 = Quaternion.Euler(RotationEuler2);
        transform.rotation = Quaternion.Slerp(rotation1, rotation2, t);

        if (IsInterpolating && (t == 0f || t == 1f))
        {
            IsInterpolating = false;
        }
    }

    public bool GetIsInterpolating()
    {
        return IsInterpolating;
    }

    public void SetIsSwitched(bool isSwitched)
    {
        IsSwitched = isSwitched;
        IsInterpolating = true;
    }

    public bool GetIsSwitched()
    {
        return IsSwitched;
    }
}
