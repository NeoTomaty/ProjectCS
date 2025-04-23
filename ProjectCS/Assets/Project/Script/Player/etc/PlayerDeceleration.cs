//======================================================
// [スクリプト名]PlayerDeceleration
// 作成者：宮林朋輝
// 最終更新日：4/1
// 
// [Log]
// 3/31 宮林　スクリプト作成
// 3/31 宮林　減速処理仮実装
// 4/1  宮林  コントローラー操作追加
// 4/1  宮林　減速処理実装
// 4/23 高下　入力に関する仕様変更(PlayerInput(InputActionAsset))
//======================================================
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerDeceleration : MonoBehaviour
{
    //プレイヤーの移動速度を設定する変数
    public float Deceleration = 5.0f;//減速量

    public PlayerSpeedManager PlayerSpeedManager; // 速度管理クラス

    private bool isHoldingKey = false;
    private float HoldTime = 0.0f;
    public float DecelerationsInterval = 1.0f; // 減速間隔

    private PlayerInput PlayerInput;         // プレイヤーの入力を管理するcomponent
    private InputAction DecelerationAction;  // 減速用のInputAction

    void Start()
    {
        // 自分にアタッチされているPlayerInputを取得
        PlayerInput = GetComponent<PlayerInput>();

        // 対応するInputActionを取得
        DecelerationAction = PlayerInput.actions["Deceleration"];
    }

    void Update()
    {
        //ここで現在の速度を受け取る

        if (DecelerationAction.ReadValue<float>() < -0.5f)
        {
            // キーを押した瞬間の処理
            if ( PlayerSpeedManager.GetPlayerSpeed> Deceleration)
            {
                //減速
               PlayerSpeedManager.SetDecelerationValue(Deceleration);
            }
        }

        if (DecelerationAction.ReadValue<float>() < -0.5f)
        {
            if (!isHoldingKey)
            {
                // 長押し開始時の処理
                isHoldingKey = true;
                HoldTime = 0.0f; // 初期化
            }

            HoldTime += Time.deltaTime;

            if (HoldTime >= DecelerationsInterval)
            {
                if (PlayerSpeedManager.GetPlayerSpeed > Deceleration)
                {
                    //減速
                    PlayerSpeedManager.SetDecelerationValue(Deceleration);
                }
                HoldTime = 0.0f; // 減速後にリセット
            }
        }

        if (DecelerationAction.ReadValue<float>() < -0.5f)
        {
            // キーを離した時の処理
            isHoldingKey = false;
        }
    }
}
