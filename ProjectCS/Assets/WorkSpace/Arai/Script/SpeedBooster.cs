//======================================================
// [SpeedBooster]
// 作成者：荒井修
// 最終更新日：04/07
// 
// [Log]
// 04/07　荒井　プレイヤーとの衝突で作動し、一定時間で効果が終了するように仮組み
// 04/07　荒井　実際にプレイヤーの速度を変化させて確認
//======================================================

using UnityEngine;

public class SpeedBooster : MonoBehaviour
{
    // プレイヤーの速度管理クラス
    [SerializeField] private PlayerSpeedManager PlayerSpeedManager;

    // ギミック作動時の加速量
    [SerializeField] private float AccelerationForGimmick = 500.0f;

    // ギミックの継続時間
    [SerializeField] private float GimmickDurationSeconds = 5.0f;
    private float GimmickTimer = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GimmickTimer <= 0.0f) return;

        GimmickTimer -= Time.deltaTime;

        // ギミックの効果が切れたら元の速度に戻す
        if (GimmickTimer <= 0.0f)
        {
            // プレイヤーを減速させる
            PlayerSpeedManager.SetAccelerationValue(-AccelerationForGimmick);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 衝突したオブジェクトのタグをチェック
        if (collision.gameObject.tag == "Player")
        {
            // プレイヤーを加速させる
            PlayerSpeedManager.SetAccelerationValue(AccelerationForGimmick);

            // ギミックの効果時間を設定
            GimmickTimer = GimmickDurationSeconds;
        }
    }
}
