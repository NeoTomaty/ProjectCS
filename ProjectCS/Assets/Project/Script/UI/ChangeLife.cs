//====================================================
// スクリプト名：ChangeLife
// 作成者：竹内
// 内容：ライフを減らす
// 最終更新日：04/08
// 
// [Log]
// 04/08 竹内 スクリプト作成
//====================================================
using UnityEngine;
public class ChangeLife : MonoBehaviour
{
    public LifeManager LifeManager; // ライフマネージャースクリプト
    public string TagName = "";     // 参照タグ

    void OnCollisionEnter(Collision collision)
    {
        // 参照したタグのオブジェクトに接触した場合
        if (collision.gameObject.CompareTag(TagName))
        {
            // ライフを減らす
            LifeManager.DecreaseLife();
        }
    }
}
