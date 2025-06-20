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
using UnityEngine.InputSystem;
using UnityEngine.UI;

// ステージ選択画面全体の制御クラス（入力・カメラ・モデル・UI管理など）
public class StageSelectManager_Ver2 : MonoBehaviour
{
    // ステージ/カーブ/各UIオブジェクトの参照
    [SerializeField] private GameObject StageObject;
    [SerializeField] private GameObject BezierCurveObject;
    [SerializeField] private GameObject StageImageUI;
    [SerializeField] private GameObject StartUI;
    [SerializeField] private GameObject ScoreUI;
    [SerializeField] private Image StageImage;
    [SerializeField] private Image StageName01;
    [SerializeField] private GameObject StageName02;

    // ステージ移動処理・カメラ・モデル制御など
    [SerializeField] private BezierMover BezierMoverComponent;
    [SerializeField] private StageSelectMoveCamera MoveCamera;
    [SerializeField] private StageSelectChangeModel ChangeModel;

    // UI要素（選択位置のライン）
    [SerializeField] private GameObject PointLineUI;

    // スコア読み込み・スコア表示クラス
    [SerializeField] private LoadStageScore StageScore;
    [SerializeField] private DrawScore DrawScore;

    // ステージ情報
    private Transform[] StageChildArray;
    private Transform[] StageModelTransform;
    private CubicBezierCurve[] BezierCurveChildArray;
    private StageSelector StageSelectorComponent;

    // ステージ名/画像/シーン名
    [SerializeField] private string[] GameSceneNames = new string[6];
    [SerializeField] private string TitleSceneName;
    [SerializeField] private Sprite[] StageImageSprites = new Sprite[5];
    [SerializeField] private Sprite[] StageNameSprites = new Sprite[5];

    // ステージ移動・スケーリング制御
    private bool IsReverse = false;
    private bool IsInputEnabled = false;
    private float ScaleChangeTimer = 0f;
    [SerializeField] private float ScaleChangeDuration = 0.5f;
    private bool IsScaling = true;
    [SerializeField] private Vector3 SmallStageModelSize = new Vector3(0.5f, 0.5f, 0.5f);
    private UILineConnector LineConnector;

    // 入力関連
    private PlayerInput PlayerInput;
    private InputAction ConfirmAction;
    private InputAction CancelAction;
    private InputAction LeftMoveAction;
    private InputAction RightMoveAction;
    private InputAction OptionAction;

    // フェード管理
    private FadeManager fade;

    // オプション管理
    [SerializeField] private GameObject OptionCanvasObject;
    private bool IsOptionsOpen = false;

    // アニメーション管理
    [SerializeField] private Animator PlayerAnimation;

    // SE管理
     [SerializeField] private StageSelectSEPlayer SEPlayer;

    private void Awake()
    {
        // PlayerInputの取得とアクションの割り当て
        PlayerInput = GetComponent<PlayerInput>();
        ConfirmAction = PlayerInput.actions["Confirm"];
        CancelAction = PlayerInput.actions["Cancel"];
        LeftMoveAction = PlayerInput.actions["LeftMove"];
        RightMoveAction = PlayerInput.actions["RightMove"];
        OptionAction = PlayerInput.actions["Option"];
    }

    void Start()
    {
        // 必須オブジェクト確認
        if (!StageObject) Debug.LogError("StageObjectが設定されていません");
        if (!BezierCurveObject) Debug.LogError("BezierCurveObjectが設定されていません");

        fade = Object.FindFirstObjectByType<FadeManager>();

        // ステージの子要素取得（ステージ本体）
        int childCount = StageObject.transform.childCount;
        StageChildArray = new Transform[childCount];
        StageModelTransform = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            StageChildArray[i] = StageObject.transform.GetChild(i);
            StageModelTransform[i] = StageChildArray[i].transform.GetChild(0);
            StageModelTransform[i].localScale = SmallStageModelSize; // 最初は小さく
        }

