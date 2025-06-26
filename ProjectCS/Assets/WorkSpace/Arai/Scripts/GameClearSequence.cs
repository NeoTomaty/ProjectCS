//======================================================
// [GameClearSequence]
// �쐬�ҁF�r��C
// �ŏI�X�V���F06/19
// 
// [Log]
// 05/08�@�r��@���̃N���A���o���쐬
// 05/10�@�r��@OnGameClear�֐��ɖ߂�l��ǉ�
// 05/11�@�r��@�J�������X�i�b�N��ǐՂ��鏈����ǉ�
// 05/12�@�r��@��A�̗����������
// 05/16�@�r��@�X�R�A�\�����ɑΉ�
// 05/17�@�r��@�X�i�b�N��������ԕ��������S�Ȑ^�ザ��Ȃ��̂��N���A���o����ŏC��
// 05/19�@�r��@�N���AUI�ȊO�̃L�����o�X���\���ɂ��鏈����ǉ�
// 05/29�@�����@�N���A���oSE����
// 05/30�@�r��@BlownAway_Ver3�ɑΉ�
// 06/13  �����@�X�i�b�N�I�u�W�F�N�g��ύX����֐���ǉ�
// 06/16�@�r��@�N���A���o�̓��e��啝�ɕύX
// 06/18�@�r��@�J�����̓���������ύX���A�Ӑ}���Ȃ��J�������[�N��΍�
// 06/19�@�r��@�|�[�Y���͖�������ǉ�
// 06/20�@�����@�X�R�A�ۑ��̏�����ǉ�
//======================================================
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// �N���A���o�𐧌䂷��X�N���v�g
public class GameClearSequence : MonoBehaviour
{
    [Header("�Q��")]
    [SerializeField] private ClearConditions ClearConditions;   // �V�[���J�ڂ��Ǘ�����X�N���v�g
    [SerializeField] private FlyingPoint FlyingPoint;           // �X�R�A���Ǘ�����X�N���v�g
    [SerializeField] private GameObject ClearUI;                // �N���A���o��UI
    [SerializeField] private GameObject PlayerObject;           // �v���C���[�I�u�W�F�N�g
    [SerializeField] private GameObject SnackObject;            // �X�i�b�N�I�u�W�F�N�g
    [SerializeField] private GameObject CameraObject;           // �J����
    [SerializeField] private PlayerInput PauseInput;

    [Header("�J�����̐ݒ�")]
    [SerializeField] private float CameraTiltAngle = 0f;    // �J�����̌X���p�x
    [SerializeField] private float Offset = 30f;            // �J�����̋����𒲐�����I�t�Z�b�g�l

    [Header("�N���AUI�������I�ɕ\�����鎞��")]
    [SerializeField] private float UIShowTime = 10f; // �N���AUI�������I�ɕ\�����鎞��


    [Header("�G�t�F�N�g�̐ݒ�")]
    [SerializeField] GameObject SnackEffect;
    [SerializeField] float EffectSize = 1.0f;

    [Header("�p�[�e�B�N���̃��b�V��")]
    [SerializeField] Mesh ParticleMesh;
    [SerializeField] Material ParticleMaterial;

    [Header("�p�[�e�B�N���̃p�����[�^")]
    [SerializeField] float Size = 1.0f;
    [SerializeField] float SpeedMIN = 0.5f;
    [SerializeField] float SpeedMAX = 1.5f;
    [SerializeField] float RotateSpeedMIN = 30.0f;
    [SerializeField] float RotateSpeedMAX = 200.0f;

    private GameObject ClearBackImage;

    private PlayerInput PlayerInput; // �v���C���[�̓��͂��Ǘ�����component

    // �X�R�A
    private float Score = 0f;

    // �N���A��^�C�}�[
    private float AfterTimer = 0f;
    private float UITimer = 0f;

    // �N���A���o���t���O
    private bool IsClearSequence = false;

    // �v���C���[��~�t���O
    private bool IsPlayerStop = false;

    // UI�\���t���O
    private bool IsUIVisible = false;

    // �w�i�\���t���O
    private bool IsBackVisible = false;

    // �J�����̐�����
    private Vector3 CameraStartPos = Vector3.zero;
    private Vector3 CameraTargetPos = Vector3.zero;
    private Vector3 SnackPos = Vector3.zero;
    private float FocusHeight = 0f;

    [Header("�T�E���h�ݒ�")]

