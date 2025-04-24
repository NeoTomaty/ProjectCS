//====================================================
// スクリプト名：PlayerRespawn
// 作成者：竹内
// 内容：プレイヤーがRespawnタグに触れると任意の座標にリスポーンする処理
// 　　　プレイヤーにアタッチする
// 最終更新日：04/08
// 
// [Log]
// 04/22 竹内 スクリプト作成
//====================================================
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
        // 任意のリスポーン座標（インスペクターからも設定可能）
    public Vector3 respawnPosition = new Vector3(0f, 1f, 0f);

    private void OnTriggerEnter(Collider other)
    {
        // タグが "Respawn" のオブジェクトに触れたとき
        if (other.CompareTag("Respawn"))
        {
            // プレイヤーを指定座標にワープ
            transform.position = respawnPosition;
        }
    }
}
