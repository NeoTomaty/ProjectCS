//======================================================
// [PlayerAnimationController]
// 作成者：森脇
// 最終更新日：06/06
//
// [Log]
// 05/22　森脇 アニメーターの管理
// 06/06　森脇 カウントダウン時に特定アニメーション再生追加
// 06/13　森脇 カメラの制御フラグ追加
//======================================================

using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("モデル切り替え")]
    [SerializeField] private GameObject rotationModel;

    [SerializeField] private GameObject model;

    [Header("true = model を表示 / false = rotationModel を表示")]
    [SerializeField] private bool useNormalModel = true;

    [Header("model の Animator（1ループ判定用）")]
    [SerializeField] private Animator modelAnimator;

    [Header("ランダムアニメーションの設定")]
    [SerializeField] private int randomAnimationCount = 3; // アニメーションの数

    [SerializeField] private string randomIndexParameterName = "RandomIndex"; // パラメータ名

    private bool waitingForAnimFinish = false;

    [Header("変身エフェクト関連")]
    [SerializeField] private GameObject transformEffectPrefab;

    [SerializeField] private Transform effectSpawnPoint; // エフェクトの再生位置（任意）

    [Header("カメラ連携")]
    [Tooltip("カメラ制御スクリプト")]
    [SerializeField] private CameraFunction cameraFunction;

    [Tooltip("特別視点時のプレイヤーからの相対的なカメラ位置")]
    [SerializeField] private Vector3 specialViewPosition = new Vector3(0, 2, -4);

    private void Update()
    {
        UpdateModelVisibility();

        // アニメーション終了待ち中ならチェック
        if (useNormalModel && waitingForAnimFinish && modelAnimator != null)
        {
            AnimatorStateInfo stateInfo = modelAnimator.GetCurrentAnimatorStateInfo(0);

            // ループせずに再生されたアニメーションが終了したら
            if (stateInfo.normalizedTime >= 1.0f /*&& !stateInfo.loop*/)
            {
                useNormalModel = false;
                waitingForAnimFinish = false;
                UpdateModelVisibility();
            }
        }
    }

    private void UpdateModelVisibility()
    {
        if (rotationModel == null || model == null)
            return;

        model.SetActive(useNormalModel);
        rotationModel.SetActive(!useNormalModel);
    }

    public void PlayRandomAnimation()
    {
        SetUseNormalModelWithWait();
        int randomIndex = Random.Range(0, randomAnimationCount);
        modelAnimator.SetInteger(randomIndexParameterName, randomIndex);
    }

    // 外部から model を表示（アニメーション再生）させるときに呼ぶ
    public void SetUseNormalModelWithWait()
    {
        useNormalModel = true;
        waitingForAnimFinish = true;
        UpdateModelVisibility();
        if (cameraFunction != null)
        {
            cameraFunction.StartSpecialView(specialViewPosition);
        }
    }

    // 外部から通常通り切り替えたい場合
    public void SetUseNormalModel(bool value)
    {
        useNormalModel = value;
        waitingForAnimFinish = false;
        UpdateModelVisibility();
        PlayTransformEffect(); // エフェクトを再生
    }

    // 特定のTriggerアニメーションを再生したいときに使う
    public void PlaySpecificAnimation(string triggerName)
    {
        if (modelAnimator != null)
        {
            useNormalModel = true;
            waitingForAnimFinish = false;
            UpdateModelVisibility();

            // アニメーターの時間をUnscaledTimeに設定（Time.timeScaleの影響を受けない）
            modelAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;

            modelAnimator.SetTrigger(triggerName);
        }
    }

    // RandomIndex方式を使いたい場合の代替
    public void PlayAnimationByIndex(int index)
    {
        useNormalModel = true;
        waitingForAnimFinish = false;
        UpdateModelVisibility();
        modelAnimator.SetInteger(randomIndexParameterName, index);
    }

    private void PlayTransformEffect()
    {
        if (transformEffectPrefab != null)
        {
            Vector3 spawnPos = effectSpawnPoint != null ? effectSpawnPoint.position : transform.position;
            Instantiate(transformEffectPrefab, spawnPos, Quaternion.identity);
        }
    }
}