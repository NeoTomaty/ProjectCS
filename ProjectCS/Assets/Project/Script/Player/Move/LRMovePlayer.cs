//====================================================
// スクリプト名：LRMovePlayer
// 作成者：高下
// 内容：プレイヤーの左右移動処理
// 
// [Log]
// 03/27 高下 スクリプト作成 
// 03/31 高下 左右移動処理追加
// 04/01 コントローラー操作追加
// 04/08 左右移動を速度に依存させる
// 04/23 高下 入力に関する仕様変更(PlayerInput(InputActionAsset))
// 05/15 竹内 カメラの前方向に依存した左右移動に変更
//====================================================
using UnityEngine;
using UnityEngine.InputSystem;

public class LRMovePlayer : MonoBehaviour
{
    [Header("コード内で参照するもの")]
    public PlayerSpeedManager PlayerSpeedManager; // 速度管理クラス
    public MovePlayer MovePlayer; // プレイヤー移動クラス

    [Header("カーブのしやすさ")]
    [SerializeField] private float TurnSmooth = 10.0f;  // カーブのしやすさ

    private PlayerInput PlayerInput; // プレイヤーの入力を管理するcomponent
    private InputAction TurnAction;  // 左右移動用のInputAction

    // プレイヤーの左右移動方向を示す変数
    float moveX;

    private void Awake()
    {
        // 自分にアタッチされているPlayerInputを取得
        PlayerInput = GetComponent<PlayerInput>();

        // 対応するInputActionを取得
        TurnAction = PlayerInput.actions["LRTurn"];
    }
    void Start()
    {
        if (PlayerSpeedManager == null)
        {
            Debug.LogWarning("MovePlayerスクリプトがアタッチされていません。");
        }
    }

    void Update()
    {
        moveX = TurnAction.ReadValue<float>();
    }


    void FixedUpdate()
    {
        if (PlayerSpeedManager == null) return;

        // 速度を取得
        float speed = PlayerSpeedManager.GetPlayerSpeed;


        // 現在の速度に応じて曲がりやすくする
        // カーブ量 = 回転速度 × 速度 × deltaTime
        float rotationAmount = speed * TurnSmooth * Time.deltaTime;


        // 回転処理（左右）
        if (Mathf.Abs(moveX) > 0.1f)    // 絶対値より大きな値の場合
        {
            float angle = rotationAmount * Mathf.Sign(moveX);   // 符号を取得
            MovePlayer.SetMoveDirection(Quaternion.Euler(0, angle, 0) * MovePlayer.GetMoveDirection);
        }

    }
}
