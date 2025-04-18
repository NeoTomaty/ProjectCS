//====================================================
// スクリプト名：IsHitEnemy
// 作成者：竹内
// 内容：敵と接触した際に発生する処理をまとめたもの
// 　　　プレイヤーとなるオブジェクトにアタッチする
// 最終更新日：04/17
// [Log]
// 04/17 竹内 スクリプト作成
//====================================================
using UnityEngine;

public class IsHitEnemy : MonoBehaviour
{
    public GameObject liftingObject; // バウンドしているボール（ObjectLiftingが付いている）

    private ObjectLifting liftingScript;
    private Rigidbody liftingRb;

    public float enemyHitForce = 10.0f; // 敵接触時に加える上方向の力

    void Start()
    {
        // スクリプトとRigidbodyを取得
        if (liftingObject != null)
        {
            liftingScript = liftingObject.GetComponent<ObjectLifting>();
            liftingRb = liftingObject.GetComponent<Rigidbody>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Enemyタグと接触した場合
        if (collision.gameObject.CompareTag("Enemy") && liftingScript.isLink)
        {
            if (liftingScript != null)
            {
                // isLinkをfalseにしてプレイヤーの追従を解除
                liftingScript.isLink = false;
            }

            if (liftingRb != null)
            {
                // 上方向に力を加える
                liftingRb.AddForce(Vector3.up * enemyHitForce, ForceMode.Impulse);
                Debug.Log("Enemyに当たったのでリンク解除 + 上向きに力を加えた！");
            }
        }
    }
}
