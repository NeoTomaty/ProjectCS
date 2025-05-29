//======================================================
// [PlayerAnimationController]
// �쐬�ҁF�X�e
// �ŏI�X�V���F05/22
//
// [Log]
// 05/22�@�X�e �A�j���[�^�[�̊Ǘ�
//======================================================

using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("���f���؂�ւ�")]
    [SerializeField] private GameObject rotationModel;

    [SerializeField] private GameObject model;

    [Header("true = model ��\�� / false = rotationModel ��\��")]
    [SerializeField] private bool useNormalModel = true;

    [Header("model �� Animator�i1���[�v����p�j")]
    [SerializeField] private Animator modelAnimator;

    private bool waitingForAnimFinish = false;

    private void Update()
    {
        UpdateModelVisibility();

        // �A�j���[�V�����I���҂����Ȃ�`�F�b�N
        if (useNormalModel && waitingForAnimFinish && modelAnimator != null)
        {
            AnimatorStateInfo stateInfo = modelAnimator.GetCurrentAnimatorStateInfo(0);

            // ���[�v�����ɍĐ����ꂽ�A�j���[�V�������I��������
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

    // �O������ model ��\���i�A�j���[�V�����Đ��j������Ƃ��ɌĂ�
    public void SetUseNormalModelWithWait()
    {
        useNormalModel = true;
        waitingForAnimFinish = true;
        UpdateModelVisibility();
    }

    // �O������ʏ�ʂ�؂�ւ������ꍇ
    public void SetUseNormalModel(bool value)
    {
        useNormalModel = value;
        waitingForAnimFinish = false;
        UpdateModelVisibility();
    }
}