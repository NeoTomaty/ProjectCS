//======================================================
// [PlayerAnimationController]
// 作成者：森脇
// 最終更新日：06/06
//
// [Log]
// 05/22　森脇 アニメーターの管理
// 06/06　森脇 カウントダウン時に特定アニメーション再生追加
// 06/13　森脇 カメラの制御フラグ追加
// 06/19　森脇  地面との衝突時に rotationModel に戻る機能を追加
// 06/20　森脇  予期しない場合に rotationModel に戻る機能を追加
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
    [SerializeField] private Transform specialViewTarget;

    [Header("衝突によるリセット")]
    [Tooltip("これに衝突するとrotationModelに戻るオブジェクト")]
    [SerializeField] private GameObject resetTriggerObject;

    private bool isInitialized = false; // 初期化が完了したかどうかのフラグ

    private void Start()
    {
        // 起動時のモデル表示を確定させる
        UpdateModelVisibility();
        // 初期化完了フラグを立てる
        isInitialized = true;
    }

    public void ReturnToRotationModel()
    {
        // すでに rotationModel が表示されている場合は何もしない
        if (!useNormalModel)
        {
            return;
        }

        // 状態をリセット
        useNormalModel = false;
        waitingForAnimFinish = false;

        // エフェクト再生とカメラ制御終了
        PlayTransformEffect();
        EndSpecialCameraView();

        // モデルの表示を更新
        UpdateModelVisibility();
    }

    private void Update()
    {
        UpdateModelVisibility();

        // アニメーション終了待ち中ならチェック
        if (useNormalModel && waitingForAnimFinish && modelAnimator != null)
        {
            AnimatorStateInfo stateInfo = modelAnimator.GetCurrentAnimatorStateInfo(0);

            // ループせずに再生されたアニメーションが終了したら
            if (stateInfo.normalizedTime >= 1.0f)
            {
                PlayTransformEffect();

                useNormalModel = false;
                waitingForAnimFinish = false;
                UpdateModelVisibility();
                EndSpecialCameraView(); // カメラを通常視点に戻す
            }
        }
    }

    private void UpdateModelVisibility()
    {
        if (rotationModel == null || model == null) return;

        if (model.activeSelf != useNormalModel) model.SetActive(useNormalModel);
        if (rotationModel.activeSelf == useNormalModel) rotationModel.SetActive(!useNormalModel);
    }

    public void PlayRandomAnimation()
    {
        SetUseNormalModelWithWait();
        int randomIndex = Random.Range(0, randomAnimationCount);
        modelAnimator.SetInteger(randomIndexParameterName, randomIndex);
    }

    public void SetUseNormalModelWithWait()
    {
        if (!useNormalModel) PlayTransformEffect();

        useNormalModel = true;
        waitingForAnimFinish = true;
        UpdateModelVisibility();
        StartSpecialCameraView(); // カメラ制御開始
    }

    public void SetUseNormalModel(bool value)
    {
        if (useNormalModel == value) return;

        PlayTransformEffect();
        useNormalModel = value;
        UpdateModelVisibility();

        if (useNormalModel)
        {
            waitingForAnimFinish = false;
            StartSpecialCameraView();
        }
        else
        {
            waitingForAnimFinish = false;
            EndSpecialCameraView();
        }
    }

    public void PlaySpecificAnimation(string triggerName)
    {
        if (modelAnimator == null) return;

        if (!useNormalModel) PlayTransformEffect();

        useNormalModel = true;
        // 特定トリガーのアニメーションは終了を待たないことが多いので waitingForAnimFinish は false のまま
        waitingForAnimFinish = false;
        UpdateModelVisibility();

        modelAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        modelAnimator.SetTrigger(triggerName);

        // StartSpecialCameraView(); // カメラ制御開始
    }

    public void PlayAnimationByIndex(int index)
    {
        if (!useNormalModel) PlayTransformEffect();

        useNormalModel = true;
        waitingForAnimFinish = true; // Index指定のアニメーションは終了を待つ想定
        UpdateModelVisibility();
        modelAnimator.SetInteger(randomIndexParameterName, index);

        StartSpecialCameraView(); // カメラ制御開始
    }

    private void PlayTransformEffect()
    {
        if (!isInitialized)
        {
            return;
        }

        if (transformEffectPrefab != null)
        {
            Vector3 spawnPos = effectSpawnPoint != null ? effectSpawnPoint.position : transform.position;
            Instantiate(transformEffectPrefab, spawnPos, Quaternion.identity);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // "Ground" タグと衝突し続けているか、または指定オブジェクトと衝突し続けているか判定
        bool isGround = collision.gameObject.CompareTag("Ground");
        bool isTriggerObject = (resetTriggerObject != null && collision.gameObject == resetTriggerObject);

        // modelが表示されている（useNormalModelがtrue）かつ、地面かリセット用オブジェクトに触れている場合
        if (useNormalModel && (isGround || isTriggerObject))
        {
            // rotationModelに戻す共通処理を呼び出す
            ReturnToRotationModel();
        }
    }

    // メラ特別視点開始用のメソッドをシンプル化
    private void StartSpecialCameraView()
    {
        if (cameraFunction != null && specialViewTarget != null)
        {
            // CameraFunctionに目標地点のTransformを渡す（状態チェックはしない）
            cameraFunction.StartSpecialView(specialViewTarget);
        }
        else if (specialViewTarget == null)
        {
            Debug.LogWarning("Special View Targetが設定されていません。", this);
        }
    }

    // カメラ特別視点終了用のメソッドをシンプル化
    private void EndSpecialCameraView()
    {
        if (cameraFunction != null)
        {
            // CameraFunction.StopSpecialView() を呼び出す（状態チェックはしない）
            cameraFunction.StopSpecialView();
        }
    }
}