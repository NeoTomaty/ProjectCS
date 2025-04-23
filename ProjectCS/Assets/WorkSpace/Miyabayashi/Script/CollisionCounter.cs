//======================================================
// CollisionCounter スクリプト
// 作成者：宮林
// 最終更新日：4/23
// 
// [Log]4/23 宮林　衝突回数の計算処理を追加
//                 
//======================================================

using UnityEngine;

public class CollisionCounter : MonoBehaviour
{
    // 衝突回数を保持する変数
    private int collisionCount = 0;

    void OnCollisionEnter(Collision collision)
    {
        // 衝突した相手のタグをチェック
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Wall")|| collision.gameObject.CompareTag("Snack"))
        {
            collisionCount++;
            Debug.Log("衝突回数: " + collisionCount);
        }
    }

    // カウントを他から参照したい時用
    public int GetCollisionCount()
    {
        return collisionCount;
    }
}
