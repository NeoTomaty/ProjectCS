//====================================================
// �X�N���v�g���FStageSelectManager_Ver2
// �쐬�ҁF����
// ���e�F�X�e�[�W�Z���N�g���ꊇ�Ǘ�����X�N���v�g
// �ŏI�X�V���F05/23
// 
// [Log]
// 05/23 ���� �X�N���v�g�쐬
//====================================================
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// �X�e�[�W�I����ʑS�̂̐���N���X�i���́E�J�����E���f���EUI�Ǘ��Ȃǁj
public class StageSelectManager_Ver2 : MonoBehaviour
{
    // �X�e�[�W/�J�[�u/�eUI�I�u�W�F�N�g�̎Q��
    [SerializeField] private GameObject StageObject;
    [SerializeField] private GameObject BezierCurveObject;
    [SerializeField] private GameObject StageImageUI;
    [SerializeField] private GameObject StartUI;
    [SerializeField] private GameObject ScoreUI;
    [SerializeField] private Image StageImage;
    [SerializeField] private Image StageName01;
    [SerializeField] private GameObject StageName02;

    // �X�e�[�W�ړ������E�J�����E���f������Ȃ�
    [SerializeField] private BezierMover BezierMoverComponent;
    [SerializeField] private StageSelectMoveCamera MoveCamera;
    [SerializeField] private StageSelectChangeModel ChangeModel;

    // UI�v�f�i�I���ʒu�̃��C���j
    [SerializeField] private GameObject PointLineUI;

    // �X�R�A�ǂݍ��݁E�X�R�A�\���N���X
    [SerializeField] private LoadStageScore StageScore;
    [SerializeField] private DrawScore DrawScore;

    // �X�e�[�W���
    private Transform[] StageChildArray;
    private Transform[] StageModelTransform;
    private CubicBezierCurve[] BezierCurveChildArray;
    private StageSelector StageSelectorComponent;

    // �X�e�[�W��/�摜/�V�[����
    [SerializeField] private string[] GameSceneNames = new string[6];
    [SerializeField] private string TitleSceneName;
    [SerializeField] private Sprite[] StageImageSprites = new Sprite[5];
    [SerializeField] private Sprite[] StageNameSprites = new Sprite[5];

    // �X�e�[�W�ړ��E�X�P�[�����O����
    private bool IsReverse = false;
    private bool IsInputEnabled = false;
    private float ScaleChangeTimer = 0f;
    [SerializeField] private float ScaleChangeDuration = 0.5f;
    private bool IsScaling = true;
    [SerializeField] private Vector3 SmallStageModelSize = new Vector3(0.5f, 0.5f, 0.5f);
    private UILineConnector LineConnector;

    // ���͊֘A
    private PlayerInput PlayerInput;
    private InputAction ConfirmAction;
    private InputAction CancelAction;
    private InputAction LeftMoveAction;
    private InputAction RightMoveAction;
    private InputAction OptionAction;

    // �t�F�[�h�Ǘ�
    private FadeManager fade;

    // �I�v�V�����Ǘ�
    [SerializeField] private GameObject OptionCanvasObject;
    private bool IsOptionsOpen = false;

    // �A�j���[�V�����Ǘ�
    [SerializeField] private Animator PlayerAnimation;

    // SE�Ǘ�
     [SerializeField] private StageSelectSEPlayer SEPlayer;

    private void Awake()
    {
        // PlayerInput�̎擾�ƃA�N�V�����̊��蓖��
        PlayerInput = GetComponent<PlayerInput>();
        ConfirmAction = PlayerInput.actions["Confirm"];
        CancelAction = PlayerInput.actions["Cancel"];
        LeftMoveAction = PlayerInput.actions["LeftMove"];
        RightMoveAction = PlayerInput.actions["RightMove"];
        OptionAction = PlayerInput.actions["Option"];
    }

    void Start()
    {
        // �K�{�I�u�W�F�N�g�m�F
        if (!StageObject) Debug.LogError("StageObject���ݒ肳��Ă��܂���");
        if (!BezierCurveObject) Debug.LogError("BezierCurveObject���ݒ肳��Ă��܂���");

        fade = Object.FindFirstObjectByType<FadeManager>();

        // �X�e�[�W�̎q�v�f�擾�i�X�e�[�W�{�́j
        int childCount = StageObject.transform.childCount;
        StageChildArray = new Transform[childCount];
        StageModelTransform = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            StageChildArray[i] = StageObject.transform.GetChild(i);
            StageModelTransform[i] = StageChildArray[i].transform.GetChild(0);
            StageModelTransform[i].localScale = SmallStageModelSize; // �ŏ��͏�����
        }

