//======================================================
// [PlayerHitWall]
// 作成者：荒井修
// 最終更新日：03/31
// 
// [Log]
// 3/31　荒井　プレイヤーが壁に衝突した際の挙動を作成
// 3/31　荒井　移動の仮スクリプトを自作し動作を確認
// 4/01　荒井　Playerオブジェクトの本スクリプトに対応
//======================================================

using UnityEngine;

public class PlayerHitWall : MonoBehaviour
{
    [Tooltip("壁に衝突した際の加速量")]
    [SerializeField] private float Acceleration = 1.0f;

    // プレイヤーの移動方向と速度にアクセスするための変数
    // 同じオブジェクトにアタッチされているスクリプトであるという想定での実装
    MovePlayer MovePlayerScript;    //実際のスクリプト

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MovePlayerScript = GetComponent<MovePlayer>();

        if (MovePlayerScript == null)
        {
            Debug.LogError("プレイヤー >> MovePlayerスクリプトが見つかりません");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (MovePlayerScript == null) return;

        // 衝突したオブジェクトのタグをチェック
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "BrokenWall")
        {
            Debug.Log("プレイヤー >> 壁に当たりました");

            // プレイヤーの移動ベクトルを取得
            Vector3 PlayerMoveDirection = MovePlayerScript.GetMoveDirection;

            // 壁の接触面の法線ベクトルを取得
            Vector3 Normal = collision.contacts[0].normal;

            // 反射ベクトルを計算
            Vector3 Reflect = Vector3.Reflect(PlayerMoveDirection, Normal);

            // 反射ベクトルをプレイヤーに適用
            MovePlayerScript.SetMoveDirection(Reflect);

            // プレイヤーを加速
            MovePlayerScript.PlayerSpeedManager.SetAccelerationValue(Acceleration);
        }
    }
}
