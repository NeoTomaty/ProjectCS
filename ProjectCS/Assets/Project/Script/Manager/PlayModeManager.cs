//====================================================
// スクリプト名：PlayModeManager
// 作成者：高下
// 内容：プレイモードを管理するクラス
// 最終更新日：04/21
// 
// [Log]
// 04/21 高下 スクリプト作成 
// 04/21 高下 CPUのインプット操作を切り替える処理を追加
// 04/23 高下 コントローラーの割り当て処理を追加
//====================================================
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

//***** マルチプレイ対応手順 *****//
//  1. PlayModeManagerプレハブをヒエラルキーに入れる
//  2. 2つ目のプレイヤーオブジェクト(またはCPU)とプレイヤー2のカメラを作成する
//  3. Player1とPlayer2（またはCPU）のオブジェクトをPlayerObject1とPlayerObject2にそれぞれ設定する
//  4. PlayerCamera1とPlayerCamera2のオブジェクトをPlayerCamera1とPlayerCamera2にそれぞれ設定する
//  5. ModeをSolo(1人プレイ)またはTwoPlayer(2人プレイ)に設定
//  6. Player1オブジェクトとPlayer2オブジェクトにPlayerInputComponentを追加（プレイヤーオブジェクト側の設定）
//  7. PlayerInputのActionsに「PlayerInputActions」を設定（プレイヤーオブジェクト側の設定）
//  8. PlayerInputのBehaviorに「InvokeUnityEvents」を設定（プレイヤーオブジェクト側の設定）
//  9. Player1InputとPlayer2InputそれぞれのプレイヤーのPlayerInputを設定

public class PlayModeManager : MonoBehaviour
{
    public enum PlayMode
    {
        Solo,       // ソロプレイ
        TwoPlayer,  // 2人プレイ
    }

    [Tooltip("プレイヤー1のオブジェクトをアタッチ")]
    [SerializeField]
    private GameObject PlayerObject1;
    [Tooltip("プレイヤー2またはCPUのオブジェクトをアタッチ")]
    [SerializeField]
    private GameObject PlayerObject2;
    [Tooltip("プレイヤー1のカメラオブジェクトをアタッチ")]
    [SerializeField]
    private GameObject PlayerCamera1;
    [Tooltip("プレイヤー2のカメラオブジェクトをアタッチ")]
    [SerializeField]
    private GameObject PlayerCamera2;
    [Tooltip("選択モード")]
    [SerializeField]
    private PlayMode Mode;

