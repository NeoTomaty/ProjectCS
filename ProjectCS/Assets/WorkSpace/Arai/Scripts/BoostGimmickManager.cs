//======================================================
// [BoostGimmickManager]
// 作成者：荒井修
// 最終更新日：04/08
// 
// [Log]
// 04/07　荒井　プレイヤーとの衝突で作動し、一定時間で効果が終了するように仮組み
// 04/07　荒井　実際にプレイヤーの速度を変化させて確認
// 04/08　荒井　一度使ったら無効化されるように実装
// 04/08　荒井　マネージャーオブジェクトにアタッチする管理クラスへ変更
// 04/08　荒井　加速が重複して発動しないように修正
//======================================================

using UnityEngine;

public class BoostGimmickManager : MonoBehaviour
{
    // プレイヤーの速度管理クラス
    [SerializeField] private PlayerSpeedManager PlayerSpeedManager;

    // ギミック作動時の加速量
    [SerializeField] private float AccelerationForGimmick = 500.0f;

    // ギミックの継続時間上限
    [SerializeField] private float GimmickDurationSecondsLimit = 5.0f;
    private float GimmickTimer = 0.0f;

    public void AddGimmickDuration(float addTime)
    {
        if (addTime <= 0.0f) return;

        // ギミックの効果が作動し始めた時だけ加速
        if (GimmickTimer <= 0.0f)
        {
            GimmickTimer = 0.0f;
            PlayerSpeedManager.SetAccelerationValue(AccelerationForGimmick);
        }

        // ギミックの効果時間を追加
        GimmickTimer += addTime;
        if (GimmickTimer > GimmickDurationSecondsLimit)
        {
            GimmickTimer = GimmickDurationSecondsLimit;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GimmickTimer <= 0.0f) return;

        GimmickTimer -= Time.deltaTime;

        // ギミックの効果が切れたら元の速度に戻す
        if (GimmickTimer <= 0.0f)
        {
            // プレイヤーを減速させる
            PlayerSpeedManager.SetAccelerationValue(-AccelerationForGimmick);
        }
    }
}
