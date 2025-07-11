//====================================================
// スクリプト名：AnimationFinishTrigger
// 作成者：森脇
// 内容：フィニッシュ時のアニメーション終了トリガー
// [Log]
// 06/13　森脇 カメラの制御フラグ追加
// 06/19　森脇  SE再生機能を追加
// 06/27  高下　ターゲットを変更する関数を追加
// 07/11　森脇  タグによる追加ターゲットへの処理機能を追加

//Snackをアセットからのプレハブで置いてないシーンはタグにSnackつけてください
//シリアライズされてる場合はPlayerプレハブ内のModelについてるAnimationFinishTriggerスクリプトの追加ターゲット項目にSnackと入れてください

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

    [Header("追加ターゲット")]
    [Tooltip("snackObject以外にヒットストップを解除するオブジェクトのタグ")]
    [SerializeField] private string additionalTargetTag = "Snack";

    public void OnKickImpact()
    {
        Debug.Log("キックが当たったタイミングで呼び出された");
        StartCoroutine(HitStopRoutine());
    }

    private IEnumerator HitStopRoutine()
    {
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (hitEffectPrefab != null && effectSpawnPoint != null)
        {
            Instantiate(hitEffectPrefab, effectSpawnPoint.position, Quaternion.identity);
        }

        if (playerAnimator != null)
        {
            playerAnimator.speed = 0f;
        }

        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(hitStopDuration);
        Time.timeScale = 1f;

        if (playerAnimator != null)
        {
            playerAnimator.speed = 1f;
        }

        // 1. インスペクターで設定した主要ターゲットのヒットストップを解除
        if (snackObject != null)
        {
            snackObject.GetComponent<BlownAway_Ver3>()?.EndHitStop();
        }

        // 2. 追加で、指定タグを持つ全てのオブジェクトのヒットストップを解除
        if (!string.IsNullOrEmpty(additionalTargetTag))
        {
            GameObject[] additionalTargets = GameObject.FindGameObjectsWithTag(additionalTargetTag);
            foreach (var target in additionalTargets)
            {
                // 主要ターゲットと重複して処理しないようにチェック
                if (target != snackObject)
                {
                    target.GetComponent<BlownAway_Ver3>()?.EndHitStop();
                }
            }
        }

        // カメラの特殊視点解除
        cameraFunction?.StopSpecialView();
    }

    public void SetTargetObject(GameObject target)
    {
        snackObject = target;
    }
}