    //�Q�[���N���A���ɍĐ�������ʉ�(AudioClip)���C���X�y�N�^�[����ݒ�ł���悤�ɂ���
    [SerializeField] private AudioClip ClearSE;

    //���ʉ����Đ����邽�߂�AudioSource�R���|�[�l���g
    private AudioSource audioSource;

    // �N���A�����𖞂��������ɌĂяo���֐�
    // ����ɏI�������ꍇ��true���A�����łȂ��ꍇ��false��Ԃ�
    public bool OnGameClear()
    {
        if (ClearConditions == null || FlyingPoint == null || ClearUI == null || PlayerObject == null || SnackObject == null || CameraObject == null)
        {
            Debug.LogError("GameClearSequence >> �C���X�y�N�^�[�ł̐ݒ肪�s�\���ł�");
            return false;
        }

        MovePlayer MovePlayer = PlayerObject.GetComponent<MovePlayer>();

        BlownAway_Ver3 BlownAway = SnackObject.GetComponent<BlownAway_Ver3>();
        ObjectGravity SnackGravity = SnackObject.GetComponent<ObjectGravity>();

        CameraFunction CameraFunction = CameraObject.GetComponent<CameraFunction>();


        if (MovePlayer == null ||BlownAway == null || SnackGravity == null|| CameraFunction == null)
        {
            Debug.LogError("GameClearSequence >> �g�p����X�N���v�g���Q�Ɛ�ɃA�^�b�`����Ă��܂���");
            return false;
        }

        // ���͂𖳌���
        PlayerInput.actions.Disable();
        if (PauseInput != null)
        {
            PauseInput.actions.Disable();
        }

        // �w�i��\��
        ClearBackImage = ClearUI.transform.GetChild(0).gameObject;
        ClearBackImage.SetActive(true);

        // �N���AUI�ȊO�̃L�����o�X���\���ɂ���
        Transform FinishCanvas = ClearUI.transform.parent;
        Transform ParentCanvas = FinishCanvas.parent;
        for (int i = 0; i < ParentCanvas.childCount; i++)
        {
            Transform Child = ParentCanvas.GetChild(i);
            if (Child != FinishCanvas)
            {
                Child.gameObject.SetActive(false);
            }
        }

        Score = FlyingPoint.TotalScore;
        Text ScoreText = ClearUI.transform.GetChild(2).GetComponent<Text>();
        ScoreText.text = "�X�R�A�F" + Score.ToString();
        Debug.Log("GameClearSequence >> �X�R�A�F" + Score);

        // �X�i�b�N�̃N���[�����쐬
        Vector3 SpawnPos = SnackObject.transform.position;

        // ���̃X�i�b�N���\���ɂ���
        SnackObject.GetComponent<MeshRenderer>().enabled = false;

        // �v���C���[�ƃX�i�b�N�̓����蔻��𖳌���
        Collider PlayerCollider = PlayerObject.GetComponent<Collider>();
        Collider SnackCollider = SnackObject.GetComponent<Collider>();
        Physics.IgnoreCollision(PlayerCollider, SnackCollider);

        // CameraFunction�𖳌���
        CameraFunction.enabled = false;

        // �J�����̈ړ��J�n���W�ƖڕW���W��ݒ�
        CameraStartPos = CameraObject.transform.position;
        SnackPos = SnackObject.transform.position;
        Vector3 CameraToSnack = SnackPos - CameraStartPos;
        Vector3 CameraDirection = CameraToSnack.normalized * Offset; // �J�����̋����𒲐�
        CameraTargetPos = SnackPos - CameraDirection;

        FocusHeight = SnackPos.y;

        if (SnackEffect != null || ParticleMesh != null || ParticleMaterial != null)
        {
            // �G�t�F�N�g����
            GameObject Effect = Instantiate(SnackEffect, SpawnPos, Quaternion.identity);

            // �G�t�F�N�g�T�C�Y�ݒ�
            Effect.transform.localScale = new Vector3(EffectSize, EffectSize, EffectSize);

            // �p�[�e�B�N�����b�V���ݒ�
            ParticleSystem PS = Effect.GetComponent<ParticleSystem>();
            var PSRenderer = PS.GetComponent<ParticleSystemRenderer>();
            PSRenderer.mesh = ParticleMesh;
            PSRenderer.material = ParticleMaterial;

            // �p�[�e�B�N���p�����[�^�ݒ�
            var PSMain = PS.main;
            // �T�C�Y
            PSMain.startSize = Size;

            // �ˏo���x
            float min = SpeedMIN;
            float max = SpeedMAX;
            PSMain.startSpeed = new ParticleSystem.MinMaxCurve(min, max);

            // ��]���x
            var Rotation = PS.rotationOverLifetime;
            min = RotateSpeedMIN * Mathf.Deg2Rad;
            max = RotateSpeedMAX * Mathf.Deg2Rad;
            Rotation.x = new ParticleSystem.MinMaxCurve(min, max);
            Rotation.y = new ParticleSystem.MinMaxCurve(min, max);
            Rotation.z = new ParticleSystem.MinMaxCurve(min, max);

            // �G�t�F�N�g�Đ�
            PS.Play();
        }

        //���ʉ�(SE)���ݒ肳��Ă��āAAudioSource�����݂���Ƃ��ɍĐ�����
        if (ClearSE != null && audioSource != null)
        {
            //��񂾂����ʉ����Đ�����(�d�˂čĐ��\)
            audioSource.PlayOneShot(ClearSE);
        }

        // �N���A���o���t���O�𗧂Ă�
        IsClearSequence = true;

        return true;
    }

