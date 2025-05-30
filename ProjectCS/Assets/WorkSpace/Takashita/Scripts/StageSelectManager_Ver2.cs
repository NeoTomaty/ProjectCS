//====================================================
// スクリプト名：StageSelectManager_Ver2
// 作成者：高下
// 内容：ステージセレクトを一括管理するスクリプト
// 最終更新日：05/23
// 
// [Log]
// 05/23 高下 スクリプト作成
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
        if (!StageObject) Debug.LogError("StageObjectが設定されていません");
        if (!BezierCurveObject) Debug.LogError("BezierCurveObjectが設定されていません");

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
