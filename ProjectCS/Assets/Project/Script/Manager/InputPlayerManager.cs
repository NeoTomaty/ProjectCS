//====================================================
// スクリプト名：InputPlayerManager
// 作成者：高下
// 内容：プレイヤーの入力を管理するクラス
// 最終更新日：04/25
// 
// [Log]
// 04/21 高下 スクリプト作成 
// 04/21 高下 CPUのインプット操作を切り替える処理を追加
// 04/23 高下 コントローラーの割り当て処理を追加
// 04/25 高下 スクリプト名変更、1人モード固定に修正
//====================================================
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

// ******* このスクリプトの使い方 ******* //
// 1. Playerオブジェクトにこのスクリプトをアタッチ
// 2. PlayerオブジェクトにPlayerInputコンポーネントを追加
// 3. PlayerInputのActionsに「PlayerInputActions」を設定
// 4. PlayerInputのBehaviorに「InvokeUnityEvents」を設定

public class InputPlayerManager : MonoBehaviour
{
    void Start()
    {
        // PlayerInputComponentを取得
        PlayerInput PlayerInputComponent = gameObject.GetComponent<PlayerInput>();
       
        // コントローラー取得
        var gamepads = Gamepad.all;
        
        // 接続コントローラー1台以上
        if(gamepads.Count >= 1)
        {
            // コントローラー操作に設定
            AssignInputToPlayer(PlayerInputComponent, gamepads[0], "Gamepad", "プレイヤー");
        }
        // 接続コントローラー0台
        else if (gamepads.Count == 0)
        {
            // キーボード操作に設定
            AssignInputToPlayer(PlayerInputComponent, Keyboard.current, "Keyboard", "プレイヤー");
        }
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