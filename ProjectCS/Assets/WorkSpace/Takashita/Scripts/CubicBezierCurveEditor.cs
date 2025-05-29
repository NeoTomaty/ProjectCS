//====================================================
// スクリプト名：CubicBezierCurveEditor
// 作成者：高下
// 内容：Sceneビューでベジェ曲線の制御点（ControlPoint1・2）をマウスで直接動かせるようにする
// 最終更新日：05/26
// 
// [Log]
// 05/26 高下 スクリプト作成
//====================================================
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

// エディタ上で DynamicBezierCurve の SceneGUI を拡張
[CustomEditor(typeof(CubicBezierCurve))]
public class DynamicBezierCurveEditor : Editor
{
    private void OnSceneGUI()
    {
        CubicBezierCurve curve = (CubicBezierCurve)target;

        // 安全チェック（anchorとcontrolの数が正しいか）
        if (curve.anchorPoints.Count < 2 || curve.controlPoints.Count != (curve.anchorPoints.Count - 1) * 2)
            return;

        // 制御点を一つずつドラッグできるようにする
        EditorGUI.BeginChangeCheck();

        for (int i = 0; i < curve.controlPoints.Count; i++)
        {
            Transform cp = curve.controlPoints[i];
            if (cp == null) continue;

            // ハンドルを表示
            Vector3 newPos = Handles.PositionHandle(cp.position, Quaternion.identity);

            if (newPos != cp.position)
            {
                Undo.RecordObject(cp, $"Move Control Point {i}");
                cp.position = newPos;
            }
        }

        // アンカーポイントも編集可能にしたい場合はこちらを追加（任意）
        for (int i = 0; i < curve.anchorPoints.Count; i++)
        {
            Transform ap = curve.anchorPoints[i];
            if (ap == null) continue;

            Vector3 newPos = Handles.PositionHandle(ap.position, Quaternion.identity);
            if (newPos != ap.position)
            {
                Undo.RecordObject(ap, $"Move Anchor Point {i}");
                ap.position = newPos;
            }
        }

        if (EditorGUI.EndChangeCheck())
        {
            // Sceneを更新
            EditorUtility.SetDirty(curve);
        }
    }
}
#endif