        // ベジェ曲線取得
        childCount = BezierCurveObject.transform.childCount;
        BezierCurveChildArray = new CubicBezierCurve[childCount];
        for (int i = 0; i < childCount; i++)
        {
            BezierCurveChildArray[i] = BezierCurveObject.transform.GetChild(i).GetComponent<CubicBezierCurve>();
        }

        // ステージ選択状態の取得
        StageSelectorComponent = Object.FindFirstObjectByType<StageSelector>();
        if (!StageSelectorComponent)
        {
            Debug.LogError("StageSelectorが見つかりません");
            return;
        }

        // プレイヤーの初期位置を決定
        if (StageSelectorComponent.GetStageNumber() == BezierCurveChildArray.Length)
        {
            BezierMoverComponent.SetPosition(BezierCurveChildArray[StageSelectorComponent.GetStageNumber() - 1].StageObject2.position);
        }
        else
        {
            BezierMoverComponent.SetPosition(BezierCurveChildArray[StageSelectorComponent.GetStageNumber()].StageObject1.position);
        }

        // 初期表示の画像とUI設定
        StageImage.sprite = StageImageSprites[StageSelectorComponent.GetStageNumber()];
        StageName01.sprite = StageNameSprites[StageSelectorComponent.GetStageNumber()];
        StageImageUI.transform.localScale = Vector3.zero;
        StartUI.SetActive(false);
        StageName02.SetActive(false);

