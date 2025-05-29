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
using System.Collections.Generic;

[RequireComponent(typeof(CubicBezierCurve))]
public class BallDistributorAutoCount : MonoBehaviour
{
    public GameObject SpherePrefab;
    public float Spacing = 0.5f;
    public float SphereScale = 0.2f;
    public float BigSphereScale = 0.4f;
    public int CurveSampleResolution = 100;

    private CubicBezierCurve bezierCurve;

    private void Start()
    {
        bezierCurve = GetComponent<CubicBezierCurve>();

        // データが不正なら終了
        if (bezierCurve.anchorPoints.Count < 2 ||
            bezierCurve.controlPoints.Count != (bezierCurve.anchorPoints.Count - 1) * 2 ||
            SpherePrefab == null)
            return;

        // 各アンカーポイントに大きい球体を配置
        foreach (Transform anchor in bezierCurve.anchorPoints)
        {
            if (anchor == null) continue;
            CreateSphere(anchor.position, BigSphereScale);
        }

        // 各セグメントごとにベジェ曲線を分割して小さい球体を配置
        for (int i = 0; i < bezierCurve.anchorPoints.Count - 1; i++)
        {
            Transform p0 = bezierCurve.anchorPoints[i];
            Transform p1 = bezierCurve.anchorPoints[i + 1];
            Transform c1 = bezierCurve.controlPoints[i * 2];
            Transform c2 = bezierCurve.controlPoints[i * 2 + 1];

            if (p0 == null || p1 == null || c1 == null || c2 == null)
                continue;

            float segmentLength = EstimateCurveLength(p0.position, c1.position, c2.position, p1.position);
            int count = Mathf.Max(2, Mathf.RoundToInt(segmentLength / Spacing));

            for (int j = 1; j < count - 1; j++) // 始点終点を除く
            {
                float t = j / (float)(count - 1);
                Vector3 pos = CalculateBezierPoint(p0.position, c1.position, c2.position, p1.position, t);
                CreateSphere(pos, SphereScale);
            }
        }
    }

    private void CreateSphere(Vector3 position, float scale)
    {
        GameObject ball = Instantiate(SpherePrefab, position, Quaternion.identity, transform);
        ball.transform.localScale = Vector3.one * scale;
    }

    // Cubic Bezierの点を求める
    private Vector3 CalculateBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        return uuu * p0 +
               3 * uu * t * p1 +
               3 * u * tt * p2 +
               ttt * p3;
    }

    // 曲線長を近似で算出
    private float EstimateCurveLength(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float length = 0f;
        Vector3 prev = CalculateBezierPoint(p0, p1, p2, p3, 0f);

        for (int i = 1; i <= CurveSampleResolution; i++)
        {
            float t = i / (float)CurveSampleResolution;
            Vector3 current = CalculateBezierPoint(p0, p1, p2, p3, t);
            length += Vector3.Distance(prev, current);
            prev = current;
        }

        return length;
    }
}