    void Start()
    {
        if (!PlayerObject1)
        {
            Debug.LogError("プレイヤー1がアタッチされてません");
        }
        if (!PlayerObject2)
        {
            Debug.LogError("プレイヤー2またはCPUがアタッチされてません");
        }
        if (!PlayerCamera1)
        {
            Debug.LogError("プレイヤーカメラ1がアタッチされてません");
        }
        if (!PlayerCamera2)
        {
            Debug.LogError("プレイヤーカメラ2がアタッチされてません");
        }

        // PlayerInputComponentをそれぞれ取得
        PlayerInput Player1Input = PlayerObject1.GetComponent<PlayerInput>();
        PlayerInput Player2Input = PlayerObject2.GetComponent<PlayerInput>();

        // コントローラー取得
        var gamepads = Gamepad.all;

        // 接続コントローラー0台、かつ2人モードの場合
        // 強制的にソロモードに切り替え（仮）
        if (gamepads.Count == 0 && Mode == PlayMode.TwoPlayer)
        {
            Mode = PlayMode.Solo;
            Debug.Log("コントローラーの接続が確認されないため、ソロモードに切り替えました");
        }

        // 接続コントローラー1台以上、かつソロモード
        if(gamepads.Count >= 1 && Mode == PlayMode.Solo)
        {
            // コントローラー操作に設定
            AssignInputToPlayer(Player1Input, gamepads[0], "Gamepad", "プレイヤー(ソロ)");
        }
        // 接続コントローラー0台、かつソロモード
        else if (gamepads.Count == 0 && Mode == PlayMode.Solo)
        {
            // キーボード操作に設定
            AssignInputToPlayer(Player1Input, Keyboard.current, "Keyboard", "プレイヤー(ソロ)");
        }

        // 接続コントローラー2台以上、かつ2人モード
        if (gamepads.Count >= 2 && Mode == PlayMode.TwoPlayer)
        {
            // プレイヤー1とプレイヤー2ともにコントローラー操作
            AssignInputToPlayer(Player1Input, gamepads[0], "Gamepad", "プレイヤー1");
            AssignInputToPlayer(Player2Input, gamepads[1], "Gamepad", "プレイヤー2");
        }
        // 接続コントローラー1台、かつ2人モード
        else if (gamepads.Count == 1 && Mode == PlayMode.TwoPlayer)
        {
            // プレイヤー1はコントローラー操作、プレイヤー2はキーボード操作
            AssignInputToPlayer(Player1Input, gamepads[0], "Gamepad", "プレイヤー1");
            AssignInputToPlayer(Player2Input, Keyboard.current, "Keyboard", "プレイヤー2");
        }
       
        // PlayerCamera1のCameraComponent
        Camera Camera1 = PlayerCamera1.gameObject.GetComponent<Camera>();

        // PlayerCamera2のCameraComponent
        Camera Camera2 = PlayerCamera2.gameObject.GetComponent<Camera>();

        // Player2の各操作系Component
        LRMovePlayer LRMovePlayer2 = PlayerObject2.GetComponent<LRMovePlayer>();
        JumpPlayer JumpPlayer2 = PlayerObject2.GetComponent<JumpPlayer>();
        PlayerDeceleration DecelerationPlayer2 = PlayerObject2.GetComponent<PlayerDeceleration>();
        CameraFunction CameraFunction2 = PlayerCamera2.GetComponent<CameraFunction>();

        switch (Mode)
        {
            // ソロ専用モード(ビューポートを画面全体に拡張)
            case PlayMode.Solo:
                this.PlayerCamera2.SetActive(false); // PlayerCamera2は非アクティブにしておく

                // ソロプレイの場合プレイヤー2はCPU扱いなので
                // 操作関連のアクティブ状態をfalseにしておく
                LRMovePlayer2.enabled = false;
                JumpPlayer2.enabled = false;
                DecelerationPlayer2.enabled = false;
                CameraFunction2.enabled = false;

                Camera1.rect = new Rect(0f, 0f, 1f, 1f); // 画面全体
                break;

            // 2人プレイ専用モード(ビューポートを左右に分割)
            case PlayMode.TwoPlayer:
                this.PlayerCamera2.SetActive(true); // PlayerCamera2をアクティブにする

                // プレイヤー2の操作関連のアクティブ状態をtrueにしておく
                LRMovePlayer2.enabled = true;
                JumpPlayer2.enabled = true;
                DecelerationPlayer2.enabled = true;
                CameraFunction2.enabled = true;

                Camera1.rect = new Rect(0f, 0f, 0.5f, 1f);   // 左半分
                Camera2.rect = new Rect(0.5f, 0f, 0.5f, 1f); // 右半分
                break;

            default:
                break;
        }

        // オブジェクト破棄
        Destroy(gameObject);
    }

    private void AssignInputToPlayer(PlayerInput playerInput, InputDevice device, string controlScheme, string playerLabel)
    {
        if (playerInput == null || device == null)
        {
            Debug.LogWarning($"{playerLabel} の入力割り当てに失敗：デバイスが null");
            return;
        }

        // 入力を一旦無効化
        playerInput.DeactivateInput();

        // 現在のInputUserとデバイスをペアリング
        InputUser.PerformPairingWithDevice(device, playerInput.user);

        // ControlSchemeを明示的に指定
        playerInput.SwitchCurrentControlScheme(controlScheme, device);

        // 入力を再有効化
        playerInput.ActivateInput();

        Debug.Log($"{playerLabel} の {controlScheme} 入力が割り当てられました");
    }
}