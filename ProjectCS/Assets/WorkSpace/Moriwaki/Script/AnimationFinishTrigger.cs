//====================================================
// スクリプト名：AnimationFinishTrigger
// 作成者：森脇
// 内容：フィニッシュ時のアニメーション終了トリガー
// [Log]
// 06/13　森脇 カメラの制御フラグ追加
// 06/19　森脇  SE再生機能を追加
//====================================================

using UnityEngine;
using System.Collections;

public class AnimationFinishTrigger : MonoBehaviour
{
    [SerializeField] private GameObject snackObject;

    [Header("カメラ連携")]
    [SerializeField] private CameraFunction cameraFunction;

    [SerializeField] private Vector3 specialViewPosition = new Vector3(0, 2, -4);

    [Header("エフェクト")]
    [Tooltip("ヒット時に再生するエフェクト")]
    [SerializeField] private GameObject hitEffectPrefab;

    [Tooltip("エフェクト再生位置（例：snackの位置）")]
    [SerializeField] private Transform effectSpawnPoint;

    [Header("SE")]
    [Tooltip("ヒット時に再生するSE")]
    [SerializeField] private AudioClip hitSound;

    [Tooltip("SEを再生するAudioSource")]
    [SerializeField] private AudioSource audioSource;

    [Header("ヒットストップ時間")]
    [Tooltip("ヒット時に全体を止める秒数")]
    [SerializeField] private float hitStopDuration = 0.5f;

    [Tooltip("停止対象のAnimator（例：プレイヤー）")]
    [SerializeField] private Animator playerAnimator;

    public void OnKickImpact()
    {
        Debug.Log("キックが当たったタイミングで呼び出された");
        StartCoroutine(HitStopRoutine());
    }

    private IEnumerator HitStopRoutine()
    {
        // SE再生とエフェクト生成
        // SE再生
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        // エフェクト生成
        if (hitEffectPrefab != null && effectSpawnPoint != null)
        {
            Instantiate(hitEffectPrefab, effectSpawnPoint.position, Quaternion.identity);
        }

        // アニメーション停止（player）
        if (playerAnimator != null)
        {
            playerAnimator.speed = 0f;
        }

        // ヒットストップ
        Time.timeScale = 0f;

        // リアルタイムで待つ（タイムスケール0でも動くように）
        yield return new WaitForSecondsRealtime(hitStopDuration);

        // ヒットストップ解除
        Time.timeScale = 1f;

        // アニメーション再開
        if (playerAnimator != null)
        {
            playerAnimator.speed = 1f;
        }

        // snack 側のヒットストップ解除
        snackObject.GetComponent<BlownAway_Ver3>()?.EndHitStop();

        // カメラの特殊視点解除
        cameraFunction?.StopSpecialView();
    }
}