//====================================================
// �X�N���v�g���FSnackHeightUIManager
// �쐬�ҁF����
// ���e�F�X�i�b�N�̒n�ʂ܂ł̋�����UI�ŊǗ�����X�N���v�g
// �ŏI�X�V���F05/14
// 
// [Log]
// 05/14 ���� �X�N���v�g�쐬
// 05/15 ���� �|�C���^�[�\���̂Ƃ��̍�������l�𒴂�����悤�ɉ��Ŏ���
//====================================================

// ******* ���̃X�N���v�g�̎g���� ******* //
// 1.SnackObject�ɃX�i�b�N��K���ݒ肷��
// 2.�v���n�u�����Ă���̂ŁACanvas�ɕt���Ă�������

using UnityEngine;
using UnityEngine.UI;

public class SnackHeightUIManager_Ver2 : MonoBehaviour
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
    private GameObject[] PointerObject;  // �|�C���^�[�I�u�W�F�N�g
    private GameObject[] IndicatorObject;
    [SerializeField] private GameObject BarObject;      // �o�[�I�u�W�F�N�g
    [SerializeField] private GameObject TextObject;     // �e�L�X�g�I�u�W�F�N�g
    [SerializeField] private GameObject SnackObject;    // �X�i�b�N�I�u�W�F�N�g
    private GameObject[] SnackObjects;
    [SerializeField] private float MaxHeight = 500.0f;  // �Q�[�W�ő厞�̍���
    [SerializeField] private float AddLimitUnlock = 0f; // �|�C���^�[�̏������̉��Z�l
    [SerializeField] private float ExtraYMargin = 30f;
    [SerializeField] private bool IsTextVisible = true; // �e�L�X�g��\�����邩�ǂ���
    [SerializeField] private GameObject OriginalSnackPointer;
    [SerializeField] private GameObject OriginalOffscreenIndicator;


   [Header("�\���֘A")]
    [SerializeField] private GaugeDisplayMethod DisplayMethod = GaugeDisplayMethod.AlwaysVisible;
    [SerializeField] private GaugeDisplayType DisplayType = GaugeDisplayType.Bar;

    private Vector3 GroundPoint = new Vector3(0f, 0f, 0f); // �n�ʂ̍��W
    private Vector3 SnackPoint = new Vector3(0f, 0f, 0f);  // �X�i�b�N�̍��W
    private float SnackOffsetY = 0f;                       // �X�i�b�N�̔��a��
    private Rigidbody SnackRb;                             // �X�i�b�N��Rigidbody
    private bool IsGround = false;                         // �X�i�b�N���n�ʂɐڂ��Ă��邩�ǂ���
    private FallPointCalculator[] FPCalculator;              // FallPointCalculator�R���|�[�l���g
    private float PointMinY = 0f;                          // �ŏ�Y�ʒu�i�|�C���g�\���̂Ƃ��Ɏg�p�j
    private float PointMaxY = 200f;                        // �ő�Y�ʒu�i�|�C���g�\���̂Ƃ��Ɏg�p�j
    private RectTransform MeterRect;                       // ���[�^�[��RectTransform
    private RectTransform[] PointerRect;                     // �|�C���^�[��RectTransform
    private RectTransform[] IndicatorRect;
    private Image BarImage;                                // �o�[�摜
    private GameObject PointerImage;                       // �|�C���^�[�摜
    private GameObject DisplayObject;                      // ���ۂɕ\��������o�[���|�C���^�[�̃I�u�W�F�N�g�����Ă���
    private Text DistanceToGroundText;                     // �n�ʂ܂ł̋����̃e�L�X�g
    private float DistanceToGround = 0f;                   // �n�ʂ܂ł̋���
    private bool IsObjectCurrentlyActive = false;          // ����UI�I�u�W�F�N�g���A�N�e�B�u���ǂ���
    private bool HasStartedRising = true;                  // ��x�ł��㏸���n�܂������ǂ���
    private AllSnackManager ASM;
    private SnackDuplicator SD;
    private int CurrentPointerCount = 0;
    private Vector3[] corners = new Vector3[4];

    void Start()
    {
        if (!OriginalSnackPointer) Debug.LogError("OriginalSnackPointer�ɁuSnackPointer�v�v���n�u���A�^�b�`����Ă��܂���");

        SD = Object.FindFirstObjectByType<SnackDuplicator>();
        if (SD)
        {
            PointerObject = new GameObject[SD.GetMaxSnackCount()];
            PointerRect = new RectTransform[SD.GetMaxSnackCount()];
            IndicatorObject = new GameObject[SD.GetMaxSnackCount()];
            IndicatorRect = new RectTransform[SD.GetMaxSnackCount()];
            SnackObjects = new GameObject[SD.GetMaxSnackCount()];
            FPCalculator = new FallPointCalculator[SD.GetMaxSnackCount()];
        }
        else
        {
            PointerObject = new GameObject[1];
            PointerRect = new RectTransform[1];
            IndicatorObject = new GameObject[1];
            IndicatorRect = new RectTransform[1];
            SnackObjects = new GameObject[1];
            FPCalculator = new FallPointCalculator[1];
        }

        SnackObjects[0] = SnackObject;
        FPCalculator[0] = SnackObject.GetComponent<FallPointCalculator>();
        CurrentPointerCount = 1;

        float OffsetX = 0f;

        for (int i = 0; i < IndicatorRect.Length; i++)
        {
            IndicatorObject[i] = Instantiate(OriginalOffscreenIndicator, new Vector3(0f, 0f, 0f), Quaternion.identity, gameObject.transform);
            IndicatorObject[i].SetActive(false);
            IndicatorRect[i] = IndicatorObject[i].GetComponent<RectTransform>();
            IndicatorRect[i].anchoredPosition3D = new Vector3(OffsetX, 630f, 0f);
            IndicatorRect[i].localRotation = Quaternion.identity;
            IndicatorRect[i].localScale = Vector3.one;
            OffsetX += 20.0f;
        }

        OffsetX = 0f;
        for (int i = 0; i < PointerRect.Length; i++)
        {
            PointerObject[i] = Instantiate(OriginalSnackPointer, new Vector3(0f, 0f, 0f), Quaternion.identity, gameObject.transform);
            PointerObject[i].SetActive(false);
            PointerRect[i] = PointerObject[i].GetComponent<RectTransform>();
            PointerRect[i].anchoredPosition3D = new Vector3(OffsetX, 0f, 0f);
            PointerRect[i].localRotation = Quaternion.identity;
            PointerRect[i].localScale = Vector3.one;
            OffsetX += 20.0f;
        }

        PointerObject[0].SetActive(true);
        IndicatorObject[0].SetActive(true);
        BarObject.SetActive(false);
        MeterObject.SetActive(false);
        TextObject.SetActive(false);

        SnackOffsetY = SnackObjects[0].GetComponent<Collider>().bounds.extents.y;
        SnackRb = SnackObjects[0].GetComponent<Rigidbody>();

        BarImage = BarObject.GetComponent<Image>();
        MeterRect = MeterObject.GetComponent<RectTransform>();

        DistanceToGroundText = TextObject.GetComponent<Text>();

        switch (DisplayType)
        {
            case GaugeDisplayType.Bar: // �o�[�\���̏ꍇ
                // �\���I�u�W�F�N�g�̐ݒ�
                DisplayObject = BarObject;

                break;
            case GaugeDisplayType.Pointer: // �|�C���^�[�\���̏ꍇ
                // �\���I�u�W�F�N�g�̐ݒ�
                DisplayObject = PointerObject[0];

                // ���[�^�[�̏㉺�̍������v�Z����
                PointMaxY = MeterRect.anchoredPosition.y + (MeterRect.rect.height / 2.0f) - (PointerRect[0].rect.height / 2.0f);
                PointMaxY += AddLimitUnlock;
                PointMinY = MeterRect.anchoredPosition.y - (MeterRect.rect.height / 2.0f) + (PointerRect[0].rect.height / 2.0f);

                break;
        }
        SetObjectActive(true);
        ASM = Object.FindFirstObjectByType<AllSnackManager>();

    }

    void Update()
    {
        //if (!SnackObject) return;

        if (IsTextVisible)
        {
            // �n�ʂ܂ł̋������v�Z
            if (ASM)
            {
                DistanceToGround = ASM.GetDistanceToGround();
                if (DistanceToGround < 0f) DistanceToGround = 0f;
            }


            // ���l���e�L�X�g�ɔ��f
            DistanceToGroundText.text = Mathf.FloorToInt(DistanceToGround).ToString() + "m";
        }

        switch (DisplayType)
        {
            case GaugeDisplayType.Bar: // �o�[�\���̏ꍇ
                //BarImage.fillAmount = Mathf.Clamp01(HeightRatio);

                break;
            case GaugeDisplayType.Pointer: // �|�C���^�[�\���̏ꍇ

                for (int i = 0; i < CurrentPointerCount; i++)
                {
                    // �n�ʂɒ����Ă��邩�ǂ������擾
                    IsGround = FPCalculator[i].GetIsGround();

                    // �����n�_�̍��W���擾
                    GroundPoint = FPCalculator[i].GetFallPoint();

                    // ���݂̍����������Ŏ擾
                    float HeightRatio = ((SnackObjects[i].transform.position.y - SnackOffsetY) - GroundPoint.y) / (MaxHeight - GroundPoint.y);

                    Vector3 TempRectPosition = PointerRect[i].anchoredPosition;

                    // Y���W�������ɉ����ĕω�������
                    TempRectPosition.y = Mathf.Lerp(PointMinY, PointMaxY, HeightRatio);
                    PointerRect[i].anchoredPosition = TempRectPosition;

                    bool isCompletelyOutOfScreen = true;

                    PointerRect[i].GetWorldCorners(corners);

                    foreach (Vector3 corner in corners)
                    {
                        Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(null, corner);
                        if (screenPoint.x >= 0 && screenPoint.x <= Screen.width &&
                            screenPoint.y >= 0 && screenPoint.y <= Screen.height + ExtraYMargin)
                        {
                            isCompletelyOutOfScreen = false; // �����ł���ʓ��ɓ����Ă���
                        }
                    }

                    if (isCompletelyOutOfScreen)
                    {
                        IndicatorObject[i].SetActive(true);
                    }
                    else
                    {
                        IndicatorObject[i].SetActive(false);
                    }
                }
                break;
        }

        bool ShouldBeVisible = true; // �f�t�H���g�̕\�����

        switch (DisplayMethod)
        {
            case GaugeDisplayMethod.AlwaysVisible:
                ShouldBeVisible = true;
                break;

            case GaugeDisplayMethod.RisingStarted:
                //// �㏸�J�n���o
                //if (!HasStartedRising && !IsGround && SnackRb.linearVelocity.y > 1f)
                //{
                //    HasStartedRising = true;
                //}

                //// �n�ʂɒ������烊�Z�b�g
                //if (IsGround)
                //{
                //    HasStartedRising = false;
                //}

                //// �t���O�������Ă���Ԃ͕\��
                //ShouldBeVisible = HasStartedRising;
                break;

            case GaugeDisplayMethod.FallingStarted:
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
        if (IsTextVisible)
        {
            TextObject.SetActive(isActive);
        }
    }

    public void SetSnackObject(GameObject snack)
    {
        if (CurrentPointerCount >= PointerObject.Length) return;

        SnackObjects[CurrentPointerCount] = snack;
        FPCalculator[CurrentPointerCount] = snack.GetComponent<FallPointCalculator>();
        PointerObject[CurrentPointerCount].SetActive(true);
        CurrentPointerCount++;
    }
}