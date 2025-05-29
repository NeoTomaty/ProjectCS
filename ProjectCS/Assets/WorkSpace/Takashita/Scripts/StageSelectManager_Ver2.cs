//====================================================
// スクリプト名：StageSelectManager_Ver2
// 作成者：高下
// 内容：ステージセレクトを一括管理するスクリプト
// 最終更新日：05/23
// 
// [Log]
// 05/23 高下 スクリプト作成
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
