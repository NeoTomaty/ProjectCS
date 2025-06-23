//====================================================
// スクリプト名：FallPointCalculator
// 作成者：高下
// 内容：飛ばす対象の落下地点を計算するクラス
// 最終更新日：05/14
// 
// [Log]
// 04/27 高下 スクリプト作成
// 05/13 高下 地面との当たり判定をタグからレイヤーに変更
// 05/14 高下 地面の座標とオブジェクトの落下座標を分けて処理をすることに変更
// 06/13 高下 スナック複製時に必要なコンポーネントを参照するSetTarget関数を追加
// 06/23 高下 あらかじめ設定されたワープ先で落下地点を計算するように変更
//====================================================

// ******* このスクリプトの使い方 ******* //
// 1. このスクリプトは飛ばす対象のオブジェクトにアタッチ
// 2. LAManagerにはLiftingAreaオブジェクトについているLiftingAreaManagerを設定
// 3. オブジェクトを飛ばした後のワープ後の座標が決定したときに、CalculateGroundPointを呼び出す（他スクリプトから参照）
// 4. BaseGroundLayerMaskにベースとなる地面（一番下の地面）のレイヤーを設定する
using UnityEngine;

public class FallPointCalculator : MonoBehaviour
{
    private string GroundTag = "Ground"; // 地面オブジェクトのタグ
    private Vector3 FallPoint;           // 落下地点
    private Vector3 GroundPoint;         // 地面の座標
    private bool IsGround = false;       // 地面に着地しているかどうか
    private float SnackOffsetY = 0f;

    [SerializeField] private LiftingAreaManager LAManager; // LiftingAreaManagerを参照
   
    [Tooltip("BaseGroundのレイヤーを設定")]
    [SerializeField] private LayerMask BaseGroundLayerMask; // ベースとなる地面のレイヤー

    private BlownAway_Ver3 BAV3;

    void Start()
    {
        if(!LAManager) Debug.LogError("LiftingAreaManagerが設定されていません");

        //CalculateGroundPoint(); // test

        SnackOffsetY = GetComponent<Collider>().bounds.extents.y;

        BAV3 = GetComponent<BlownAway_Ver3>();

        // クローンで複製されたスナックのみ初回の落下地点の計算を行う
        if (gameObject.name.EndsWith("(Clone)"))
        {
            CalculateGroundPoint();
        }
    }

    public void SetTarget(LiftingAreaManager LAM)
    {
        LAManager = LAM;
    }

    // スナックが地面に接しているかつリフティングエリア外のときは
    // リフティングエリアの位置を再調整する
    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag(GroundTag)) return;

        if(!LAManager.GetIsTargetContacting())
        {
            CalculateGroundPoint(); // リフティングエリアのポイントを再計算
            Debug.Log("リフティングエリア再計算実行");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(GroundTag)) return;

        IsGround = true; // 地面に着いている
    }

    void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag(GroundTag)) return;

        IsGround = false; // 地面に着いていない
    }

    public void CalculateGroundPoint()
    {
        RaycastHit hit;
        Vector3 origin = BAV3.NextWarpPosition;
        Vector3 direction = Vector3.down;

        // BaseGroundLayerのみ判定
        if (Physics.Raycast(origin, direction, out hit, Mathf.Infinity, BaseGroundLayerMask)) // 一応下方向のレイは無限に設定
        {
            Debug.Log("落下地点：" + hit.point);
            GroundPoint = hit.point;

            // リフティングエリアオブジェクトを移動させる
            LAManager.SetFallPoint(GroundPoint);
        }

        // ターゲットオブジェクトの落下地点を計算（すべての地面に対して実行）
        if (Physics.Raycast(origin, direction, out hit, Mathf.Infinity))
        {
            if(hit.collider.CompareTag(GroundTag))
            {
               FallPoint = hit.point;
            }
        }
    }

    // FallPointを取得（使用するか分からないが一応作ってます）
    public Vector3 GetFallPoint()
    {
        return FallPoint; 
    }

    // 地面に着いているどうかを取得
    public bool GetIsGround()
    {
        return IsGround;
    }

    public float GetDistanceToGround()
    {
        return (transform.position.y - SnackOffsetY) - GroundPoint.y;
    }
}
