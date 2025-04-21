//====================================================
// スクリプト名：PlayModeManager
// 作成者：高下
// 内容：プレイモードを管理するクラス
// 最終更新日：04/21
// 
// [Log]
// 04/21 高下 スクリプト作成 
// 04/21 高下 CPUのインプット操作を切り替える処理を追加
// 
//====================================================
using UnityEngine;

//***** マルチプレイ対応手順 *****//
// 1. PlayModeManagerプレハブをヒエラルキーに入れる
// 2. 2つ目のプレイヤーオブジェクト(またはCPU1)とカメラを作成する
// 3. PlayerCamera1にはプレイヤー1のカメラをセット
// 4. PlayerCamera2にはプレイヤー2(またはCPU1)のカメラをセット
// 5. ModeをSolo(1人プレイ)またはTwoPlayer(2人プレイ)に設定
// 6. PlayerObject2にPlayer2をセットする

public class PlayModeManager : MonoBehaviour
{
    public enum PlayMode
    {
        Solo,       // ソロプレイ
        TwoPlayer,  // 2人プレイ
    }

    [Tooltip("プレイヤー2のオブジェクトをアタッチ")]
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
        if (!PlayerObject2)
        {
            Debug.LogError("プレイヤー2がアタッチされてません");
        }
        if (!PlayerCamera1)
        {
            Debug.LogError("プレイヤーカメラ1がアタッチされてません");
        }
        if (!PlayerCamera2)
        {
            Debug.LogError("プレイヤーカメラ2がアタッチされてません");
        }
       
        // PlayerCamera1のCameraComponent
        Camera Camera1 = PlayerCamera1.gameObject.GetComponent<Camera>();

        // PlayerCamera2のCameraComponent
        Camera Camera2 = PlayerCamera2.gameObject.GetComponent<Camera>();

        // Player2の各操作系Component
        LRMovePlayer LRMovePlayer2 = PlayerObject2.GetComponent<LRMovePlayer>();
        JumpPlayer JumpPlayer2 = PlayerObject2.GetComponent<JumpPlayer>();
        PlayerDeceleration DecelerationPlayer2 = PlayerObject2.GetComponent<PlayerDeceleration>();

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

                Camera1.rect = new Rect(0f, 0f, 1f, 1f); // 画面全体
                break;

            // 2人プレイ専用モード(ビューポートを左右に分割)
            case PlayMode.TwoPlayer:
                this.PlayerCamera2.SetActive(true); // PlayerCamera2をアクティブにする

                // プレイヤー2の操作関連のアクティブ状態をtrueにしておく
                LRMovePlayer2.enabled = true;
                JumpPlayer2.enabled = true;
                DecelerationPlayer2.enabled = true;

                Camera1.rect = new Rect(0f, 0f, 0.5f, 1f);   // 左半分
                Camera2.rect = new Rect(0.5f, 0f, 0.5f, 1f); // 右半分
                break;

            default:
                break;
        }

        // オブジェクト破棄
        Destroy(gameObject);
    }

    
    void Update()
    {
        
    }
}
