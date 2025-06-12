//====================================================
// スクリプト名：BezierMover
// 作成者：高下
// 内容：曲線上にプレイヤーを動かせる関数
// 最終更新日：06/01
// 
// [Log]
// 06/01 高下 スクリプト作成
//====================================================
using UnityEngine;

// ベジェ曲線に沿ってオブジェクトを移動・回転させるコンポーネント
public class BezierMover : MonoBehaviour
{
    // ベジェ曲線を計算するスクリプト（必須）
    [SerializeField] private CubicBezierCurve curve;

    // 移動にかかる合計時間（秒）
    [Range(0.1f, 10f)]
    [SerializeField] private float moveDuration = 3f;

    // 回転させる見た目モデル（このオブジェクトを回転）
    [SerializeField] private Transform RotatingModel;

    // 半径（転がり角度の計算に使用）
    [SerializeField] private float radius = 0.01f;

    // 移動方向（逆再生かどうか）
    private bool IsReverse = false;

    // 移動用のタイマー
    private float timer = 0f;

    // 現在移動中かどうかのフラグ
    private bool isMoving = false;

    // 前フレームの位置（進行方向を求めるために使用）
    private Vector3 previousPosition;

    void Update()
    {
        // 移動が有効でなければ何もしない
        if (!isMoving || curve == null) return;

        // 時間を進める（逆再生時は減算）
        timer += IsReverse ? -Time.deltaTime : Time.deltaTime;

        // 時間を 0～1 に正規化
        float t = Mathf.Clamp01(timer / moveDuration);

        // 現在位置をベジェ曲線上から取得して移動
        Vector3 currentPosition = curve.CalculateBezierPoint(t);
        transform.position = currentPosition;

        // 進行方向のベクトル（前フレームとの差分）
        Vector3 delta = currentPosition - previousPosition;
        float distance = delta.magnitude;

        // 向きの更新（進行方向にY軸を基準に回転）
        if (delta != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(delta.normalized, Vector3.up);
            RotatingModel.rotation = lookRotation;
        }

        // 回転運動の演出（地面を転がるように回転）
        if (distance > 0f && radius > 0f)
        {
            // 進行方向と上方向の外積で回転軸を生成
            Vector3 rotationAxis = Vector3.Cross(delta.normalized, Vector3.up);

            // 回転角度（距離 ÷ 半径 → ラジアン → 度に変換）
            float angleDeg = Mathf.Rad2Deg * (distance / radius);

            // モデルを回転
            RotatingModel.Rotate(rotationAxis, angleDeg, Space.World);
        }

        // 次フレームの差分計算のために現在位置を保存
        previousPosition = currentPosition;

        // 移動完了判定（時間オーバーまたは逆再生で時間ゼロ）
        if ((!IsReverse && timer >= moveDuration) || (IsReverse && timer <= 0f))
        {
            isMoving = false;
        }
    }

    // ベジェ曲線移動を開始する（開始方向と対象曲線を指定）
    /// <param name="isReverse">逆再生するか</param>
    /// <param name="cubicBezierCurve">移動対象のベジェ曲線</param>
    public void StartMove(bool isReverse, CubicBezierCurve cubicBezierCurve)
    {
        IsReverse = isReverse;
        curve = cubicBezierCurve;
        isMoving = true;

        // 初回移動ならタイマーを開始点または終了点に設定
        if (timer <= 0f || timer >= moveDuration)
        {
            timer = isReverse ? moveDuration : 0f;
        }
    }

    // 即時的に位置だけ設定したいときに使用
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    // 移動中かどうかを取得
    public bool GetIsMoving()
    {
        return isMoving;
    }
}
