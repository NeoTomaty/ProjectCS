//======================================================
// [SelfDestroy]
// 作成者：荒井修
// 最終更新日：04/22
// 
// [Log]
// 04/22　荒井　衝突したオブジェクトが特定のタグだったら自分自身を削除するように実装
//======================================================
using UnityEngine;

// 特定のオブジェクトと衝突した時に自分自身を削除するクラス
// 飛ばすオブジェクトにアタッチ
public class SelfDestroy : MonoBehaviour
{
    [SerializeField] private GameObject GroundObject;       // 地面オブジェクト
    [SerializeField] private GameObject DestroyerObject;    // 自分を削除するオブジェクト

    // タグ
    private string GroundTag = "Ground";        // 地面
    private string DestroyerTag = "Destroyer";  // 自分を削除するオブジェクト


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(GroundObject != null)
        {
            GroundTag = GroundObject.tag;   // 地面オブジェクトのタグを取得
        }

        if (DestroyerObject != null)
        {
            DestroyerTag = DestroyerObject.tag; // 自分を削除するオブジェクトのタグを取得
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 衝突したオブジェクトのタグを取得
        string collidedTag = collision.gameObject.tag;

        // 地面か透明なオブジェクトと衝突した場合に自分自身を削除
        if (collidedTag == GroundTag || collidedTag == DestroyerTag)
        {
            // 自分自身を削除
            Destroy(this.gameObject);
        }
    }
}
