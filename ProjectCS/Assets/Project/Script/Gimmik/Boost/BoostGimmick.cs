//======================================================
// [BoostGimmick]
// 作成者：荒井修
// 最終更新日：06/27
// 
// [Log]
// 04/08　荒井　プレイヤーと衝突すると継続時間が追加されるように実装
// 04/08　荒井　一度発動するとインスタンスが無効化されるように実装
// 05/30　中町　パワーアップSE実装
// 06/27　中町　パワーアップSE音量調整実装
//======================================================

using UnityEngine;

public class BoostGimmick : MonoBehaviour
{
    // 加速ギミックの管理クラス
    [SerializeField] private BoostGimmickManager BoostGimmickManager;

    // ギミックの継続時間
    [SerializeField] private float GimmickDurationSeconds = 5.0f;

    //効果音を再生するためのAudioSource
    [SerializeField] private AudioSource SeAudioSource;

    //効果音のAudioClip
    [SerializeField] private AudioClip BoostSE;

    //効果音の音量(0.0～1.0)
    [Range(0.0f, 1.0f)]
    [SerializeField] private float BoostSEVolume = 1.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // ギミックの効果を追加
            BoostGimmickManager.AddGimmickDuration(GimmickDurationSeconds);

            //効果音を再生
            if(SeAudioSource != null && BoostSE != null)
            {
                SeAudioSource.volume = BoostSEVolume;
                SeAudioSource.PlayOneShot(BoostSE);
            }

            // ギミックを無効化
            gameObject.SetActive(false);
        }
    }
}
