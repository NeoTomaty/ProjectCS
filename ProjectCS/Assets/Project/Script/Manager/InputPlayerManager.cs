//====================================================
// スクリプト名：InputPlayerManager
// 作成者：高下
// 内容：プレイヤーの入力を管理するクラス
// 最終更新日：05/29
// 
// [Log]
// 04/21 高下 スクリプト作成 
// 04/21 高下 CPUのインプット操作を切り替える処理を追加
// 04/23 高下 コントローラーの割り当て処理を追加
// 04/25 高下 スクリプト名変更、1人モード固定に修正
// 05/29 中町 UI選択SE実装
// 06/27 中町 UI選択SE音量調整実装
//====================================================
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ******* このスクリプトの使い方 ******* //
// 1. Playerオブジェクトにこのスクリプトをアタッチ
// 2. PlayerオブジェクトにPlayerInputコンポーネントを追加
// 3. PlayerInputのActionsに「PlayerInputActions」を設定
// 4. PlayerInputのBehaviorに「InvokeUnityEvents」を設定

public class InputPlayerManager : MonoBehaviour
{
    [Header("SE設定")]

    //効果音を再生するAudioSource
    [SerializeField] private AudioSource audioSource;

    //UI移動時に再生する効果音
    [SerializeField] private AudioClip MoveSE;

    //決定時に再生する効果音
    [SerializeField] private AudioClip DecideSE;

    //最後に選択されていたUIオブジェクト
    private GameObject LastSelected;

    //音量調整(0.0～1.0)
    [Range(0.0f, 1.0f)]
    [SerializeField] private float SEVolume = 0.5f;

    void Start()
    {
        //AudioSourceの音量を初期値に設定
        if (audioSource != null)
        {
            audioSource.volume = SEVolume;
        }

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

        //現在選択されているUIオブジェクトを取得
        LastSelected = EventSystem.current.currentSelectedGameObject;

        //全てのボタンにクリックSEを追加
        AddClickSEToAllButtons();
    }

    void Update()
    {
        //現在選択されているUIオブジェクトを取得
        GameObject CurrentSelected = EventSystem.current.currentSelectedGameObject;

        //選択されているUIが前回と異なるとき(UI移動があったとき)
        if (CurrentSelected != null && CurrentSelected != LastSelected)
        {
            //効果音を再生
            PlayMoveSE();

            //現在の選択を記録
            LastSelected = CurrentSelected;
        }
    }

    //入力デバイスをプレイヤーに割り当てる処理
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

    //UI移動時の効果音を再生する処理
    private void PlayMoveSE()
    {
        if(audioSource != null && MoveSE != null)
        {
            if(audioSource != null && MoveSE != null)
            {
                //一回だけ効果音を再生する
                audioSource.PlayOneShot(MoveSE);
            }
        }
    }

    //UI決定時に鳴らすSE
    private void PlayDecideSE()
    {
        if(audioSource != null && DecideSE != null)
        {
            audioSource.PlayOneShot(DecideSE);
        }
    }

    //シーン内の全てのボタンに「決定SE」を追加する処理
    private void AddClickSEToAllButtons()
    {
        Button[] buttons = FindObjectsOfType<Button>(true);

        foreach (var button in buttons)
        {
            button.onClick.AddListener(() => PlayDecideSE());
        }
    }
}