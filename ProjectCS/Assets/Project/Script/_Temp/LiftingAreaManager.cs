//====================================================
// スクリプト名：LiftingAreaManager
// 作成者：高下
// 内容：プレイヤーとターゲットとのエリアを管理するクラス
// 最終更新日：05/14
// 
// [Log]
// 04/26 高下 スクリプト作成
// 04/26 高下 リフティングパートに移行した際に、エリア内の色が変わる処理を追加
// 04/27 高下 落下地点に応じてオブジェクトを再配置するSetFallPointを追加
// 05/10 高下 ターゲットオブジェクトの地面までの距離をテキストに表示する機能を追加
// 05/13 高下 GetIsTargetContactin関数追加
// 05/14 高下 テキストに関する処理を削除
// 06/13 高下 エリア複製時に必要なコンポーネントを参照するSetTarget関数を追加
//====================================================

// ******* このスクリプトの使い方 ******* //
// 1. このスクリプトはリフティングエリアオブジェクトにアタッチする
// 2. Playerにプレイヤーオブジェクトを設定
// 3. Targetにリフティング対象オブジェクトを設定

using System;
using UnityEngine;

public class LiftingAreaManager : MonoBehaviour
{

    [SerializeField] private GameObject Player; // プレイヤーオブジェクト
    [SerializeField] private GameObject Target; // ターゲットオブジェクト
    [SerializeField] private GameClearSequence ClearSequenceComponent;

    private bool IsPlayerContacting = false; // Playerがエリア内に入ったかどうか
    private bool IsTargetContacting = false; // Targetがエリア内に入ったかどうか

    PlayerStateManager PlayerState = null; // プレイヤーの状態管理コンポーネント

    private Renderer ObjectRenderer;
    private LiftingJump LiftingJumpComponent;
    [SerializeField] private AnimationFinishTrigger AnimationFinishComponent;

    [Header("エリアの大きさ設定")]
    [SerializeField] private float Radius = 35.0f;  // 半径
    [SerializeField] private float Height = 100.0f; // 高さ

    [Header("エリアのカラー設定")]
    [SerializeField] private Color NormalColor = new Color(1.0f, 0.3f, 0.3f, 0.2f); // 通常時の色
    [SerializeField] private Color LiftingPartColor = new Color(1f, 1f, 1f, 0.2f); // リフティングパート時の色

    [SerializeField] private GameObject Effect;
    private ParticleColorChanger ParticleColorChanger;

    private ChargeJumpPlayer ChargeJumpPlayer;

    void Start()
    {
        if (!Player) Debug.LogError("プレイヤーオブジェクトが設定されていません");
        if (!Target) Debug.LogError("ターゲットオブジェクトが設定されていません");

        PlayerState = Player.GetComponent<PlayerStateManager>();
        if(!PlayerState) Debug.LogError("プレイヤーオブジェクトにPlayerStateManagerがアタッチされていません");

        ChargeJumpPlayer = Player.GetComponent<ChargeJumpPlayer>();

        ObjectRenderer = GetComponent<Renderer>();
        ObjectRenderer.material.SetColor("_Color", NormalColor);

        // エリアのサイズを初期化
        float Diameter = Radius * 2.0f;
        transform.localScale = new Vector3(Diameter, Height, Diameter);

        AllSnackManager ASM = FindFirstObjectByType<AllSnackManager>();
        if(ASM)
        {
            // データの追加
            ASM.AddSnackData(Target, gameObject);
        }

        LiftingJumpComponent = Player.GetComponent<LiftingJump>();

        if(Effect)
        {
            GetComponent<MeshRenderer>().enabled = false;
            Effect.SetActive(true);
            ParticleColorChanger = Effect.GetComponent<ParticleColorChanger>();
            Effect.transform.localScale = new Vector3(Radius * 0.4f, Radius * 0.4f, Radius * 0.4f);
            for (int i = 0; i < Effect.transform.childCount; i++)
            {
                Effect.transform.GetChild(i).transform.localScale = new Vector3(Radius * 0.4f, Radius * 0.4f, Radius * 0.4f);
            }
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
      
    }

    public void SetTarget(GameObject PlayerObj, GameObject SnackObj, GameClearSequence GCS, AnimationFinishTrigger AFT, float radius, float height)
    {
        Player = PlayerObj;
        Target = SnackObj;
        ClearSequenceComponent = GCS;
        AnimationFinishComponent = AFT;
        Radius = radius;
        Height = height;
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
            if (AnimationFinishComponent) AnimationFinishComponent.SetTargetObject(Target);
            if (LiftingJumpComponent) LiftingJumpComponent.SetTargetObject(Target);
            ClearSequenceComponent.SetSnackObject(Target);

            PlayerState.SetLiftingState(PlayerStateManager.LiftingState.LiftingPart);
            ObjectRenderer.material.SetColor("_Color", LiftingPartColor);
            if (ParticleColorChanger) ParticleColorChanger.SetColor(LiftingPartColor);

            LiftingJump LJ = Player.GetComponent<LiftingJump>();

            ChargeJumpPlayer.IsAreaJump = true;
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
            if (ParticleColorChanger) ParticleColorChanger.SetColor(NormalColor);

            ChargeJumpPlayer.IsAreaJump = true;
        }
    }

    // ターゲットの落下地点にエリアを配置する
    public void SetFallPoint(Vector3 fallPoint, Vector3 areaPoint)
    {
        areaPoint.y += Height - 0.1f;
        Debug.Log("エリア着地地点：" + areaPoint);
        transform.position = areaPoint;
    }

    // 対象オブジェクトが範囲外にあるかどうかを返す
    public bool GetIsTargetContacting()
    {
        return IsTargetContacting;
    }

    public void AdjustXZAreaPosition(Vector3 pos)
    {
        if (transform.position.x == pos.x && transform.position.z == pos.z) return;

        Vector3 tempPosition = transform.position;
        tempPosition.x = pos.x;
        tempPosition.z = pos.z;

        transform.position = tempPosition;
    }
}
