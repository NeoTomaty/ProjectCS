//======================================================
// [�X�N���v�g��]Enemy1
// �쐬�ҁF�X�e���S
// �ŏI�X�V���F4/01
//
// [Log]
// 3/31  �X�e�@�X�N���v�g�쐬
//======================================================

using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 1;  // ダメージ
    public string playerTag = "Player"; // プレイヤーのタグ名を指定

    private void OnCollisionEnter(Collision collision)
    {
        // 衝突したオブジェクトのタグがプレイヤータグと一致する場合
        if (collision.gameObject.CompareTag(playerTag))
        {
            // Player クラスが見つからない場合に備えて TryGetComponent を使用
            if (collision.gameObject.TryGetComponent(out Component playerComponent))
            {
                var takeDamageMethod = playerComponent.GetType().GetMethod("TakeDamage");
                if (takeDamageMethod != null)
                {
                    takeDamageMethod.Invoke(playerComponent, new object[] { damage });
                }
            }
            Destroy(gameObject); // 弾を消す
        }
    }

    private void Start()
    {
        Destroy(gameObject, 30f); // 30秒後に消える
    }
}