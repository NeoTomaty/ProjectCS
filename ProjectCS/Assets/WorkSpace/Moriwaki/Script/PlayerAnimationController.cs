//======================================================
// [PlayerAnimationController]
// 作成者：森脇
// 最終更新日：05/22
//
// [Log]
// 05/22　森脇 アニメーターの管理
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

    private bool waitingForAnimFinish = false;

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

    // 外部から model を表示（アニメーション再生）させるときに呼ぶ
    public void SetUseNormalModelWithWait()
    {
        useNormalModel = true;
        waitingForAnimFinish = true;
        UpdateModelVisibility();
    }

    // 外部から通常通り切り替えたい場合
    public void SetUseNormalModel(bool value)
    {
        useNormalModel = value;
        waitingForAnimFinish = false;
        UpdateModelVisibility();
    }
}