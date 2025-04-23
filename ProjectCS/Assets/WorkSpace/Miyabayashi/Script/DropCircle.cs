//======================================================
// DropCircleスクリプト
// 作成者：宮林
// 最終更新日：4/17
// 
// [Log]4/17 宮林　落下位置表示
//
//  targetにはSnackを入れる
//======================================================

using UnityEngine;

public class DropCircle : MonoBehaviour
{
    public Transform target; // 影を落とす対象のオブジェクト
    public float minY = 0f;  // 最低の高さ（ここからスケーリング開始）
    public float maxY = 10f; // 最大の高さ（このとき最大スケール）

    public float minScale = 0.01f; // Yが小さい時の最小スケール（ほぼ見えない）
    public float maxScale = 2.0f;  // Yが大きい時の最大スケール

    void Update()
    {
        if (target == null) return;

        // 対象のY座標を取得
        float height = target.position.y;

        
        float t = Mathf.InverseLerp(minY, maxY, height);

        // スケールを補間して計算
        float scale = Mathf.Lerp(minScale, maxScale, t);

        
        transform.localScale = new Vector3(scale, scale, 1f);

        
        transform.position = new Vector3(target.position.x, 0.2f, target.position.z);
    }

}
