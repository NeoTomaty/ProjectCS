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
using UnityEngine.UI;


public class StageSelectManager_Ver2 : MonoBehaviour
{


    [SerializeField] private GameObject StageObject;
    [SerializeField] private GameObject BezierCurveObject;
    [SerializeField] private GameObject StageImageUI;
    [SerializeField] private GameObject StartUI;
    [SerializeField] private Image StageImage;
    [SerializeField] private Image StageName01;
    [SerializeField] private GameObject StageName02;
    [SerializeField] private BezierMover BezierMoverComponent;
    [SerializeField] private StageSelectMoveCamera MoveCamera;
    [SerializeField] private StageSelectChangeModel ChangeModel;
    [SerializeField] private GameObject PointLineUI;

    private Transform[] StageChildArray;
    private Transform[] StageModelTransform;
    private CubicBezierCurve[] BezierCurveChildArray;
    private StageSelector StageSelectorComponent;


    [SerializeField] private string[] GameSceneNames = new string[6];

    [SerializeField] private string TitleSceneName;
    [SerializeField] private Sprite[] StageImageSprites = new Sprite[5];
    [SerializeField] private Sprite[] StageNameSprites = new Sprite[5];

    private bool IsReverse = false;
    private bool IsInputEnabled = false;
    private float ScaleChangeTimer = 0f;
    [SerializeField] private float ScaleChangeDuration = 0.5f;
    private bool IsScaling = true;
    [SerializeField] private Vector3 SmallStageModelSize = new Vector3(0.5f, 0.5f, 0.5f);
    private UILineConnector LineConnector;


    void Start()
    {
        if (!StageObject) Debug.LogError("StageObjectが設定されていません");
        if (!BezierCurveObject) Debug.LogError("BezierCurveObjectが設定されていません");

        int childCount = StageObject.transform.childCount;
        StageChildArray = new Transform[childCount];
        StageModelTransform = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            StageChildArray[i] = StageObject.transform.GetChild(i);
            StageModelTransform[i] = StageChildArray[i].transform.GetChild(0);
            StageModelTransform[i].transform.localScale = SmallStageModelSize;
        }


        childCount = BezierCurveObject.transform.childCount;
        BezierCurveChildArray = new CubicBezierCurve[childCount];

        for (int i = 0; i < childCount; i++)
        {
            BezierCurveChildArray[i] = BezierCurveObject.transform.GetChild(i).GetComponent<CubicBezierCurve>();
        }

        StageSelectorComponent = Object.FindFirstObjectByType<StageSelector>();

        if (!StageSelectorComponent)
        {
            Debug.LogError("StageSelectorが見つかりません");
            return;
        }
       
        if(StageSelectorComponent.GetStageNumber() == BezierCurveChildArray.Length)
        {
            BezierMoverComponent.SetPosition(BezierCurveChildArray[StageSelectorComponent.GetStageNumber() - 1].StageObject2.position);
        }
        else
        {
            BezierMoverComponent.SetPosition(BezierCurveChildArray[StageSelectorComponent.GetStageNumber()].StageObject1.position);
        }

        StageImage.sprite = StageImageSprites[StageSelectorComponent.GetStageNumber()];
        
        StageName01.sprite = StageNameSprites[StageSelectorComponent.GetStageNumber()];

        StageImageUI.transform.localScale = Vector3.zero;

        StartUI.SetActive(false);
        StageName02.SetActive(false);

        LineConnector = PointLineUI.GetComponent<UILineConnector>();

