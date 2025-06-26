//====================================================
// �X�N���v�g���FSnackHeightUIManager
// �쐬�ҁF����
// ���e�F�X�i�b�N�̒n�ʂ܂ł̋�����UI�ŊǗ�����X�N���v�g
// �ŏI�X�V���F06/25
// 
// [Log]
// 05/14 ���� �X�N���v�g�쐬
// 05/15 ���� �|�C���^�[�\���̂Ƃ��̍�������l�𒴂�����悤�ɉ��Ŏ���
// 06/25 �r�� �����̃X�i�b�N�ɑΉ�����悤�ɕύX
//====================================================

// ******* ���̃X�N���v�g�̎g���� ******* //
// 1.SnackObject�ɃX�i�b�N��K���ݒ肷��
// 2.�v���n�u�����Ă���̂ŁACanvas�ɕt���Ă�������

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SnackHeightUIManager : MonoBehaviour
{
    // �Q�[�W�̕\�����@��\��
    public enum GaugeDisplayMethod
    {
        [InspectorName("��ɕ\���iAlways Visible�j")]
        AlwaysVisible,     // ��ɕ\��
        [InspectorName("�㏸���n�܂����Ƃ��iRising Started�j")]
        RisingStarted,     // �㏸���n�܂����Ƃ�
        [InspectorName("�������n�܂����Ƃ��iFalling Started�j")]
        FallingStarted     // �������n�܂����Ƃ�
    }

    // �Q�[�W�^�C�v
    public enum GaugeDisplayType
    {
        [InspectorName("�o�[�\���iBar�j")]
        Bar,       // �o�[�\��
        [InspectorName("�|�C���g�\���iPointer�j")]
        Pointer,   // �|�C���g�\��
    }

    [SerializeField] private GameObject MeterObject;    // ���[�^�[�I�u�W�F�N�g
    [SerializeField] private GameObject PointerObject;  // �|�C���^�[�I�u�W�F�N�g
    [SerializeField] private GameObject BarObject;      // �o�[�I�u�W�F�N�g
    [SerializeField] private GameObject TextObject;     // �e�L�X�g�I�u�W�F�N�g
    [SerializeField] private GameObject SnackObject;    // �X�i�b�N�I�u�W�F�N�g
    [SerializeField] private float MaxHeight = 500.0f;  // �Q�[�W�ő厞�̍���
    [SerializeField] private float AddLimitUnlock = 0f; // �|�C���^�[�̏������̉��Z�l
    [SerializeField] private bool IsTextVisible = true; // �e�L�X�g��\�����邩�ǂ���
   
    [Header("�\���֘A")]
    [SerializeField] private GaugeDisplayMethod DisplayMethod = GaugeDisplayMethod.AlwaysVisible;
    [SerializeField] private GaugeDisplayType DisplayType = GaugeDisplayType.Bar;

    private Vector3 GroundPoint = new Vector3(0f, 0f, 0f); // �n�ʂ̍��W
    private Vector3 SnackPoint = new Vector3(0f, 0f, 0f);  // �X�i�b�N�̍��W
    private float SnackOffsetY = 0f;                       // �X�i�b�N�̔��a��
    private Rigidbody SnackRb;                             // �X�i�b�N��Rigidbody
    private bool IsGround = false;                         // �X�i�b�N���n�ʂɐڂ��Ă��邩�ǂ���
    private FallPointCalculator FPCalculator;              // FallPointCalculator�R���|�[�l���g
    private float PointMinY = 0f;                          // �ŏ�Y�ʒu�i�|�C���g�\���̂Ƃ��Ɏg�p�j
    private float PointMaxY = 200f;                        // �ő�Y�ʒu�i�|�C���g�\���̂Ƃ��Ɏg�p�j
    private RectTransform MeterRect;                       // ���[�^�[��RectTransform
    private RectTransform PointerRect;                     // �|�C���^�[��RectTransform
    private Image BarImage;                                // �o�[�摜
    private GameObject PointerImage;                       // �|�C���^�[�摜
    private GameObject DisplayObject;                      // ���ۂɕ\��������o�[���|�C���^�[�̃I�u�W�F�N�g�����Ă���
    private Text DistanceToGroundText;                     // �n�ʂ܂ł̋����̃e�L�X�g
    private float DistanceToGround = 0f;                   // �n�ʂ܂ł̋���
    private bool IsObjectCurrentlyActive = false;          // ����UI�I�u�W�F�N�g���A�N�e�B�u���ǂ���
    private bool HasStartedRising = true;                  // ��x�ł��㏸���n�܂������ǂ���

    private List<GameObject> SnackObjects = new List<GameObject>(); // �X�i�b�N�I�u�W�F�N�g�̃��X�g


    void Start()
    {
        BarObject.SetActive(false);
        PointerObject.SetActive(false);
        MeterObject.SetActive(false);
        TextObject.SetActive(false);

        if (!SnackObject)
        {
            Debug.LogError("SnackObject���ݒ肳��Ă��܂���");
            return;
        }
        
        SnackOffsetY = SnackObject.GetComponent<Collider>().bounds.extents.y;
        SnackRb = SnackObject.GetComponent<Rigidbody>();
        FPCalculator = SnackObject.GetComponent<FallPointCalculator>();
        BarImage = BarObject.GetComponent<Image>();
        MeterRect = MeterObject.GetComponent<RectTransform>();
        PointerRect = PointerObject.GetComponent<RectTransform>();
        DistanceToGroundText = TextObject.GetComponent<Text>();

        if (SnackObject != null)
        {
            SnackObjects.Add(SnackObject);
        }

        switch (DisplayType)
        {
            case GaugeDisplayType.Bar: // �o�[�\���̏ꍇ
                // �\���I�u�W�F�N�g�̐ݒ�
                DisplayObject = BarObject;

                break;
            case GaugeDisplayType.Pointer: // �|�C���^�[�\���̏ꍇ
                // �\���I�u�W�F�N�g�̐ݒ�
                DisplayObject = PointerObject;

                // ���[�^�[�̏㉺�̍������v�Z����
                PointMaxY = MeterRect.anchoredPosition.y + (MeterRect.rect.height / 2.0f) - (PointerRect.rect.height / 2.0f);
                PointMaxY += AddLimitUnlock;
                PointMinY = MeterRect.anchoredPosition.y - (MeterRect.rect.height / 2.0f) + (PointerRect.rect.height / 2.0f);

                break;
        }
        SetObjectActive(true);
    }

    void Update()
    {
        if (SnackObjects.Count == 0) return;
        //if (!SnackObject) return;

        float SnackHeight = 100000f;
        bool IsSnackGround = true;
        float SnackVelocityY = 0f;
        foreach (var Snack in SnackObjects)
        {
            // �n�ʂɒ����Ă��Ȃ�
            if (!Snack.GetComponent<FallPointCalculator>().GetIsGround())
            {
                IsSnackGround = false; // �n�ʂɒ����ĂȂ��X�i�b�N������
                // ���Ⴂ�ʒu�̃X�i�b�N
                if (Snack.transform.position.y < SnackHeight)
                {
                    SnackHeight = Snack.transform.position.y;
                    SnackVelocityY = Snack.GetComponent<Rigidbody>().linearVelocity.y;
                }
            }
        }

        if (IsTextVisible)
        {
            // �n�ʂ܂ł̋������v�Z
            DistanceToGround = Mathf.Max(0f, (SnackHeight - SnackOffsetY) - GroundPoint.y);
            //DistanceToGround = Mathf.Max(0f, (SnackObject.transform.position.y - SnackOffsetY) - GroundPoint.y);

            // ���l���e�L�X�g�ɔ��f
            DistanceToGroundText.text = Mathf.FloorToInt(DistanceToGround).ToString() + "m";
        }

        // �n�ʂɒ����Ă��邩�ǂ������擾
        IsGround = FPCalculator.GetIsGround();

        // �����n�_�̍��W���擾
        GroundPoint = FPCalculator.GetFallPoint();

        // ���݂̍����������Ŏ擾
        float HeightRatio = ((SnackHeight - SnackOffsetY) - GroundPoint.y) / (MaxHeight - GroundPoint.y);
        //float HeightRatio = ((SnackObject.transform.position.y - SnackOffsetY) - GroundPoint.y) / (MaxHeight - GroundPoint.y);

        switch (DisplayType)
        {
            case GaugeDisplayType.Bar: // �o�[�\���̏ꍇ
                BarImage.fillAmount = Mathf.Clamp01(HeightRatio);

                break;
            case GaugeDisplayType.Pointer: // �|�C���^�[�\���̏ꍇ

                Vector3 TempRectPosition = PointerRect.anchoredPosition;

                // Y���W�������ɉ����ĕω�������
                TempRectPosition.y = Mathf.Lerp(PointMinY, PointMaxY, HeightRatio);
                PointerRect.anchoredPosition = TempRectPosition;

                break;
        }

        bool ShouldBeVisible = true; // �f�t�H���g�̕\�����

        switch (DisplayMethod)
        {
            case GaugeDisplayMethod.AlwaysVisible:
                ShouldBeVisible = true;
                break;

            case GaugeDisplayMethod.RisingStarted:
                // �㏸�J�n���o
                if (!HasStartedRising && !IsSnackGround && SnackVelocityY > 1f)
                //if (!HasStartedRising && !IsGround && SnackRb.linearVelocity.y > 1f)
                {
                        HasStartedRising = true;
                }

                // �n�ʂɒ������烊�Z�b�g
                if (IsSnackGround)
                {
                    HasStartedRising = false;
                }

                // �t���O�������Ă���Ԃ͕\��
                ShouldBeVisible = HasStartedRising;
                break;

            case GaugeDisplayMethod.FallingStarted:
                ShouldBeVisible = !IsSnackGround && SnackVelocityY < 1f;
                //ShouldBeVisible = !IsGround && SnackRb.linearVelocity.y < 1f;

                break;
        }

        // �\����Ԃ��ς�����Ƃ�����SetObjectActive���Ă�
        if (IsObjectCurrentlyActive != ShouldBeVisible)
        {
            SetObjectActive(ShouldBeVisible);
            IsObjectCurrentlyActive = ShouldBeVisible;
        }

    }

    // �I�u�W�F�N�g�̕\����Ԃ�؂�ւ���
    private void SetObjectActive(bool isActive)
    {
        MeterObject.SetActive(isActive);
        DisplayObject.SetActive(isActive);
        if(IsTextVisible)
        {
            TextObject.SetActive(isActive);
        }
    }

    public void AddSnack(GameObject snackClone)
    {
        if (snackClone == null)
        {
            return;
        }

        // �v�f���`�F�b�N
        if (SnackObjects.Contains(snackClone)) return;

        // �X�i�b�N�̐V���ȃN���[�������X�g�ɒǉ�
        SnackObjects.Add(snackClone);
    }
}