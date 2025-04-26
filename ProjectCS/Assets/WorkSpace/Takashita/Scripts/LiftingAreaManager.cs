//====================================================
// スクリプト名：LiftingAreaManager
// 作成者：高下
// 内容：プレイヤーとターゲットとのエリアを管理するクラス
// 最終更新日：04/26
// 
// [Log]
// 04/26 高下 スクリプト作成
//
//====================================================

// ******* このスクリプトの使い方 ******* //
// 1. このスクリプトはリフティングエリアオブジェクトにアタッチする
// 2. Playerにプレイヤーオブジェクトを設定
// 3. Targetにリフティング対象オブジェクトを設定

using UnityEngine;

public class LiftingAreaManager : MonoBehaviour
{

    [SerializeField] private GameObject Player; // プレイヤーオブジェクト
    [SerializeField] private GameObject Target; // ターゲットオブジェクト

    private bool IsPlayerContacting = false; // Playerがエリア内に入ったかどうか
    private bool IsTargetContacting = false; // Targetがエリア内に入ったかどうか

    PlayerStateManager PlayerState = null; // プレイヤーの状態管理コンポーネント

    void Start()
    {
        if (!Player) Debug.LogError("プレイヤーオブジェクトが設定されていません");
        if (!Target) Debug.LogError("ターゲットオブジェクトが設定されていません");

        PlayerState = Player.GetComponent<PlayerStateManager>();
        if(!PlayerState) Debug.LogError("プレイヤーオブジェクトにPlayerStateManagerがアタッチされていません");
    }

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーがエリア内に入ったかどうか判定
        if (other.gameObject == Player)
        {
            IsPlayerContacting = true;
        }

        // ターゲットがエリア内に入ったかどうか判定
        if (other.gameObject == Target)
        {
            IsTargetContacting = true;
        }

        // 両方のオブジェクトが入っている場合、リフティングパートに移行する
        if(IsPlayerContacting && IsTargetContacting)
        {
            PlayerState.SetLiftingState(PlayerStateManager.LiftingState.LiftingPart);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // プレイヤーがエリア外に出たかどうか判定
        if (other.gameObject == Player)
        {
            IsPlayerContacting = false;
        }

        // プレイヤーがエリア外に出たかどうか判定
        if (other.gameObject == Target)
        {
            IsTargetContacting = false;
        }

        // どちらか片方でもエリア外に出たら、通常状態に切り替える
        if(!IsPlayerContacting || !IsTargetContacting)
        {
            PlayerState.SetLiftingState(PlayerStateManager.LiftingState.Normal);
        }
    }
}
