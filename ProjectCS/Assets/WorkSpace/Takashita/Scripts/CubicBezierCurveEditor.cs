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

// CubicBezierCurve ��ΏۂƂ����J�X�^���G�f�B�^�i�V�[���r���[�p�j
[CustomEditor(typeof(CubicBezierCurve))]
public class CubicBezierCurveEditor : Editor
{
    // �V�[���r���[�ɃJ�X�^��UI��\�����郁�\�b�h
    private void OnSceneGUI()
    {
        // �ҏW�Ώۂ� CubicBezierCurve ���擾
        CubicBezierCurve curve = (CubicBezierCurve)target;

        // �K�v�ȑS�Ă�Transform�����݂��Ă��邩�m�F
        if (curve.StageObject1 && curve.StageObject2 && curve.ControlPoint1 && curve.ControlPoint2)
        {
            // �ύX�̋L�^�J�n�i�ύX�����o����u���b�N�j
            EditorGUI.BeginChangeCheck();

            // ����_�Ƀh���b�O�\�Ȉʒu�n���h����\���i�}�E�X����œ�������j
            Vector3 p1 = Handles.PositionHandle(curve.ControlPoint1.position, Quaternion.identity);
            Vector3 p2 = Handles.PositionHandle(curve.ControlPoint2.position, Quaternion.identity);

            // �ʒu���ύX���ꂽ�ꍇ�ɂ̂ݏ���
            if (EditorGUI.EndChangeCheck())
            {
                // Undo�V�X�e���ɐ���_�̈ړ����L�^�iCtrl+Z �ȂǂŖ߂���悤�Ɂj
                Undo.RecordObject(curve.ControlPoint1, "Move Control Point 1");
                Undo.RecordObject(curve.ControlPoint2, "Move Control Point 2");

                // ����_�̈ʒu���X�V
                curve.ControlPoint1.position = p1;
                curve.ControlPoint2.position = p2;
            }
        }
    }
}
#endif