        // �x�W�F�Ȑ��擾
        childCount = BezierCurveObject.transform.childCount;
        BezierCurveChildArray = new CubicBezierCurve[childCount];
        for (int i = 0; i < childCount; i++)
        {
            BezierCurveChildArray[i] = BezierCurveObject.transform.GetChild(i).GetComponent<CubicBezierCurve>();
        }

        // �X�e�[�W�I����Ԃ̎擾
        StageSelectorComponent = Object.FindFirstObjectByType<StageSelector>();
        if (!StageSelectorComponent)
        {
            Debug.LogError("StageSelector��������܂���");
            return;
        }

        // �v���C���[�̏����ʒu������
        if (StageSelectorComponent.GetStageNumber() == BezierCurveChildArray.Length)
        {
            BezierMoverComponent.SetPosition(BezierCurveChildArray[StageSelectorComponent.GetStageNumber() - 1].StageObject2.position);
        }
        else
        {
            BezierMoverComponent.SetPosition(BezierCurveChildArray[StageSelectorComponent.GetStageNumber()].StageObject1.position);
        }

        // �����\���̉摜��UI�ݒ�
        StageImage.sprite = StageImageSprites[StageSelectorComponent.GetStageNumber()];
        StageName01.sprite = StageNameSprites[StageSelectorComponent.GetStageNumber()];
        StageImageUI.transform.localScale = Vector3.zero;
        StartUI.SetActive(false);
        StageName02.SetActive(false);

