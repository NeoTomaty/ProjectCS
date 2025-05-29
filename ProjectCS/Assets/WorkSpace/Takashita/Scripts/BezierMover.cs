using UnityEngine;

public class BezierMover : MonoBehaviour
{
    public CubicBezierCurve curve;  // 曲線スクリプト（ベジェ情報）
    [Range(0.1f, 10f)]
    public float moveDuration = 3f; // 曲線を移動し終えるまでの秒数



    private float timer = 0f;

    void Update()
    {
        //if (curve == null) return;

        //// 経過時間を更新
        //timer += Time.deltaTime;
        //float t = Mathf.Clamp01(timer / moveDuration); // 0～1 の範囲に正規化

        //// 曲線上の位置を取得してオブジェクトを移動
        //Vector3 position = curve.CalculateBezierPoint(t);
        //transform.position = position;
    }
}
