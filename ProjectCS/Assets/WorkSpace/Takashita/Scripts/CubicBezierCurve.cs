//====================================================
// スクリプト名：CubicBezierCurve
// 作成者：高下
// 内容：2点間に滑らかな曲線を描画する
// 最終更新日：05/26
// 
// [Log]
// 05/26 高下 スクリプト作成
//====================================================
using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class CubicBezierCurve : MonoBehaviour
{
    [Header("アンカーポイント（始点・終点含む）")]
    public List<Transform> anchorPoints = new List<Transform>(); // P0〜Pn

    [Header("制御点（各セグメントごとに2つ）")]
    public List<Transform> controlPoints = new List<Transform>(); // (N-1)×2個

    [Range(2, 100)]
    public int resolution = 20;

    private void OnValidate()
    {
        AdjustControlPointList();
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying) return;
        if (anchorPoints.Count < 2 || controlPoints.Count != (anchorPoints.Count - 1) * 2) return;

        Gizmos.color = Color.green;

        for (int i = 0; i < anchorPoints.Count - 1; i++)
        {
            Transform p0 = anchorPoints[i];
            Transform p1 = anchorPoints[i + 1];
            Transform cp1 = controlPoints[i * 2];
            Transform cp2 = controlPoints[i * 2 + 1];

            if (p0 == null || p1 == null || cp1 == null || cp2 == null)
                continue;

            Vector3 prev = p0.position;
            for (int j = 1; j <= resolution; j++)
            {
                float t = j / (float)resolution;
                Vector3 point = CalculateBezierPoint(p0.position, cp1.position, cp2.position, p1.position, t);
                Gizmos.DrawLine(prev, point);
                prev = point;
            }

            // 補助線
            Gizmos.color = Color.red;
            Gizmos.DrawLine(p0.position, cp1.position);
            Gizmos.DrawLine(cp2.position, p1.position);
            Gizmos.color = Color.green;
        }
    }

    // Cubic Bezier
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

    // アンカーポイントの数に応じて制御点リストの長さを調整
    private void AdjustControlPointList()
    {
        int targetCount = (anchorPoints.Count - 1) * 2;
        if (targetCount < 0) targetCount = 0;

        while (controlPoints.Count < targetCount)
            controlPoints.Add(null);

        while (controlPoints.Count > targetCount)
            controlPoints.RemoveAt(controlPoints.Count - 1);
    }
}
