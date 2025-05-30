//====================================================
// �X�N���v�g���FStageSelectManager_Ver2
// �쐬�ҁF����
// ���e�F�X�e�[�W�Z���N�g���ꊇ�Ǘ�����X�N���v�g
// �ŏI�X�V���F05/23
// 
// [Log]
// 05/23 ���� �X�N���v�g�쐬
//====================================================
using UnityEngine;

public class StageSelectManager_Ver2 : MonoBehaviour
{


    [SerializeField] private GameObject StageObject;
    [SerializeField] private GameObject BezierCurveObject;
    [SerializeField] private StageSelector StageSelectorComponent;
    [SerializeField] private BezierMover BezierMoverComponent;

    private Transform[] StageChildArray;
    private CubicBezierCurve[] BezierCurveChildArray;

    private bool IsReverse = false;

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

        BezierMoverComponent.SetPosition(BezierCurveChildArray[StageSelectorComponent.GetStageNumber()].StageObject1.position);
    }

   
    void Update()
    {
        if (StageChildArray.Length - 1 != BezierCurveChildArray.Length) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (StageSelectorComponent.GetStageNumber() <= 0) return;

            if (BezierMoverComponent.GetIsMoving() && IsReverse) return;

            IsReverse = true;

            BezierMoverComponent.StartMove(true, BezierCurveChildArray[StageSelectorComponent.GetStageNumber() - 1]);

            StageSelectorComponent.SetStageNumber(-1);
            
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            if (StageSelectorComponent.GetStageNumber() >= BezierCurveChildArray.Length) return;

            if (BezierMoverComponent.GetIsMoving() && !IsReverse) return;

            IsReverse = false;

            BezierMoverComponent.StartMove(false, BezierCurveChildArray[StageSelectorComponent.GetStageNumber()]);

            StageSelectorComponent.SetStageNumber(1);
           
        }
    }
}
