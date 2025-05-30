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

// CubicBezierCurve を対象としたカスタムエディタ（シーンビュー用）
[CustomEditor(typeof(CubicBezierCurve))]
public class CubicBezierCurveEditor : Editor
{
    // シーンビューにカスタムUIを表示するメソッド
    private void OnSceneGUI()
    {
        // 編集対象の CubicBezierCurve を取得
        CubicBezierCurve curve = (CubicBezierCurve)target;

        // 必要な全てのTransformが存在しているか確認
        if (curve.StageObject1 && curve.StageObject2 && curve.ControlPoint1 && curve.ControlPoint2)
        {
            // 変更の記録開始（変更を検出するブロック）
            EditorGUI.BeginChangeCheck();

            // 制御点にドラッグ可能な位置ハンドルを表示（マウス操作で動かせる）
            Vector3 p1 = Handles.PositionHandle(curve.ControlPoint1.position, Quaternion.identity);
            Vector3 p2 = Handles.PositionHandle(curve.ControlPoint2.position, Quaternion.identity);

            // 位置が変更された場合にのみ処理
            if (EditorGUI.EndChangeCheck())
            {
                // Undoシステムに制御点の移動を記録（Ctrl+Z などで戻せるように）
                Undo.RecordObject(curve.ControlPoint1, "Move Control Point 1");
                Undo.RecordObject(curve.ControlPoint2, "Move Control Point 2");

                // 制御点の位置を更新
                curve.ControlPoint1.position = p1;
                curve.ControlPoint2.position = p2;
            }
        }
    }
}
#endif