        LineConnector.SetArrayNumber(StageSelectorComponent.GetStageNumber());

        
    }

    private void OnValidate()
    {
        if (GameSceneNames.Length != StageImageSprites.Length)
        {

            Sprite[] newArray = new Sprite[GameSceneNames.Length];
            for (int i = 0; i < Mathf.Min(GameSceneNames.Length, StageImageSprites.Length); i++)
            {
                newArray[i] = StageImageSprites[i]; // 既存の値を保持
            }
            StageImageSprites = newArray;
        }

        if (GameSceneNames.Length != StageNameSprites.Length)
        {

            Sprite[] newArray = new Sprite[GameSceneNames.Length];
            for (int i = 0; i < Mathf.Min(GameSceneNames.Length, StageNameSprites.Length); i++)
            {
                newArray[i] = StageNameSprites[i]; // 既存の値を保持
            }
            StageNameSprites = newArray;
        }
    }


    void Update()
    {
        if (StageChildArray.Length - 1 != BezierCurveChildArray.Length) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow) && !MoveCamera.GetIsSwitched())
        {
            if (StageSelectorComponent.GetStageNumber() <= 0) return;

            if (BezierMoverComponent.GetIsMoving() && IsReverse) return;

            IsReverse = true;

            BezierMoverComponent.StartMove(true, BezierCurveChildArray[StageSelectorComponent.GetStageNumber() - 1]);

            StageSelectorComponent.SetStageNumber(-1);
            

        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && !MoveCamera.GetIsSwitched())
        {

            if (StageSelectorComponent.GetStageNumber() == BezierCurveChildArray.Length) return;

            if (BezierMoverComponent.GetIsMoving() && !IsReverse) return;

            IsReverse = false;

            BezierMoverComponent.StartMove(false, BezierCurveChildArray[StageSelectorComponent.GetStageNumber()]);

            StageSelectorComponent.SetStageNumber(1);
           

        }

        if (Input.GetKeyDown(KeyCode.Return) && !BezierMoverComponent.GetIsMoving())
        {
            if (GameSceneNames.Length == StageSelectorComponent.GetStageNumber()) return;

            if (!MoveCamera.GetIsSwitched())
            {
                MoveCamera.SetIsSwitched(true);
            }
            else
            {
                ChangeScene(GameSceneNames[StageSelectorComponent.GetStageNumber()]);
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace) && !BezierMoverComponent.GetIsMoving())
        {

            if (MoveCamera.GetIsSwitched())
            {
                MoveCamera.SetIsSwitched(false);
            }
            else
            {
                ChangeScene(TitleSceneName);
            }
        }



        if (!BezierMoverComponent.GetIsMoving())
        {
            ScaleChangeTimer += Time.deltaTime;
            float t = Mathf.Clamp01(ScaleChangeTimer / ScaleChangeDuration);
            StageImageUI.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            ChangeModel.SetChangeModel(true);

            for (int i = 0; i < StageModelTransform.Length; i++)
            {
                if (i == StageSelectorComponent.GetStageNumber())
                {
                    if(StageModelTransform[i].transform.localScale != Vector3.one)
                    {
                        StageModelTransform[i].transform.localScale = Vector3.Lerp(SmallStageModelSize, Vector3.one, t);
                    }

                    StageModelTransform[i].transform.Rotate(0f, 10.0f * Time.deltaTime, 0f);
                }
                else
                {
                    StageModelTransform[i].transform.rotation = Quaternion.identity;
                }

               
            }


            if (t >= 1.0f)
            {
                ScaleChangeTimer = ScaleChangeDuration;
                
            }

            LineConnector.SetArrayNumber(StageSelectorComponent.GetStageNumber());
        }
        else
        {
            ScaleChangeTimer -= Time.deltaTime;
            float t = Mathf.Clamp01(ScaleChangeTimer / ScaleChangeDuration);
            StageImageUI.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            ChangeModel.SetChangeModel(false);

            for (int i = 0; i < StageModelTransform.Length;i++)
            {
                if(StageModelTransform[i].transform.localScale != SmallStageModelSize)
                {
                    StageModelTransform[i].transform.localScale = Vector3.Lerp(SmallStageModelSize, Vector3.one, t);
                }

            }

            if (t <= 0.0f)
            {
                ScaleChangeTimer = 0f;
                StageImage.sprite = StageImageSprites[StageSelectorComponent.GetStageNumber()];
                StageName01.sprite = StageNameSprites[StageSelectorComponent.GetStageNumber()];
                StageName02.GetComponent<Image>().sprite = StageNameSprites[StageSelectorComponent.GetStageNumber()];
            }
        }

        if (MoveCamera.GetIsSwitched())
        {
            StageImageUI.SetActive(false);
            PointLineUI.SetActive(false);
        }
        else if (!MoveCamera.GetIsSwitched() && !MoveCamera.GetIsInterpolating())
        {
            StageImageUI.SetActive(true);
            PointLineUI.SetActive(true);
        }

        if (MoveCamera.GetIsSwitched() && !MoveCamera.GetIsInterpolating())
        {
            StartUI.SetActive(true);
            StageName02.SetActive(true);
            
        }
        else if (!MoveCamera.GetIsSwitched())
        {
            StartUI.SetActive(false);
            StageName02.SetActive(false);
        }

    }

    private void ChangeScene(string sceneName)
    {
        FadeManager fade = Object.FindFirstObjectByType<FadeManager>();
        if (fade)
        {
            fade.FadeToScene(sceneName);
        }
        else
        {
            Debug.LogError("FadeManagerが見つかりません");
        }
    }
}
