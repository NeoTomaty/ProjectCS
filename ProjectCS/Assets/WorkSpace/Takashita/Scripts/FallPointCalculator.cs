//====================================================
// スクリプト名：FallPointCalculator
// 作成者：高下
// 内容：飛ばす対象の落下地点を計算するクラス
// 最終更新日：04/27
// 
// [Log]
// 04/27 高下 スクリプト作成
//====================================================

// ******* このスクリプトの使い方 ******* //
// 1. このスクリプトは飛ばす対象のオブジェクトにアタッチ
// 2. LAManagerにはLiftingAreaオブジェクトについているLiftingAreaManagerを設定
// 3. オブジェクトを飛ばした後のワープ後の座標が決定したときに、CalculateGroundPointを呼び出す（他スクリプトから参照）
using UnityEngine;

public class FallPointCalculator : MonoBehaviour
{
    private string GroundTag = "Ground"; // 地面として認識するタグ名

    private Vector3 FallPoint; // 落下地点

    [SerializeField] private LiftingAreaManager LAManager; // LiftingAreaManagerを参照

    void Start()
    {
        if(!LAManager) Debug.LogError("LiftingAreaManagerが設定されていません");

        CalculateGroundPoint(); // test
    }

    public void CalculateGroundPoint()
    {
        RaycastHit hit;
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;

        if (Physics.Raycast(origin, direction, out hit, Mathf.Infinity)) // 一応下方向のレイは無限に設定
        {
            // ヒットした地面オブジェクトのタグを確認する
            if (hit.collider.CompareTag(GroundTag))
            {
                Debug.Log("落下地点：" + hit.point);
                FallPoint = hit.point;

                // リフティングエリアオブジェクトを移動させる
                LAManager.SetFallPoint(FallPoint);
            }
        }
    }

    // FallPointを取得（使用するか分からないが一応作ってます）
    public Vector3 GetFallPoint()
    {
        return FallPoint; 
    }
}