        LineConnector = PointLineUI.GetComponent<UILineConnector>();
        LineConnector.SetArrayNumber(StageSelectorComponent.GetStageNumber());
    }

    // �G�f�B�^��Ŕz��̗v�f�����Y�����Ƃ������C��
    private void OnValidate()
    {
        // �X�e�[�W�摜�z��̗v�f���`�F�b�N
        if (GameSceneNames.Length != StageImageSprites.Length)
        {
            Sprite[] newArray = new Sprite[GameSceneNames.Length];
            for (int i = 0; i < Mathf.Min(GameSceneNames.Length, StageImageSprites.Length); i++)
            {
                newArray[i] = StageImageSprites[i];
            }
            StageImageSprites = newArray;
        }

        // �X�e�[�W���z��̗v�f���`�F�b�N
        if (GameSceneNames.Length != StageNameSprites.Length)
        {
            Sprite[] newArray = new Sprite[GameSceneNames.Length];
            for (int i = 0; i < Mathf.Min(GameSceneNames.Length, StageNameSprites.Length); i++)
            {
                newArray[i] = StageNameSprites[i];
            }
            StageNameSprites = newArray;
        }
    }

    void Update()
    {
        if (IsOptionsOpen)
        {
            if (!OptionCanvasObject.activeSelf || OptionAction.WasPerformedThisFrame())
            {
                IsOptionsOpen = false;
                OptionCanvasObject.SetActive(false);
                if(PlayerAnimation) PlayerAnimation.speed = 1f;
            }

            return;
        }
        else
        {
            if (OptionAction.WasPerformedThisFrame() && !MoveCamera.GetIsSwitched() && !BezierMoverComponent.GetIsMoving())
            {
                IsOptionsOpen = true;
                OptionCanvasObject.SetActive(true);
                if (PlayerAnimation) PlayerAnimation.speed = 0f;
            }
        }

        // �x�W�F�Ȑ��ƃX�e�[�W���̐������`�F�b�N
        if (StageChildArray.Length - 1 != BezierCurveChildArray.Length) return;

        // ���ړ�����
        if (LeftMoveAction.WasPerformedThisFrame() && !MoveCamera.GetIsSwitched())
        {
            if (StageSelectorComponent.GetStageNumber() <= 0) return;
            if (BezierMoverComponent.GetIsMoving() && IsReverse) return;

            SEPlayer.PlaySE(StageSelectSEPlayer.StageSelectSE.Select);
            IsReverse = true;
            BezierMoverComponent.StartMove(true, BezierCurveChildArray[StageSelectorComponent.GetStageNumber() - 1]);
            StageSelectorComponent.SetStageNumber(-1);
        }

        // �E�ړ�����
        if (RightMoveAction.WasPerformedThisFrame() && !MoveCamera.GetIsSwitched())
        {
            if (StageSelectorComponent.GetStageNumber() == BezierCurveChildArray.Length) return;
            if (BezierMoverComponent.GetIsMoving() && !IsReverse) return;

            SEPlayer.PlaySE(StageSelectSEPlayer.StageSelectSE.Select);
            IsReverse = false;
            BezierMoverComponent.StartMove(false, BezierCurveChildArray[StageSelectorComponent.GetStageNumber()]);
            StageSelectorComponent.SetStageNumber(1);
        }

        // ����{�^�������i�J�����؂�ւ� or �V�[���J�ځj
        if (ConfirmAction.WasPerformedThisFrame() && !BezierMoverComponent.GetIsMoving())
        {
            if (GameSceneNames.Length == StageSelectorComponent.GetStageNumber()) return;

            SEPlayer.PlaySE(StageSelectSEPlayer.StageSelectSE.Confirm);

            if (!MoveCamera.GetIsSwitched())
            {
                MoveCamera.SetIsSwitched(true);
            }
            else
            {
                ChangeScene(GameSceneNames[StageSelectorComponent.GetStageNumber()]);
            }
        }

        // �L�����Z���{�^�������i�߂� or �^�C�g���ɖ߂�j
        if (CancelAction.WasPerformedThisFrame() && !BezierMoverComponent.GetIsMoving())
        {
            SEPlayer.PlaySE(StageSelectSEPlayer.StageSelectSE.Cancel);

            if (MoveCamera.GetIsSwitched())
            {
                MoveCamera.SetIsSwitched(false);
            }
            else
            {
                ChangeScene(TitleSceneName);
            }
        }

        // �X�e�[�W���f���̊g��E��]���o����
        if (!BezierMoverComponent.GetIsMoving())
        {
            ScaleChangeTimer += Time.deltaTime;
            float t = Mathf.Clamp01(ScaleChangeTimer / ScaleChangeDuration);
            StageImageUI.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            ChangeModel.SetChangeModel(true);

            for (int i = 0; i < StageModelTransform.Length; i++)
            {
                if (i == StageSelectorComponent.GetStageNumber())
                {
                    if (StageModelTransform[i].localScale != Vector3.one)
                    {
                        StageModelTransform[i].localScale = Vector3.Lerp(SmallStageModelSize, Vector3.one, t);
                    }
                    StageModelTransform[i].Rotate(0f, 10.0f * Time.deltaTime, 0f);
                }
                else
                {
                    StageModelTransform[i].rotation = Quaternion.identity;
                }
            }

            if (t >= 1.0f) ScaleChangeTimer = ScaleChangeDuration;
            LineConnector.SetArrayNumber(StageSelectorComponent.GetStageNumber());
        }
        else
        {
            // �ړ����̓��f�����k������
            ScaleChangeTimer -= Time.deltaTime;
            float t = Mathf.Clamp01(ScaleChangeTimer / ScaleChangeDuration);
            StageImageUI.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            ChangeModel.SetChangeModel(false);

            for (int i = 0; i < StageModelTransform.Length; i++)
            {
                if (StageModelTransform[i].localScale != SmallStageModelSize)
                {
                    StageModelTransform[i].localScale = Vector3.Lerp(SmallStageModelSize, Vector3.one, t);
                }
            }

            if (t <= 0.0f)
            {
                ScaleChangeTimer = 0f;
                StageImage.sprite = StageImageSprites[StageSelectorComponent.GetStageNumber()];
                StageName01.sprite = StageNameSprites[StageSelectorComponent.GetStageNumber()];
            }
        }

        // �J������Ԃɉ�����UI�̕\��/��\������
        if (MoveCamera.GetIsSwitched())
        {
            StageImageUI.SetActive(false);
            PointLineUI.SetActive(false);
        }
        else if (!MoveCamera.GetIsSwitched() && !MoveCamera.GetIsInterpolating())
        {
            StageImageUI.SetActive(true);
            PointLineUI.SetActive(true);
        }

        // �X�e�[�W������UI�\���ƃX�R�A���f
        if (MoveCamera.GetIsSwitched() && !MoveCamera.GetIsInterpolating())
        {
            StartUI.SetActive(true);
            StageName02.SetActive(true);
            ScoreUI.SetActive(true);
            DrawScore.SetScore(StageScore.GetStageScore(StageSelectorComponent.GetStageNumber()));
            StageName02.GetComponent<Image>().sprite = StageNameSprites[StageSelectorComponent.GetStageNumber()];
        }
        else if (!MoveCamera.GetIsSwitched())
        {
            StartUI.SetActive(false);
            StageName02.SetActive(false);
            ScoreUI.SetActive(false);
        }
    }

    // �w��V�[���֑J�ځi�t�F�[�h����j
    private void ChangeScene(string sceneName)
    {
        if (fade)
        {
            fade.FadeToScene(sceneName);
        }
    }
}
