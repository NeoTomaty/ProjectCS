//======================================================
// [GameClearSequence]
// �쐬�ҁF�r��C
// �ŏI�X�V���F05/30
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
    [SerializeField] private GameObject StarObject;             // ��

    [Header("�J�����̐ݒ�")]
    [SerializeField] private float CameraTiltAngle = 0f;    // �J�����̌X���p�x
    [SerializeField] private float Offset = 30f;            // �J�����̋����𒲐�����I�t�Z�b�g�l
    private float CameraDistance = 0f;                      // �Q�[�����̃J�����̋���

    [Header("���̐ݒ�")]
    [SerializeField] private float StarHeight = 300f; // ���̍���
    [SerializeField] private float StarToStarDistance = 200f; // ���Ɛ��̋���
    [SerializeField] private int[] StarScoreThresholdArray; // �X�R�A��臒l

    [Header("�X�i�b�N�̑��x")]
    [SerializeField] private int SnackSpeed = 700;

    [Header("�N���AUI�������I�ɕ\�����鎞��")]
    [SerializeField] private float UIShowTime = 10f; // �N���AUI�������I�ɕ\�����鎞��

    private GameObject SnackClone; // �X�i�b�N�̃N���[��

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

    // �J�����ǐՃt���O
    private bool IsCameraStop = false;

    [Header("�T�E���h�ݒ�")]

    //�Q�[���N���A���ɍĐ�������ʉ�(AudioClip)���C���X�y�N�^�[����ݒ�ł���悤�ɂ���
    [SerializeField] private AudioClip ClearSE;

    //���ʉ����Đ����邽�߂�AudioSource�R���|�[�l���g
    private AudioSource audioSource;

    // �N���A�����𖞂��������ɌĂяo���֐�
    // ����ɏI�������ꍇ��true���A�����łȂ��ꍇ��false��Ԃ�
    public bool OnGameClear()
    {
        if (ClearConditions == null || FlyingPoint == null || ClearUI == null || PlayerObject == null || SnackObject == null || CameraObject == null || StarObject == null)
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

        PlayerInput.actions.Disable(); // ���͂𖳌��ɂ���

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

        // �X�i�b�N�̃N���[�����쐬
        Vector3 SpawnPos = SnackObject.transform.position;
        SnackClone = Instantiate(SnackObject, SpawnPos, Quaternion.identity);

        // ���̃X�i�b�N���ړ�������
        SnackObject.transform.localPosition = new Vector3(10000f, 10f, 10000f);

        // ���̃X�i�b�N���\���ɂ���
        SnackObject.GetComponent<MeshRenderer>().enabled = false;

        // �v���C���[�ƃX�i�b�N�̓����蔻��𖳌���
        Collider PlayerCollider = PlayerObject.GetComponent<Collider>();
        Collider SnackCollider = SnackClone.GetComponent<Collider>();
        Physics.IgnoreCollision(PlayerCollider, SnackCollider);

        SnackClone.GetComponent<BlownAway_Ver3>().OnClear();        // �X�i�b�N�̃��X�|�[���𖳌���
        SnackClone.GetComponent<ObjectGravity>().IsActive = false;  // �X�i�b�N�̏d�͂𖳌���
        SnackClone.GetComponent<Rigidbody>().AddForce(Vector3.up * SnackSpeed, ForceMode.Impulse);  // �X�i�b�N����ɐ�����΂�

        // CameraFunction�𖳌���
        CameraFunction.enabled = false;

        // �Q�[�����̃J�����̋������擾
        Vector3 CameraDirection = SnackClone.transform.position - CameraObject.transform.position;
        CameraDistance = CameraDirection.magnitude;

        // ����z�u����
        Vector3 StarPos = SnackClone.transform.position;
        for (int i = 0; i < StarScoreThresholdArray.Length; i++)
        {
            // �X�R�A��臒l�̐���������z�u
            StarPos.y = StarHeight + (StarToStarDistance * i); // �X�i�b�N�̏�ɐ���z�u
            GameObject StarClone = Instantiate(StarObject, StarPos, Quaternion.identity);
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

        // 10�b�o�߂ŃN���AUI�������I�ɕ\��
        if (AfterTimer > UIShowTime) IsUIVisible = true;

        // �L�[�E�{�^�����͂ŃV�[���J��
        // ���o���I����Ă�����͎�t
        if (IsUIVisible)
        {
            if (Input.anyKeyDown)
            {
                ClearConditions.TriggerSceneTransition();
            }

            // �N���AUI�\���ς݂Ȃ�ȍ~�̏������X�L�b�v
            return;
        }

        // �J�����ɃX�i�b�N��ǐՂ�����
        // ���W
        if (!IsCameraStop)
        {
            Vector3 TargetPos = SnackClone.transform.position;
            Vector3 CameraPos = CameraObject.transform.position;

            float OffsetTime = AfterTimer * 1f;
            OffsetTime = Mathf.Clamp01(OffsetTime);
            float CurrentOffset = Mathf.Lerp(CameraDistance, Offset, OffsetTime);

            // �J��������
            Vector3 CameraDirection = TargetPos - CameraPos;
            Vector3 DirectionOffset = CameraDirection.normalized * CurrentOffset; // �J�����̋����𒲐�
            CameraPos = TargetPos - DirectionOffset;

            // �J������Y���W���X�i�b�N��Y���W�ɍ��킹��
            float HeightMatchTime = AfterTimer * 5f;
            HeightMatchTime = Mathf.Clamp01(HeightMatchTime);
            float CurrentHeight = Mathf.Lerp(CameraPos.y, TargetPos.y, HeightMatchTime);
            CameraPos.y = CurrentHeight;
            CameraObject.transform.position = CameraPos; // �J�����̈ʒu�𒲐�
        }

        // ����
        CameraObject.transform.LookAt(SnackClone.transform.position);
        //float FocusTime = AfterTimer * CameraFocusSpeed;
        //FocusTime = Mathf.Clamp01(FocusTime);
        //Vector3 TargetFocus = Vector3.Lerp(CameraObject.transform.position, SnackClone.transform.position, FocusTime);
        //CameraObject.transform.LookAt(TargetFocus);

        //// �X��
        //float TiltTime = AfterTimer * 1f;
        //TiltTime = Mathf.Clamp01(TiltTime);
        //float CurrentTiltAngle = Mathf.LerpAngle(0f, CameraTiltAngle, TiltTime);
        //CameraObject.transform.Rotate(0f, 0f, CurrentTiltAngle);


        // �X�i�b�N�𐁂���΂�����v���C���[���~
        if (!IsPlayerStop && AfterTimer > 0.1f)
        {
            // �v���C���[�̈ړ����x��0�ɂ���
            PlayerObject.GetComponent<MovePlayer>().MoveSpeedMultiplier = 0f;

            IsPlayerStop = true;
        }

        // �N���A���o����UI��\��
        Vector3 SnackPos = SnackClone.transform.position;

        // UI��\������X�i�b�N��Y���W���v�Z
        float PosY = 100f;
        if (StarScoreThresholdArray.Length > 0)
        {
            PosY += 300f + ((StarScoreThresholdArray.Length - 1) * StarToStarDistance);
        }

        if (!IsUIVisible && SnackPos.y > PosY)
        {
            if (!IsBackVisible)
            {
                // �J�����̓������~�߂�
                IsCameraStop = true;

                UITimer += Time.deltaTime*0.5f;
                UITimer=Mathf.Clamp01(UITimer);

                Color C = Color.black;
                C.a = Mathf.Lerp(0f, 0.6f, UITimer);
                ClearBackImage.GetComponent<Image>().color = C;

                if(UITimer>=1f) IsBackVisible = true;
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
    }
}
