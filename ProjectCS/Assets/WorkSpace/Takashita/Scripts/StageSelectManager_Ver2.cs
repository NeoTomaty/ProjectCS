//====================================================
// �X�N���v�g���FStageSelectManager_Ver2
// �쐬�ҁF����
// ���e�F�X�e�[�W�Z���N�g���ꊇ�Ǘ�����X�N���v�g
// �ŏI�X�V���F05/23
// 
// [Log]
// 05/23 ���� �X�N���v�g�쐬
//====================================================
using Unity.VisualScripting;
using UnityEngine;

public class StageSelectManager_Ver2 : MonoBehaviour
{


    [SerializeField] private GameObject StageObject;
    [SerializeField] private GameObject BezierCurveObject;
    [SerializeField] private StageSelector StageSelectorComponent;


    private Transform[] StageChildArray;
    private CubicBezierCurve[] BezierCurveChildArray;

    void Start()
    {
        if (!StageObject) Debug.LogError("StageObject���ݒ肳��Ă��܂���");
        if (!BezierCurveObject) Debug.LogError("BezierCurveObject���ݒ肳��Ă��܂���");

        int childCount = StageObject.transform.childCount;
        StageChildArray = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            StageChildArray[i] = StageObject.transform.GetChild(i);
        }


        childCount = BezierCurveObject.transform.childCount;
        BezierCurveChildArray = new CubicBezierCurve[childCount];

        for (int i = 0; i < childCount; i++)
        {
            BezierCurveChildArray[i] = BezierCurveObject.transform.GetChild(i).GetComponent<CubicBezierCurve>();
        }

    }

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StageSelectorComponent.SetStageNumber(-1);

        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StageSelectorComponent.SetStageNumber(1);
        }
    }
}
