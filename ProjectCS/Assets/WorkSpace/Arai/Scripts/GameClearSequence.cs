//======================================================
// [GameClearSequence]
// 作成者：荒井修
// 最終更新日：05/12
// 
// [Log]
// 05/08　荒井　仮のクリア演出を作成
// 05/10　荒井　OnGameClear関数に戻り値を追加
// 05/11　荒井　カメラがスナックを追跡する処理を追加
// 05/12　荒井　一連の流れを仮実装
//======================================================
using UnityEngine;

// クリア演出を制御するスクリプト
public class GameClearSequence : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private ClearConditions ClearConditions;   // シーン遷移を管理するスクリプト
    //[SerializeField] private PlayerScore PlayerScore;           // スコアを管理するスクリプト
    [SerializeField] private GameObject ClearUI;                // クリア演出のUI
    [SerializeField] private GameObject PlayerObject;           // プレイヤーオブジェクト
    [SerializeField] private GameObject SnackObject;            // スナックオブジェクト
    [SerializeField] private GameObject CameraObject;           // カメラ
    [SerializeField] private GameObject StarObject;             // 星

    [Header("カメラの設定")]
    [SerializeField] private float CameraTiltAngle = 0f; // カメラの傾き角度

    [Header("星の設定")]
    [SerializeField] private float StarHeight = 300f; // 星の高さ
    [SerializeField] private float StarToStarDistance = 200f; // 星と星の距離
    //[SerializeField] private int[] StarScoreThresholdArray; // スコアの閾値

    // クリア後タイマー
    private float AfterTimer = 0f;

    // クリア演出中フラグ
    private bool IsClearSequence = false;

    // プレイヤー停止フラグ
    private bool IsPlayerStop = false;

    // UI表示フラグ
    private bool IsUIVisible = false;

    // カメラ追跡フラグ
    private bool IsCameraStop = false;

    // クリア条件を満たした時に呼び出す関数
    // 正常に終了した場合はtrueを、そうでない場合はfalseを返す
    public bool OnGameClear()
    {
        if (ClearUI == null || PlayerObject == null|| SnackObject == null || CameraObject == null|| ClearConditions == null)
        {
            Debug.LogError("GameClearSequence >> インスペクターでの設定が不十分です");
            return false;
        }

        PlayerSpeedManager PlayerSpeedManager = PlayerObject.GetComponent<PlayerSpeedManager>();

        BlownAway_Ver2 BlownAway = SnackObject.GetComponent<BlownAway_Ver2>();
        ObjectGravity SnackGravity = SnackObject.GetComponent<ObjectGravity>();

        CameraFunction CameraFunction = CameraObject.GetComponent<CameraFunction>();

        if (PlayerSpeedManager == null ||BlownAway == null || SnackGravity == null|| CameraFunction == null)
        {
            Debug.LogError("GameClearSequence >> 使用するスクリプトが参照先にアタッチされていません");
            return false;
        }

        // スナックのリスポーンを無効化
        BlownAway.OnClear();

        // CameraFunctionを無効化
        CameraFunction.enabled = false;

        // 星を配置する
        Vector3 StarPos = SnackObject.transform.position;
        StarPos.y = StarHeight; // スナックの上に星を配置
        GameObject StarClone = Instantiate(StarObject, StarPos, Quaternion.identity);
        //Vector3 StarPos = SnackObject.transform.position;
        //for (int i = 0; i < StarScoreThresholdArray.Length; i++)
        //{
        //    // スコアの閾値の数だけ星を配置
        //    StarPos.y = StarHeight + (StarToStarDistance * i); // スナックの上に星を配置
        //    GameObject StarClone = Instantiate(StarObject, StarPos, Quaternion.identity);
        //}

        // クリア演出中フラグを立てる
        IsClearSequence = true;

        return true;
    }

    private void Start()
    {
        ClearUI.SetActive(false);
    }

    void Update()
    {
        if(!IsClearSequence) return;

        // タイマー進行
        AfterTimer += Time.deltaTime;

        // キー・ボタン入力でシーン遷移
        if (Input.anyKeyDown)
        {
            ClearConditions.TriggerSceneTransition();
        }

        // カメラにスナックを追跡させる
        // 座標
        if (!IsCameraStop)
        {
            float TargetPosY = SnackObject.transform.position.y;
            Vector3 CameraPos = CameraObject.transform.position;
            CameraPos.y = TargetPosY;   // 高さをスナックに合わせる
            CameraObject.transform.position = CameraPos;
        }

        // 視線
        CameraObject.transform.LookAt(SnackObject.transform.position);
        //float FocusTime = AfterTimer * CameraFocusSpeed;
        //FocusTime = Mathf.Clamp01(FocusTime);
        //Vector3 TargetFocus = Vector3.Lerp(CameraObject.transform.position, SnackObject.transform.position, FocusTime);
        //CameraObject.transform.LookAt(TargetFocus);

        // 傾き
        float TiltTime = AfterTimer * 1f;
        TiltTime = Mathf.Clamp01(TiltTime);
        float CurrentTiltAngle = Mathf.LerpAngle(0f, CameraTiltAngle, TiltTime);
        CameraObject.transform.Rotate(0f, 0f, CurrentTiltAngle);


        // スナックを吹っ飛ばした後プレイヤーを停止
        if (!IsPlayerStop && AfterTimer > 0.1f)
        {
            // プレイヤーの移動速度を0にする
            PlayerObject.GetComponent<PlayerSpeedManager>().SetOverSpeed(0f);

            // スナックの重力もここで無効化
            SnackObject.GetComponent<ObjectGravity>().IsActive = false;

            IsPlayerStop = true;
        }

        // クリア演出中のUIを表示
        Vector3 SnackPos = SnackObject.transform.position;

        //// UIを表示するスナックのY座標を計算
        //float PosY = 100f;
        //if (StarScoreThresholdArray.Length > 0)
        //{
        //    PosY += 300f + ((StarScoreThresholdArray.Length - 1) * StarToStarDistance);
        //}

        //if (!IsUIVisible && SnackPos.y > PosY)
        if (!IsUIVisible && SnackPos.y > 600f)
        {
            // UIを表示
            ClearUI.SetActive(true);

            // UI表示フラグを立てる
            IsUIVisible = true;

            // カメラの動きを止める
            IsCameraStop = true;

            //// ゲームが止まっていなかったらここで止める
            //if (Time.timeScale != 0f)
            //{
            //    Time.timeScale = 0f;
            //}
        }
    }
}
