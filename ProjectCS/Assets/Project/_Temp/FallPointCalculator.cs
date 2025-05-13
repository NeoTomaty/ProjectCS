//====================================================
// スクリプト名：FallPointCalculator
// 作成者：高下
// 内容：飛ばす対象の落下地点を計算するクラス
// 最終更新日：05/13
// 
// [Log]
// 04/27 高下 スクリプト作成
// 05/13 高下 地面との当たり判定をタグからレイヤーに変更
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
    private Vector3 FallPoint; // 落下地点
   
    [SerializeField] private LiftingAreaManager LAManager; // LiftingAreaManagerを参照

    [Tooltip("BaseGroundのレイヤーを設定")]
    [SerializeField] private LayerMask BaseGroundLayerMask; // ベースとなる地面のレイヤー

    void Start()
    {
        if(!LAManager) Debug.LogError("LiftingAreaManagerが設定されていません");

        CalculateGroundPoint(); // test
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

    public void CalculateGroundPoint()
    {
        RaycastHit hit;
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;

        // BaseGroundLayerのみ判定
        if (Physics.Raycast(origin, direction, out hit, Mathf.Infinity, BaseGroundLayerMask)) // 一応下方向のレイは無限に設定
        {
            Debug.Log("落下地点：" + hit.point);
            FallPoint = hit.point;

            // リフティングエリアオブジェクトを移動させる
            LAManager.SetFallPoint(FallPoint);
        }
    }

    // FallPointを取得（使用するか分からないが一応作ってます）
    public Vector3 GetFallPoint()
    {
        return FallPoint; 
    }
}
