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
#if UNITY_EDITOR
using UnityEditor;
#endif



[ExecuteAlways]
public class CubicBezierCurve : MonoBehaviour
{
    // 曲線の始点・終点および制御点
    public Transform StageObject1;       // ベジェ曲線の始点（P0）
    public Transform StageObject2;       // ベジェ曲線の終点（P3）
    public Transform ControlPoint1;      // 制御点1（P1）
    public Transform ControlPoint2;      // 制御点2（P2）

    [Range(2, 100)]
    public int resolution = 20;          // 曲線を構成する線分の数（精度）

    // シーンビューでGizmosを描画（ただし実行時には描画しない）
    private void OnDrawGizmos()
    {
        // 必要なTransformが揃っていない場合は描画しない
        if (StageObject1 == null || StageObject2 == null || ControlPoint1 == null || ControlPoint2 == null) return;

        // 実行中（Play中）は描画をスキップ
        if (Application.isPlaying) return;

        Gizmos.color = Color.green; // 曲線部分の色を設定

        Vector3 prev = StageObject1.position; // 始点を初期値とする
        for (int i = 1; i <= resolution; i++)
        {
            float t = i / (float)resolution;         // 0〜1の範囲で等間隔に分割
            Vector3 point = CalculateBezierPoint(t); // tに応じたベジェ点を計算
            Gizmos.DrawLine(prev, point);            // 前回の点と今回の点を結ぶ
            prev = point;                            // 今回の点を次回の起点に更新
        }

        // 制御点と始点・終点を結ぶ補助線（視覚的な操作ガイド）
        Gizmos.color = Color.red;
        Gizmos.DrawLine(StageObject1.position, ControlPoint1.position);
        Gizmos.DrawLine(ControlPoint2.position, StageObject2.position);
    }

    // Cubic Bezier（3次ベジェ曲線）でt位置の点を計算
    public Vector3 CalculateBezierPoint(float t)
    {
        Vector3 p0 = StageObject1.position;    // 始点
        Vector3 p1 = ControlPoint1.position;   // 制御点1
        Vector3 p2 = ControlPoint2.position;   // 制御点2
        Vector3 p3 = StageObject2.position;    // 終点

        float u = 1 - t;       // 補間係数の逆数
        float tt = t * t;      // t^2
        float uu = u * u;      // (1 - t)^2
        float uuu = uu * u;    // (1 - t)^3
        float ttt = tt * t;    // t^3

        // Cubic Bezier の式に基づいて曲線上の点を計算して返す
        return uuu * p0 +
               3 * uu * t * p1 +
               3 * u * tt * p2 +
               ttt * p3;
    }

    // Cubic Bezier 曲線の接線（進行方向ベクトル）を取得
    public Vector3 GetTangent(float t)
    {
        Vector3 p0 = StageObject1.position;
        Vector3 p1 = ControlPoint1.position;
        Vector3 p2 = ControlPoint2.position;
        Vector3 p3 = StageObject2.position;

        // Cubic Bezier の導関数（一次微分）
        return
            3 * Mathf.Pow(1 - t, 2) * (p1 - p0) +
            6 * (1 - t) * t * (p2 - p1) +
            3 * Mathf.Pow(t, 2) * (p3 - p2);
    }


    public void ConvertToLine()
    {
#if UNITY_EDITOR
        
        // 制御点を始点と終点の線上に揃える（直線になる）
        if (StageObject1 && StageObject2 && ControlPoint1 && ControlPoint2)
        {
            Undo.RecordObject(ControlPoint1, "Move Control Point 1");
            Undo.RecordObject(ControlPoint2, "Move Control Point 2");

            Vector3 start = StageObject1.position;
            Vector3 end = StageObject2.position;

            ControlPoint1.position = Vector3.Lerp(start, end, 1f / 3f);
            ControlPoint2.position = Vector3.Lerp(start, end, 2f / 3f);
        }
#endif
    }

}