        LineConnector = PointLineUI.GetComponent<UILineConnector>();
        LineConnector.SetArrayNumber(StageSelectorComponent.GetStageNumber());
    }

    // エディタ上で配列の要素数がズレたとき自動修正
    private void OnValidate()
    {
        // ステージ画像配列の要素数チェック
        if (GameSceneNames.Length != StageImageSprites.Length)
        {
            Sprite[] newArray = new Sprite[GameSceneNames.Length];
            for (int i = 0; i < Mathf.Min(GameSceneNames.Length, StageImageSprites.Length); i++)
            {
                newArray[i] = StageImageSprites[i];
            }
            StageImageSprites = newArray;
        }

        // ステージ名配列の要素数チェック
        if (GameSceneNames.Length != StageNameSprites.Length)
        {
            Sprite[] newArray = new Sprite[GameSceneNames.Length];
            for (int i = 0; i < Mathf.Min(GameSceneNames.Length, StageNameSprites.Length); i++)
            {
                newArray[i] = StageNameSprites[i];
            }
            StageNameSprites = newArray;
        }
    }

    void Update()
    {
        if (IsOptionsOpen)
        {
            if (!OptionCanvasObject.activeSelf || OptionAction.WasPerformedThisFrame())
            {
                IsOptionsOpen = false;
                OptionCanvasObject.SetActive(false);
                if(PlayerAnimation) PlayerAnimation.speed = 1f;
            }

            return;
        }
        else
        {
            if (OptionAction.WasPerformedThisFrame() && !MoveCamera.GetIsSwitched() && !BezierMoverComponent.GetIsMoving())
            {
                IsOptionsOpen = true;
                OptionCanvasObject.SetActive(true);
                if (PlayerAnimation) PlayerAnimation.speed = 0f;
            }
        }

        // ベジェ曲線とステージ数の整合性チェック
        if (StageChildArray.Length - 1 != BezierCurveChildArray.Length) return;

        // 左移動処理
        if (LeftMoveAction.WasPerformedThisFrame() && !MoveCamera.GetIsSwitched())
        {
            if (StageSelectorComponent.GetStageNumber() <= 0) return;
            if (BezierMoverComponent.GetIsMoving() && IsReverse) return;

            SEPlayer.PlaySE(StageSelectSEPlayer.StageSelectSE.Select);
            IsReverse = true;
            BezierMoverComponent.StartMove(true, BezierCurveChildArray[StageSelectorComponent.GetStageNumber() - 1]);
            StageSelectorComponent.SetStageNumber(-1);
        }

        // 右移動処理
        if (RightMoveAction.WasPerformedThisFrame() && !MoveCamera.GetIsSwitched())
        {
            if (StageSelectorComponent.GetStageNumber() == BezierCurveChildArray.Length) return;
            if (BezierMoverComponent.GetIsMoving() && !IsReverse) return;

            SEPlayer.PlaySE(StageSelectSEPlayer.StageSelectSE.Select);
            IsReverse = false;
            BezierMoverComponent.StartMove(false, BezierCurveChildArray[StageSelectorComponent.GetStageNumber()]);
            StageSelectorComponent.SetStageNumber(1);
        }

        // 決定ボタン処理（カメラ切り替え or シーン遷移）
        if (ConfirmAction.WasPerformedThisFrame() && !BezierMoverComponent.GetIsMoving())
        {
            if (GameSceneNames.Length == StageSelectorComponent.GetStageNumber()) return;

            SEPlayer.PlaySE(StageSelectSEPlayer.StageSelectSE.Confirm);

            if (!MoveCamera.GetIsSwitched())
            {
                MoveCamera.SetIsSwitched(true);
            }
            else
            {
                ChangeScene(GameSceneNames[StageSelectorComponent.GetStageNumber()]);
            }
        }

        // キャンセルボタン処理（戻る or タイトルに戻る）
        if (CancelAction.WasPerformedThisFrame() && !BezierMoverComponent.GetIsMoving())
        {
            SEPlayer.PlaySE(StageSelectSEPlayer.StageSelectSE.Cancel);

            if (MoveCamera.GetIsSwitched())
            {
                MoveCamera.SetIsSwitched(false);
            }
            else
            {
                ChangeScene(TitleSceneName);
            }
        }

        // ステージモデルの拡大・回転演出処理
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
                    if (StageModelTransform[i].localScale != Vector3.one)
                    {
                        StageModelTransform[i].localScale = Vector3.Lerp(SmallStageModelSize, Vector3.one, t);
                    }
                    StageModelTransform[i].Rotate(0f, 10.0f * Time.deltaTime, 0f);
                }
                else
                {
                    StageModelTransform[i].rotation = Quaternion.identity;
                }
            }

            if (t >= 1.0f) ScaleChangeTimer = ScaleChangeDuration;
            LineConnector.SetArrayNumber(StageSelectorComponent.GetStageNumber());
        }
        else
        {
            // 移動中はモデルを縮小する
            ScaleChangeTimer -= Time.deltaTime;
            float t = Mathf.Clamp01(ScaleChangeTimer / ScaleChangeDuration);
            StageImageUI.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            ChangeModel.SetChangeModel(false);

            for (int i = 0; i < StageModelTransform.Length; i++)
            {
                if (StageModelTransform[i].localScale != SmallStageModelSize)
                {
                    StageModelTransform[i].localScale = Vector3.Lerp(SmallStageModelSize, Vector3.one, t);
                }
            }

            if (t <= 0.0f)
            {
                ScaleChangeTimer = 0f;
                StageImage.sprite = StageImageSprites[StageSelectorComponent.GetStageNumber()];
                StageName01.sprite = StageNameSprites[StageSelectorComponent.GetStageNumber()];
            }
        }

        // カメラ状態に応じてUIの表示/非表示制御
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

        // ステージ決定後のUI表示とスコア反映
        if (MoveCamera.GetIsSwitched() && !MoveCamera.GetIsInterpolating())
        {
            StartUI.SetActive(true);
            StageName02.SetActive(true);
            ScoreUI.SetActive(true);
            DrawScore.SetScore(StageScore.GetStageScore(StageSelectorComponent.GetStageNumber()));
            StageName02.GetComponent<Image>().sprite = StageNameSprites[StageSelectorComponent.GetStageNumber()];
        }
        else if (!MoveCamera.GetIsSwitched())
        {
            StartUI.SetActive(false);
            StageName02.SetActive(false);
            ScoreUI.SetActive(false);
        }
    }

    // 指定シーンへ遷移（フェードあり）
    private void ChangeScene(string sceneName)
    {
        if (fade)
        {
            fade.FadeToScene(sceneName);
        }
    }
}
