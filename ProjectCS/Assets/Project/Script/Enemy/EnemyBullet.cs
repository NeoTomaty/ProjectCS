//======================================================
// [�X�N���v�g��]EnemyBullet
// �쐬�ҁF�X�e���S
// �ŏI�X�V���F4/01
//
// [Log]
// 3/31  �X�e�@�X�N���v�g�쐬
// 4/17  EnemyBulletで１減るように
//======================================================

using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 1;  // ダメージ（※使わなくてもOK）
    public string playerTag = "Player"; // プレイヤーのタグ名を指定

    private void OnCollisionEnter(Collision collision)
    {
        // 衝突したオブジェクトのタグがプレイヤータグと一致する場合
        if (collision.gameObject.CompareTag(playerTag))
        {
            // LifeManager を探してライフを減らす
            LifeManager lifeManager = FindObjectOfType<LifeManager>();
            if (lifeManager != null)
            {
                lifeManager.DecreaseLife();
            }

            Destroy(gameObject); // 弾を消す
        }
    }

    private void Start()
    {
        Destroy(gameObject, 30f); // 30秒後に消える
    }
}