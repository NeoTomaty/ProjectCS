//======================================================
// [BoostGimmick]
// 作成者：荒井修
// 最終更新日：04/08
// 
// [Log]
// 04/08　荒井　プレイヤーと衝突すると継続時間が追加されるように実装
// 04/08　荒井　一度発動するとインスタンスが無効化されるように実装
//======================================================

using UnityEngine;

public class BoostGimmick : MonoBehaviour
{
    // 加速ギミックの管理クラス
    [SerializeField] private BoostGimmickManager BoostGimmickManager;

    // ギミックの継続時間
    [SerializeField] private float GimmickDurationSeconds = 5.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // ギミックの効果を追加
            BoostGimmickManager.AddGimmickDuration(GimmickDurationSeconds);

            // ギミックを無効化
            gameObject.SetActive(false);
        }
    }
}
