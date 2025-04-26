//======================================================
// [TriangleGaugeController]
// �쐬�ҁF�r��C
// �ŏI�X�V���F04/26
// 
// [Log]
// 04/26�@�r��@�Q�[�W�������ő�������悤�Ɏ���
//======================================================
using UnityEngine;

// �Q�[�W���������J��Ԃ������̃X�N���v�g
// �ǂ̃I�u�W�F�N�g�ɃA�^�b�`���Ă�OK
public class TriangleGaugeController : MonoBehaviour
{
    // �Q�[�W��UI�Ƃ��Ẵp�����[�^
    [SerializeField] private RectTransform GaugeRectTransform;

    // �e�I�u�W�F�N�g
    [SerializeField] private RectTransform ParentRectTransform;

    // �Q�[�W��
    private float GaugeValue = 0.0f;

    // �Q�[�W�̑������x
    [SerializeField] private float GaugeSpeed = 0.5f;

    // �Q�[�W�̉���
    private const float GaugeWidth = 100.0f;

    // �Q�[�W�����̃��[�h
    private enum GaugeMode
    {
        Increase,
        Decrease,
    }

    private GaugeMode CurrentGaugeMode = GaugeMode.Increase;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // �Q�[�W�̑�������
        switch (CurrentGaugeMode)
        {
            case GaugeMode.Increase:                        // �������[�h
                GaugeValue += Time.deltaTime * GaugeSpeed;
                if (GaugeValue >= 1.0f)
                {
                    GaugeValue = 1.0f;
                    CurrentGaugeMode = GaugeMode.Decrease;
                }
                break;

            case GaugeMode.Decrease:                        // �������[�h
                GaugeValue -= Time.deltaTime * GaugeSpeed;
                if (GaugeValue <= 0.0f)
                {
                    GaugeValue = 0.0f;
                    CurrentGaugeMode = GaugeMode.Increase;
                }
                break;
        }

        // �Q�[�W�ʂɉ�����UI�̍��W��ύX
        // X���W�̂ݕω�
        GaugeRectTransform.localPosition = new Vector3((1 - GaugeValue) * GaugeWidth, GaugeRectTransform.localPosition.y, GaugeRectTransform.localPosition.z);

        // ���[�����ő�������悤�ɐe�̍��W�𒲐�
        // X���W�̂ݕω�
        ParentRectTransform.localPosition = new Vector3(GaugeValue * GaugeWidth, ParentRectTransform.localPosition.y, ParentRectTransform.localPosition.z);
    }
}
