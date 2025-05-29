//====================================================
// �X�N���v�g���FCubicBezierCurveEditor
// �쐬�ҁF����
// ���e�FScene�r���[�Ńx�W�F�Ȑ��̐���_�iControlPoint1�E2�j���}�E�X�Œ��ړ�������悤�ɂ���
// �ŏI�X�V���F05/26
// 
// [Log]
// 05/26 ���� �X�N���v�g�쐬
//====================================================
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

// �G�f�B�^��� DynamicBezierCurve �� SceneGUI ���g��
[CustomEditor(typeof(CubicBezierCurve))]
public class DynamicBezierCurveEditor : Editor
{
    private void OnSceneGUI()
    {
        CubicBezierCurve curve = (CubicBezierCurve)target;

        // ���S�`�F�b�N�ianchor��control�̐������������j
        if (curve.anchorPoints.Count < 2 || curve.controlPoints.Count != (curve.anchorPoints.Count - 1) * 2)
            return;

        // ����_������h���b�O�ł���悤�ɂ���
        EditorGUI.BeginChangeCheck();

        for (int i = 0; i < curve.controlPoints.Count; i++)
        {
            Transform cp = curve.controlPoints[i];
            if (cp == null) continue;

            // �n���h����\��
            Vector3 newPos = Handles.PositionHandle(cp.position, Quaternion.identity);

            if (newPos != cp.position)
            {
                Undo.RecordObject(cp, $"Move Control Point {i}");
                cp.position = newPos;
            }
        }

        // �A���J�[�|�C���g���ҏW�\�ɂ������ꍇ�͂������ǉ��i�C�Ӂj
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
            // Scene���X�V
            EditorUtility.SetDirty(curve);
        }
    }
}
#endif