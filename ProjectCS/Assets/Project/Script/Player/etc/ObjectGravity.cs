//====================================================
// スクリプト名：ObjectGravity
// 作成者：高下
// 内容：オブジェクトの重力を管理するクラス
// 最終更新日：05/23
// 
// [Log]
// 04/21 高下 スクリプト作成 
// 04/27 荒井 アクティブフラグを追加 
// 05/09 高下 落下速度の制御を追加
// 05/23 中町 スナック落下SE実装
//====================================================
using UnityEngine;

public class ObjectGravity : MonoBehaviour
{
    [SerializeField]
    private Vector3 GravityScale = new Vector3(0.0f, -9.8f, 0.0f);     // 重力の大きさ

    [SerializeField]
    private float MaxFallSpeed = 20.0f; // 最大落下速度（負の値）

    private Rigidbody Rb;    // オブジェクトのRigidbody

    public bool IsActive = true;

    //落下時に再生する効果音(AudioClip)
    [SerializeField] private AudioClip FallSE;

    //効果音を再生するためのAudioSourceコンポーネント
    private AudioSource Audio;

    //現在落下中かどうかを示すフラグ
    private bool IsFalling = false;

    //落下音をすでに再生したかどうかのフラグ
    private bool HasPlayedFallSE = false;

    void Start()
    {
        Rb = GetComponent<Rigidbody>(); // Rigidbodyを取得

        //AudioSourceを取得
        Audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!IsActive) return;
        // 重力
        Rb.AddForce(GravityScale, ForceMode.Acceleration);

        // 落下速度を制限
        if (Rb.linearVelocity.y < -MaxFallSpeed)
        {
            Vector3 clampedVelocity = Rb.linearVelocity;
            clampedVelocity.y = -MaxFallSpeed;
            Rb.linearVelocity = clampedVelocity;
        }

        //一定以上の下向き速度があるとき、落下中と判定
        if (Rb.linearVelocity.y < -1.0f)
        {
            IsFalling = true;
        }
    }

    //他のオブジェクトと衝突したときに呼ばれる
    private void OnCollisionEnter(Collision collision)
    {
        //落下中でまだSEを再生していないとき、効果音を再生
        if(IsFalling && !HasPlayedFallSE)
        {
            if (FallSE != null && Audio != null)
            {
                //効果音を1回だけ再生
                Audio.PlayOneShot(FallSE);
            }

            //再生済みフラグを立てる
            HasPlayedFallSE = true;
        }

        //衝突したので落下状態を解除
        IsFalling = false;
    }

    //衝突が終了したときに呼ばれる
    private void OnCollisionExit(Collision collision)
    {
        //次の落下に備えてフラグをリセット
        HasPlayedFallSE = false;
    }

    public float GetGravityScaleY()
    {
        return GravityScale.y;
    }
}
