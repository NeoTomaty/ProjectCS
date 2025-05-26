//====================================================
// スクリプト名：BallDistributorAutoCount
// 作成者：高下
// 内容：CubicBezierCurve に基づいて、実行時に曲線上に球体を間隔に応じて自動配置する
// 最終更新日：05/26
// 
// [Log]
// 05/26 高下 スクリプト作成
//====================================================
using UnityEngine;

public class BallDistributorAutoCount : MonoBehaviour
{
    // CubicBezierCurve から取得する制御点
    private Transform StartPoint;
    private Transform EndPoint;
    private Transform ControlPoint1;
    private Transform ControlPoint2;

    // 配置する球体のPrefab
    public GameObject SpherePrefab;

    // 球体同士の間隔（単位：メートル）
    public float Spacing = 0.5f;

    // 球体のサイズ（スケール）
    public float SphereScale = 0.2f;

    // 曲線の長さを計算する際に使用する分割数（数が多いほど精度が上がる）
    public int CurveSampleResolution = 100;

    // 実行時に曲線長を計算し、球体を配置
    private void Start()
    {
        // 同一オブジェクトにアタッチされた CubicBezierCurve から制御点を取得
        StartPoint = GetComponent<CubicBezierCurve>().StageObject1.transform;
        EndPoint = GetComponent<CubicBezierCurve>().StageObject2.transform;
        ControlPoint1 = GetComponent<CubicBezierCurve>().ControlPoint1.transform;
        ControlPoint2 = GetComponent<CubicBezierCurve>().ControlPoint2.transform;

        // 必須の設定が揃っていなければ処理を中断
        if (StartPoint == null || EndPoint == null || ControlPoint1 == null || ControlPoint2 == null || SpherePrefab == null)
            return;

        // 曲線の長さを近似的に計算
        float curveLength = EstimateCurveLength();

        // 線の長さに基づいて配置する球の数を決定（最低2つ）
        int numberOfBalls = Mathf.Max(2, Mathf.RoundToInt(curveLength / Spacing));

        // 計算した数だけ球体を配置
        for (int i = 0; i < numberOfBalls; i++)
        {
            float t = i / (float)(numberOfBalls - 1); // 0〜1の範囲で等間隔に分割
            Vector3 pos = CalculateBezierPoint(t);    // 曲線上の位置を取得
            GameObject ball = Instantiate(SpherePrefab, pos, Quaternion.identity, transform); // 球体を生成して親に設定
            ball.transform.localScale = Vector3.one * SphereScale; // サイズを設定
        }
    }

    // 3次ベジェ曲線に基づいて、指定された t の位置の点を返す
    private Vector3 CalculateBezierPoint(float t)
    {
        Vector3 p0 = StartPoint.position;      // 始点
        Vector3 p1 = ControlPoint1.position;   // 制御点1
        Vector3 p2 = ControlPoint2.position;   // 制御点2
        Vector3 p3 = EndPoint.position;        // 終点

        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        // ベジェ曲線の式
        return uuu * p0 +
               3 * uu * t * p1 +
               3 * u * tt * p2 +
               ttt * p3;
    }

    // 曲線を多数の直線で近似して、全体の長さを求める
    private float EstimateCurveLength()
    {
        float length = 0f;
        Vector3 prev = CalculateBezierPoint(0f); // 初期点

        // resolutionの数だけ曲線をサンプリングして、区間ごとの距離を加算
        for (int i = 1; i <= CurveSampleResolution; i++)
        {
            float t = i / (float)CurveSampleResolution;
            Vector3 current = CalculateBezierPoint(t);
            length += Vector3.Distance(prev, current); // 前の点との距離を加算
            prev = current; // 現在の点を次回の前の点として保存
        }

        return length;
    }
}
