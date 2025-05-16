//======================================================
// [GameClearSequence]
// �쐬�ҁF�r��C
// �ŏI�X�V���F05/16
// 
// [Log]
// 05/08�@�r��@���̃N���A���o���쐬
// 05/10�@�r��@OnGameClear�֐��ɖ߂�l��ǉ�
// 05/11�@�r��@�J�������X�i�b�N��ǐՂ��鏈����ǉ�
// 05/12�@�r��@��A�̗����������
// 05/16�@�r��@�X�R�A�\�����ւ̑Ή�����������
//======================================================
using UnityEngine;
using UnityEngine.UI;

// �N���A���o�𐧌䂷��X�N���v�g
public class GameClearSequence : MonoBehaviour
{
    [Header("�Q��")]
    [SerializeField] private ClearConditions ClearConditions;   // �V�[���J�ڂ��Ǘ�����X�N���v�g
    //[SerializeField] private FlyingPoint FlyingPoint;           // �X�R�A���Ǘ�����X�N���v�g
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
    //[SerializeField] private int[] StarScoreThresholdArray; // �X�R�A��臒l

    [Header("�X�i�b�N�̑��x")]
    [SerializeField] private int SnackSpeed = 700;

    private GameObject ClearBackImage;

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

    // �N���A�����𖞂��������ɌĂяo���֐�
    // ����ɏI�������ꍇ��true���A�����łȂ��ꍇ��false��Ԃ�
    public bool OnGameClear()
    {
        if (ClearUI == null || PlayerObject == null|| SnackObject == null || CameraObject == null|| ClearConditions == null)
        {
            Debug.LogError("GameClearSequence >> �C���X�y�N�^�[�ł̐ݒ肪�s�\���ł�");
            return false;
        }

        MovePlayer MovePlayer = PlayerObject.GetComponent<MovePlayer>();

        BlownAway_Ver2 BlownAway = SnackObject.GetComponent<BlownAway_Ver2>();
        ObjectGravity SnackGravity = SnackObject.GetComponent<ObjectGravity>();

        CameraFunction CameraFunction = CameraObject.GetComponent<CameraFunction>();

        if (MovePlayer == null ||BlownAway == null || SnackGravity == null|| CameraFunction == null)
        {
            Debug.LogError("GameClearSequence >> �g�p����X�N���v�g���Q�Ɛ�ɃA�^�b�`����Ă��܂���");
            return false;
        }

        ClearBackImage = ClearUI.transform.GetChild(0).gameObject;
        ClearBackImage.SetActive(true);

        //Score = FlyingPoint.TotalScore;
        //Text ScoreText = ClearUI.transform.GetChild(2).GetComponent<Text>();
        //ScoreText.text = "�X�R�A�F" + Score.ToString();

        // �X�i�b�N�̃��X�|�[���𖳌���
        BlownAway.OnClear(SnackSpeed);

        // CameraFunction�𖳌���
        CameraFunction.enabled = false;

        // �Q�[�����̃J�����̋������擾
        Vector3 CameraDirection = SnackObject.transform.position - CameraObject.transform.position;
        CameraDistance = CameraDirection.magnitude;

        // ����z�u����
        Vector3 StarPos = SnackObject.transform.position;
        StarPos.y = StarHeight; // �X�i�b�N�̏�ɐ���z�u
        GameObject StarClone = Instantiate(StarObject, StarPos, Quaternion.identity);
        //Vector3 StarPos = SnackObject.transform.position;
        //for (int i = 0; i < StarScoreThresholdArray.Length; i++)
        //{
        //    // �X�R�A��臒l�̐���������z�u
        //    StarPos.y = StarHeight + (StarToStarDistance * i); // �X�i�b�N�̏�ɐ���z�u
        //    GameObject StarClone = Instantiate(StarObject, StarPos, Quaternion.identity);
        //}

        // �N���A���o���t���O�𗧂Ă�
        IsClearSequence = true;

        return true;
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
        if (IsUIVisible && Input.anyKeyDown)
        {
            ClearConditions.TriggerSceneTransition();
        }

        // �J�����ɃX�i�b�N��ǐՂ�����
        // ���W
        if (!IsCameraStop)
        {
            Vector3 TargetPos = SnackObject.transform.position;
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
        CameraObject.transform.LookAt(SnackObject.transform.position);
        //float FocusTime = AfterTimer * CameraFocusSpeed;
        //FocusTime = Mathf.Clamp01(FocusTime);
        //Vector3 TargetFocus = Vector3.Lerp(CameraObject.transform.position, SnackObject.transform.position, FocusTime);
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

            // �X�i�b�N�̏d�͂������Ŗ�����
            SnackObject.GetComponent<ObjectGravity>().IsActive = false;

            IsPlayerStop = true;
        }

        // �N���A���o����UI��\��
        Vector3 SnackPos = SnackObject.transform.position;

        //// UI��\������X�i�b�N��Y���W���v�Z
        //float PosY = 100f;
        //if (StarScoreThresholdArray.Length > 0)
        //{
        //    PosY += 300f + ((StarScoreThresholdArray.Length - 1) * StarToStarDistance);
        //}

        //if (!IsUIVisible && SnackPos.y > PosY)
        if (!IsUIVisible && SnackPos.y > 600f)
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

                // �Q�[�����~�܂��Ă��Ȃ������炱���Ŏ~�߂�
                if (Time.timeScale != 0f)
                {
                    Time.timeScale = 0f;
                }
            }
        }
    }
}
