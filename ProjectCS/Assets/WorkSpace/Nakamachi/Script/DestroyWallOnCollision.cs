//DestroyWallOnCollision.cs
//作成者:中町雷我
//最終更新日:2025/04/30
//アタッチ:DestructionWallのタグが付いたオブジェクトにアタッチ
//[Log]
//04/30　中町　Playerが破壊壁に一定のスピード以上あれば破壊してエフェクトを出す処理

using UnityEngine;

public class DestroyWallOnCollision : MonoBehaviour
{
    //パーティクルエフェクトのプレハブ
    public GameObject ParticlePrefab;

    //プレイヤーの速度を管理するスクリプト
    public PlayerSpeedManager playerSpeedManager;

    //壁を破壊するために必要なプレイヤーの速度
    public float RequiredSpeed = 200.0f;

    //プレイヤーの移動を管理するスクリプト
    public MovePlayer MovePlayerScript;

    //プレイヤーの加速度
    [SerializeField] private float Acceleration = 1.0f;

    //衝突が発生したときに呼び出されるメソッド
    void OnCollisionEnter(Collision collision)
    {
        //衝突したオブジェクトの名前をデバッグログに出力
        Debug.Log("衝突検出:" + collision.gameObject.name);

        //衝突したオブジェクトがプレイヤーかどうかを確認
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("プレイヤーとの衝突");

            //プレイヤーの移動方向を取得
            Vector3 PlayerMoveDirection = MovePlayerScript.GetMoveDirection;
            Debug.Log("プレイヤーの移動方向:" + PlayerMoveDirection);

            //衝突の法線を取得
            Vector3 Normal = collision.contacts[0].normal;
            Debug.Log("衝突の法線:" + Normal);

            //プレイヤーの移動方向を法線に対して反射させる
            Vector3 Reflect = Vector3.Reflect(PlayerMoveDirection, Normal).normalized;
            Debug.Log("反射方向:" + Reflect);

            //プレイヤーの移動方向を反射方向に設定
            MovePlayerScript.SetMoveDirection(Reflect);

            //プレイヤーの速度が壁を破壊するのに十分かどうかを確認
            if (playerSpeedManager.GetPlayerSpeed>=RequiredSpeed)
            {
                Debug.Log("プレイヤーの速度が壁を破壊するのに十分");

                //パーティクルエフェクトを生成
                GameObject ParticleInstance = Instantiate(ParticlePrefab, transform.position, Quaternion.identity);
                Debug.Log("パーティクルエフェクトを生成");

                //パーティクルエフェクトを再生
                ParticleSystem particleSystem = ParticleInstance.GetComponent<ParticleSystem>();
                particleSystem.Play();
                Debug.Log("パーティクルエフェクトを再生");

                //パーティクルエフェクトを持続時間後に破壊
                Destroy(ParticleInstance, particleSystem.main.duration);
                Debug.Log("パーティクルエフェクトを持続時間後に破壊");

                //壁を破壊
                Destroy(gameObject);
                Debug.Log("壁を破壊");
            }
            else
            {
                Debug.Log("プレイヤーの速度が壁を破壊するのに不十分");
            }

            //プレイヤーの加速度の値を設定
            playerSpeedManager.SetAccelerationValue(Acceleration);
            Debug.Log("加速度の値を設定:" + Acceleration);
        }
    }
}