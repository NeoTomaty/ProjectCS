//======================================================
// [GameClearSequence]
// 作成者：荒井修
// 最終更新日：06/19
// 
// [Log]
// 05/08　荒井　仮のクリア演出を作成
// 05/10　荒井　OnGameClear関数に戻り値を追加
// 05/11　荒井　カメラがスナックを追跡する処理を追加
// 05/12　荒井　一連の流れを仮実装
// 05/16　荒井　スコア表示等に対応
// 05/17　荒井　スナックが吹っ飛ぶ方向が完全な真上じゃないのをクリア演出限定で修正
// 05/19　荒井　クリアUI以外のキャンバスを非表示にする処理を追加
// 05/29　中町　クリア演出SE実装
// 05/30　荒井　BlownAway_Ver3に対応
// 06/13  高下　スナックオブジェクトを変更する関数を追加
// 06/16　荒井　クリア演出の内容を大幅に変更
// 06/18　荒井　カメラの動かし方を変更し、意図しないカメラワークを対策
// 06/19　荒井　ポーズ入力無効化を追加
// 06/20　高下　スコア保存の処理を追加
//======================================================
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// クリア演出を制御するスクリプト
public class GameClearSequence : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private ClearConditions ClearConditions;   // シーン遷移を管理するスクリプト
    [SerializeField] private FlyingPoint FlyingPoint;           // スコアを管理するスクリプト
    [SerializeField] private GameObject ClearUI;                // クリア演出のUI
    [SerializeField] private GameObject PlayerObject;           // プレイヤーオブジェクト
    [SerializeField] private GameObject SnackObject;            // スナックオブジェクト
    [SerializeField] private GameObject CameraObject;           // カメラ
    [SerializeField] private PlayerInput PauseInput;

    [Header("カメラの設定")]
    [SerializeField] private float CameraTiltAngle = 0f;    // カメラの傾き角度
    [SerializeField] private float Offset = 30f;            // カメラの距離を調整するオフセット値

    [Header("クリアUIを強制的に表示する時間")]
    [SerializeField] private float UIShowTime = 10f; // クリアUIを強制的に表示する時間


    [Header("エフェクトの設定")]
    [SerializeField] GameObject SnackEffect;
    [SerializeField] float EffectSize = 1.0f;

    [Header("パーティクルのメッシュ")]
    [SerializeField] Mesh ParticleMesh;
    [SerializeField] Material ParticleMaterial;

    [Header("パーティクルのパラメータ")]
    [SerializeField] float Size = 1.0f;
    [SerializeField] float SpeedMIN = 0.5f;
    [SerializeField] float SpeedMAX = 1.5f;
    [SerializeField] float RotateSpeedMIN = 30.0f;
    [SerializeField] float RotateSpeedMAX = 200.0f;

    private GameObject ClearBackImage;

    private PlayerInput PlayerInput; // プレイヤーの入力を管理するcomponent

    // スコア
    private float Score = 0f;

    // クリア後タイマー
    private float AfterTimer = 0f;
    private float UITimer = 0f;

    // クリア演出中フラグ
    private bool IsClearSequence = false;

    // プレイヤー停止フラグ
    private bool IsPlayerStop = false;

    // UI表示フラグ
    private bool IsUIVisible = false;

    // 背景表示フラグ
    private bool IsBackVisible = false;

    // カメラの制御情報
    private Vector3 CameraStartPos = Vector3.zero;
    private Vector3 CameraTargetPos = Vector3.zero;
    private Vector3 SnackPos = Vector3.zero;
    private float FocusHeight = 0f;

    [Header("サウンド設定")]

    //ゲームクリア時に再生する効果音(AudioClip)をインスペクターから設定できるようにする
    [SerializeField] private AudioClip ClearSE;

    //効果音を再生するためのAudioSourceコンポーネント
    private AudioSource audioSource;

    // クリア条件を満たした時に呼び出す関数
    // 正常に終了した場合はtrueを、そうでない場合はfalseを返す
    public bool OnGameClear()
    {
        if (ClearConditions == null || FlyingPoint == null || ClearUI == null || PlayerObject == null || SnackObject == null || CameraObject == null)
        {
            Debug.LogError("GameClearSequence >> インスペクターでの設定が不十分です");
            return false;
        }

        MovePlayer MovePlayer = PlayerObject.GetComponent<MovePlayer>();

        BlownAway_Ver3 BlownAway = SnackObject.GetComponent<BlownAway_Ver3>();
        ObjectGravity SnackGravity = SnackObject.GetComponent<ObjectGravity>();

        CameraFunction CameraFunction = CameraObject.GetComponent<CameraFunction>();


        if (MovePlayer == null ||BlownAway == null || SnackGravity == null|| CameraFunction == null)
        {
            Debug.LogError("GameClearSequence >> 使用するスクリプトが参照先にアタッチされていません");
            return false;
        }

        // 入力を無効化
        PlayerInput.actions.Disable();
        if (PauseInput != null)
        {
            PauseInput.actions.Disable();
        }

        // 背景を表示
        ClearBackImage = ClearUI.transform.GetChild(0).gameObject;
        ClearBackImage.SetActive(true);

        // クリアUI以外のキャンバスを非表示にする
        Transform FinishCanvas = ClearUI.transform.parent;
        Transform ParentCanvas = FinishCanvas.parent;
        for (int i = 0; i < ParentCanvas.childCount; i++)
        {
            Transform Child = ParentCanvas.GetChild(i);
            if (Child != FinishCanvas)
            {
                Child.gameObject.SetActive(false);
            }
        }

        Score = FlyingPoint.TotalScore;
        Text ScoreText = ClearUI.transform.GetChild(2).GetComponent<Text>();
        ScoreText.text = "スコア：" + Score.ToString();
        Debug.Log("GameClearSequence >> スコア：" + Score);

        // スナックのクローンを作成
        Vector3 SpawnPos = SnackObject.transform.position;

        // 元のスナックを非表示にする
        SnackObject.GetComponent<MeshRenderer>().enabled = false;

        // プレイヤーとスナックの当たり判定を無効化
        Collider PlayerCollider = PlayerObject.GetComponent<Collider>();
        Collider SnackCollider = SnackObject.GetComponent<Collider>();
        Physics.IgnoreCollision(PlayerCollider, SnackCollider);

        // CameraFunctionを無効化
        CameraFunction.enabled = false;

        // カメラの移動開始座標と目標座標を設定
        CameraStartPos = CameraObject.transform.position;
        SnackPos = SnackObject.transform.position;
        Vector3 CameraToSnack = SnackPos - CameraStartPos;
        Vector3 CameraDirection = CameraToSnack.normalized * Offset; // カメラの距離を調整
        CameraTargetPos = SnackPos - CameraDirection;

        FocusHeight = SnackPos.y;

        if (SnackEffect != null || ParticleMesh != null || ParticleMaterial != null)
        {
            // エフェクト生成
            GameObject Effect = Instantiate(SnackEffect, SpawnPos, Quaternion.identity);

            // エフェクトサイズ設定
            Effect.transform.localScale = new Vector3(EffectSize, EffectSize, EffectSize);

            // パーティクルメッシュ設定
            ParticleSystem PS = Effect.GetComponent<ParticleSystem>();
            var PSRenderer = PS.GetComponent<ParticleSystemRenderer>();
            PSRenderer.mesh = ParticleMesh;
            PSRenderer.material = ParticleMaterial;

            // パーティクルパラメータ設定
            var PSMain = PS.main;
            // サイズ
            PSMain.startSize = Size;

            // 射出速度
            float min = SpeedMIN;
            float max = SpeedMAX;
            PSMain.startSpeed = new ParticleSystem.MinMaxCurve(min, max);

            // 回転速度
            var Rotation = PS.rotationOverLifetime;
            min = RotateSpeedMIN * Mathf.Deg2Rad;
            max = RotateSpeedMAX * Mathf.Deg2Rad;
            Rotation.x = new ParticleSystem.MinMaxCurve(min, max);
            Rotation.y = new ParticleSystem.MinMaxCurve(min, max);
            Rotation.z = new ParticleSystem.MinMaxCurve(min, max);

            // エフェクト再生
            PS.Play();
        }

        //効果音(SE)が設定されていて、AudioSourceも存在するときに再生する
        if (ClearSE != null && audioSource != null)
        {
            //一回だけ効果音を再生する(重ねて再生可能)
            audioSource.PlayOneShot(ClearSE);
        }

        // クリア演出中フラグを立てる
        IsClearSequence = true;

        return true;
    }

    private void Awake()
    {
        PlayerInput = PlayerObject.GetComponent<PlayerInput>();

        //AudioSourceコンポーネントをこのGameObjectに追加し、効果音再生用に保持しておく
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    private void Start()
    {
        ClearUI.transform.GetChild(0).gameObject.SetActive(false);
        ClearUI.transform.GetChild(1).gameObject.SetActive(false);
        ClearUI.transform.GetChild(2).gameObject.SetActive(false);
    }

    void Update()
    {
        if(!IsClearSequence) return;

        // タイマー進行
        AfterTimer += Time.deltaTime;

        // キー・ボタン入力でシーン遷移
        // 演出が終わってから入力受付
        if (IsUIVisible)
        {
            if (Input.anyKeyDown)
            {
                // スコア保存
                ScoreManager scoreManager = Object.FindFirstObjectByType<ScoreManager>();
                StageSelector stageSelector = Object.FindFirstObjectByType<StageSelector>();

                if (scoreManager && stageSelector)
                {
                    int selectStageNumber = stageSelector.GetStageNumber();
                    int stageScore = scoreManager.GetStageScore(selectStageNumber);

                    if (stageScore < Score)
                    {
                        scoreManager.SetStageScore(selectStageNumber, (int)Score);
                        Debug.Log("スコア更新");
                    }
                }

                ClearConditions.TriggerSceneTransition();
            }
        }

         // 一定時間経過で後の処理をスキップ
        if (AfterTimer > UIShowTime) return;

       // カメラにスナックを追跡させる
        // 座標
        float OffsetTime = AfterTimer * 1f;
        OffsetTime = Mathf.Clamp01(OffsetTime);
        Vector3 CarrentCameraPos=Vector3.Lerp(CameraStartPos, CameraTargetPos, OffsetTime);
        CameraObject.transform.position = CarrentCameraPos;

        // 視線
        Vector3 Target = SnackPos;
        FocusHeight += 1f * Time.deltaTime * EffectSize;
        Target.y = FocusHeight;
        CameraObject.transform.LookAt(Target);

        // スナックを吹っ飛ばした後プレイヤーを停止
        if (!IsPlayerStop && AfterTimer > 0.1f)
        {
            // プレイヤーの移動速度を0にする
            PlayerObject.GetComponent<MovePlayer>().MoveSpeedMultiplier = 0f;

            IsPlayerStop = true;
        }

        // クリア演出中のUIを表示
        if (!IsBackVisible)
        {
            UITimer += Time.deltaTime * 0.5f;
            UITimer = Mathf.Clamp01(UITimer);

            Color C = Color.black;
            C.a = Mathf.Lerp(0f, 0.6f, UITimer);
            ClearBackImage.GetComponent<Image>().color = C;

            if (UITimer >= 1f) IsBackVisible = true;
        }
        else
        {
            // UIを表示
            ClearUI.transform.GetChild(1).gameObject.SetActive(true);
            ClearUI.transform.GetChild(2).gameObject.SetActive(true);

            // UI表示フラグを立てる
            IsUIVisible = true;
        }
    }

    public void SetSnackObject(GameObject snack)
    {
        SnackObject = snack;
    }
}
