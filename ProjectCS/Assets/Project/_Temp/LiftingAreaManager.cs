//====================================================
// スクリプト名：LiftingAreaManager
// 作成者：高下
// 内容：プレイヤーとターゲットとのエリアを管理するクラス
// 最終更新日：04/26
// 
// [Log]
// 04/26 高下 スクリプト作成
// 04/26 高下 リフティングパートに移行した際に、エリア内の色が変わる処理を追加
// 04/27 高下 落下地点に応じてオブジェクトを再配置するSetFallPointを追加
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

    private Renderer ObjectRenderer;

    [SerializeField]
    private Color NormalColor = new Color(1.0f, 0.3f, 0.3f, 0.2f); // 通常時の色
    [SerializeField]
    private Color LiftingPartlColor = new Color(1f, 1f, 1f, 0.2f); // リフティングパート時の色

    void Start()
    {
        if (!Player) Debug.LogError("プレイヤーオブジェクトが設定されていません");
        if (!Target) Debug.LogError("ターゲットオブジェクトが設定されていません");

        PlayerState = Player.GetComponent<PlayerStateManager>();
        if(!PlayerState) Debug.LogError("プレイヤーオブジェクトにPlayerStateManagerがアタッチされていません");

        ObjectRenderer = GetComponent<Renderer>();
        ObjectRenderer.material.SetColor("_Color", NormalColor);
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
            ObjectRenderer.material.SetColor("_Color", LiftingPartlColor);
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
            ObjectRenderer.material.SetColor("_Color", NormalColor);
        }
    }

    // ターゲットの落下地点にエリアを配置する
    public void SetFallPoint(Vector3 fallPoint)
    {
        // エリアのY座標を調整
        fallPoint.y += transform.localScale.y - 0.1f;
        transform.position = fallPoint;
    }
}
