//======================================================
// [GaugeController]
// �쐬�ҁF�r��C
// �ŏI�X�V���F05/03
// 
// [Log]
// 04/26�@�r��@�Q�[�W�������ő�������悤�Ɏ���
// 04/27�@�r��@�L�[�E�{�^�����͂Ŏ~�߂���悤�ɕύX
// 04/27�@�r��@�Q�[�W�̒��S���I�u�W�F�N�g�̍��W�Ƃ���Ȃ��悤�ɏC��
// 04/27�@�r��@���X�N���v�g�ƍ��킹�ē��삷��悤�ɏC��
// 04/28�@�r��@�Q�[�W���~�߂���̑ҋ@���Ԃ�ǉ�
// 05/03�@�r��@�X���[���[�V�����̐�������
//======================================================
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

// �Q�[�W���������J��Ԃ������̃X�N���v�g
// GuageUI�ɃA�^�b�`
public class GaugeController : MonoBehaviour
{
    [SerializeField] private RectTransform GaugeRectTransform;  // �Q�[�W��UI�Ƃ��Ẵp�����[�^

    [SerializeField] private RectTransform ParentRectTransform; // �e�I�u�W�F�N�g

    private float GaugeValue = 0.0f;    // �Q�[�W��
    public float GetGaugeValue => GaugeValue;

    
    [SerializeField] private float GaugeSpeed = 0.5f;   // �Q�[�W�̑������x

    private const float GaugeWidth = 100.0f;    // �Q�[�W�̉���

    // �Q�[�W�̑������~�߂����
    [SerializeField] private KeyCode StopKey = KeyCode.Return;              // �L�[
    [SerializeField] private KeyCode StopBottun = KeyCode.JoystickButton1;  // �{�^��

    private bool IsStop = false;        // �Q�[�W�������~�߂�t���O
    private bool IsFinished = false;    // �Q�[�W�̓��삪�I���������ǂ���
    private bool IsFinishedLast = false; // ���O�̃t���[����IsFinished

    [SerializeField] private float StandTime = 0.5f;    // �Q�[�W���~�߂���̑ҋ@����
    private float TimeCount = 0f;

    // �Q�[�W�����̃��[�h
    private enum GaugeMode
    {
        Increase,
        Decrease,
    }

    private GaugeMode CurrentGaugeMode = GaugeMode.Increase;

    public void SetGaugeValue(float Value)
    {
        GaugeValue = Value; // �Q�[�W��������
    }

    // �Q�[�W�̓��삪�I�������u�Ԃ�Ԃ��֐�
    public bool IsFinishEnter()
    {
        return (IsFinished && !IsFinishedLast);
    }

    public void Play()
    {
        GaugeValue = 0.0f; // �Q�[�W��������
        CurrentGaugeMode = GaugeMode.Increase; // �������[�h�ɐݒ�

        // �Q�[�W��\��
        this.gameObject.SetActive(true);

        IsStop = false;
        IsFinished = false; // �Q�[�W�̓��삪�I�����Ă��Ȃ�
    }

    public void Stop()
    {
        // �Q�[�W���\��
        this.gameObject.SetActive(false);

        IsFinished = true; // �Q�[�W�̓��삪�I��
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Stop();
    }

    // Update is called once per frame
    void Update()
    {
        IsFinishedLast = IsFinished; // ���O�̃t���[����IsFinished��ۑ�

        if (IsStop)
        {
            TimeCount += Time.unscaledDeltaTime;

            if (TimeCount > StandTime)
            {
                Stop();
            }

            return;
        }

        if (Input.GetKeyDown(StopKey) || Input.GetKeyDown(StopBottun))
        {
            // �Q�[�W�̑������~�߂�
            IsStop = true;
        }

        // �Q�[�W�̑�������
        switch (CurrentGaugeMode)
        {
            case GaugeMode.Increase:                        // �������[�h
                GaugeValue += Time.unscaledDeltaTime * GaugeSpeed;
                if (GaugeValue >= 1.0f)
                {
                    GaugeValue = 1.0f;
                    CurrentGaugeMode = GaugeMode.Decrease;
                }
                break;

            case GaugeMode.Decrease:                        // �������[�h
                GaugeValue -= Time.unscaledDeltaTime * GaugeSpeed;
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
        ParentRectTransform.localPosition = new Vector3((GaugeValue * GaugeWidth) - GaugeWidth, ParentRectTransform.localPosition.y, ParentRectTransform.localPosition.z);
    }
}