    private void Awake()
    {
        PlayerInput = PlayerObject.GetComponent<PlayerInput>();

        //AudioSource�R���|�[�l���g������GameObject�ɒǉ����A���ʉ��Đ��p�ɕێ����Ă���
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    private void Start()
    {
        ClearUI.transform.GetChild(0).gameObject.SetActive(false);
        ClearUI.transform.GetChild(1).gameObject.SetActive(false);
        ClearUI.transform.GetChild(2).gameObject.SetActive(false);
    }

    void Update()
    {
        if(!IsClearSequence) return;

        // �^�C�}�[�i�s
        AfterTimer += Time.deltaTime;

        // �L�[�E�{�^�����͂ŃV�[���J��
        // ���o���I����Ă�����͎�t
        if (IsUIVisible)
        {
            if (Input.anyKeyDown)
            {
                // �X�R�A�ۑ�
                ScoreManager scoreManager = Object.FindFirstObjectByType<ScoreManager>();
                StageSelector stageSelector = Object.FindFirstObjectByType<StageSelector>();

                if (scoreManager && stageSelector)
                {
                    int selectStageNumber = stageSelector.GetStageNumber();
                    int stageScore = scoreManager.GetStageScore(selectStageNumber);

                    if (stageScore < Score)
                    {
                        scoreManager.SetStageScore(selectStageNumber, (int)Score);
                        Debug.Log("�X�R�A�X�V");
                    }
                }

                ClearConditions.TriggerSceneTransition();
            }
        }

         // ��莞�Ԍo�߂Ō�̏������X�L�b�v
        if (AfterTimer > UIShowTime) return;

       // �J�����ɃX�i�b�N��ǐՂ�����
        // ���W
        float OffsetTime = AfterTimer * 1f;
        OffsetTime = Mathf.Clamp01(OffsetTime);
        Vector3 CarrentCameraPos=Vector3.Lerp(CameraStartPos, CameraTargetPos, OffsetTime);
        CameraObject.transform.position = CarrentCameraPos;

        // ����
        Vector3 Target = SnackPos;
        FocusHeight += 1f * Time.deltaTime * EffectSize;
        Target.y = FocusHeight;
        CameraObject.transform.LookAt(Target);

        // �X�i�b�N�𐁂���΂�����v���C���[���~
        if (!IsPlayerStop && AfterTimer > 0.1f)
        {
            // �v���C���[�̈ړ����x��0�ɂ���
            PlayerObject.GetComponent<MovePlayer>().MoveSpeedMultiplier = 0f;

            IsPlayerStop = true;
        }

        // �N���A���o����UI��\��
        if (!IsBackVisible)
        {
            UITimer += Time.deltaTime * 0.5f;
            UITimer = Mathf.Clamp01(UITimer);

            Color C = Color.black;
            C.a = Mathf.Lerp(0f, 0.6f, UITimer);
            ClearBackImage.GetComponent<Image>().color = C;

            if (UITimer >= 1f) IsBackVisible = true;
        }
        else
        {
            // UI��\��
            ClearUI.transform.GetChild(1).gameObject.SetActive(true);
            ClearUI.transform.GetChild(2).gameObject.SetActive(true);

            // UI�\���t���O�𗧂Ă�
            IsUIVisible = true;
        }
    }

    public void SetSnackObject(GameObject snack)
    {
        SnackObject = snack;
    }
}
