//====================================================
// �X�N���v�g���FSweetSizeUp
// �쐬�ҁF����
// ���e�F���َq�̃T�C�Y��傫������
// �ŏI�X�V���F04/13
// 
// [Log]
// 04/13 ���� �X�N���v�g�쐬
// 04/15 �|�� �����T�C�Y�̏C���i�T�C�Y�A�b�v��{���Ɂj
// 
//====================================================
using UnityEngine;
using UnityEngine.UIElements;

public class SweetSizeUp : MonoBehaviour
{
    [SerializeField]
    private float MaxSize = 50.0f;      // ���َq�̍ő�T�C�Y
    [SerializeField]
    private float MinSize = 10.0f;      // ���َq�̍ŏ��i�����j�T�C�Y
    [SerializeField]
    private float SizeUpAmount = 10.0f; // ��x�̃T�C�Y�A�b�v�{��
    [SerializeField]
    private float ColliderSizeMultiplier = 1.0f; // �����蔻��̃T�C�Y�{��



    void Start()
    {
        // �����T�C�Y�̌���
        Vector3 scale = transform.localScale;
        scale *= MinSize;

        BoxCollider Box = GetComponent<BoxCollider>(); // �{�b�N�X�R���C�_�[�擾
        if(!Box)
        {
            Debug.LogError("BoxCollider���A�^�b�`����Ă��܂���");
        }
        Box.size *= ColliderSizeMultiplier; // �����蔻��̃T�C�Y��ݒ�
    }
   
    // �I�u�W�F�N�g��傫������֐�
    public void ScaleUpSweet()
    {
        // ���݂̃X�P�[��
        Vector3 CurrentScale = transform.localScale;

        // �X�P�[���A�b�v��̃T�C�Y�i�e�����ƂɌv�Z�j
        Vector3 NewScale = CurrentScale * SizeUpAmount;

        // MaxSize �𒴂��Ȃ��悤�ɐ���
        NewScale.x = Mathf.Min(NewScale.x, MaxSize);
        NewScale.y = Mathf.Min(NewScale.y, MaxSize);
        NewScale.z = Mathf.Min(NewScale.z, MaxSize);

        // �ŏI�I�ȃX�P�[����ݒ�
        transform.localScale = NewScale;
    }

    // ���݂̑傫���̊������擾
    public float GetScaleRatio()
    {
        return Mathf.Clamp01((transform.localScale.x - MinSize) / (MaxSize - MinSize));
    }
}
