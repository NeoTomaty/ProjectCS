//DestroyWallOnCollision.cs
//作成者:中町雷我
//最終更新日:2025/05/04
//アタッチ:DestructionWallのタグが付いたオブジェクトにアタッチ
//[Log]
//04/30　中町　Playerが破壊壁に一定のスピード以上あれば破壊してエフェクトを出す処理
//05/04　荒井　プレイヤーを反射させる処理を関数化
//05/04　荒井　壁が破壊される時にプレイヤーを反射させない処理を追加
//05/04　荒井　反射 or 貫通を切り替えられるように変更
//06/04　中町　壁破壊SE実装

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

    // 壁破壊時の貫通フラグ
    [SerializeField] private bool IsPenetration = false;

    //壁が破壊されたときに再生するサウンドエフェクト(AudioClip)
    [SerializeField] private AudioClip DestorySE;

    //サウンド再生用のAudioSource
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //AudioSourceが存在しないときは新しく追加する
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    //プレイヤーを反射させる関数
    private void ReflectPlayer(Vector3 Normal)
    {
        //プレイヤーの移動方向を法線に対して反射させる
        Vector3 Reflect = Vector3.Reflect(MovePlayerScript.GetMoveDirection, Normal).normalized;

        //プレイヤーの移動方向を反射方向に設定
        MovePlayerScript.SetMoveDirection(Reflect);
    }

    //衝突が発生したときに呼び出されるメソッド
    void OnCollisionEnter(Collision collision)
    {
        //衝突したオブジェクトがプレイヤーかどうかを確認
        if(collision.gameObject.CompareTag("Player"))
        {
            //衝突の法線を取得
            Vector3 Normal = collision.contacts[0].normal;

            //プレイヤーの速度が壁を破壊するのに十分かどうかを確認
            if (playerSpeedManager.GetPlayerSpeed >= RequiredSpeed)
            {
                // 貫通フラグがfalseの場合、プレイヤーを反射させる
                if (!IsPenetration) ReflectPlayer(Normal);

                //サウンドエフェクトが設定されていれば再生
                if(DestorySE != null && audioSource != null)
                {
                    audioSource.PlayOneShot(DestorySE);
                }

                //パーティクルエフェクトを生成
                GameObject ParticleInstance = Instantiate(ParticlePrefab, transform.position, Quaternion.Euler(-90f,0f,0f));

                //パーティクルエフェクトを持続時間後に破壊
                Destroy(ParticleInstance, ParticleInstance.GetComponent<ParticleSystem>().main.duration);

                //壁を破壊
                Destroy(gameObject);
            }
            else
            {
                // プレイヤーを反射させる
                ReflectPlayer(Normal);
            }

            //プレイヤーの加速度の値を設定
            playerSpeedManager.SetAccelerationValue(Acceleration);
        }
    }
}