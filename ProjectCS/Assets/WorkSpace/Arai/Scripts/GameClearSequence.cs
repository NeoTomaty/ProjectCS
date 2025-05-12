//======================================================
// [GameClearSequence]
// �쐬�ҁF�r��C
// �ŏI�X�V���F05/12
// 
// [Log]
// 05/08�@�r��@���̃N���A���o���쐬
// 05/10�@�r��@OnGameClear�֐��ɖ߂�l��ǉ�
// 05/11�@�r��@�J�������X�i�b�N��ǐՂ��鏈����ǉ�
// 05/12�@�r��@��A�̗����������
//======================================================
using UnityEngine;

// �N���A���o�𐧌䂷��X�N���v�g
public class GameClearSequence : MonoBehaviour
{
    [Header("�Q��")]
    [SerializeField] private ClearConditions ClearConditions;   // �V�[���J�ڂ��Ǘ�����X�N���v�g
    //[SerializeField] private PlayerScore PlayerScore;           // �X�R�A���Ǘ�����X�N���v�g
    [SerializeField] private GameObject ClearUI;                // �N���A���o��UI
    [SerializeField] private GameObject PlayerObject;           // �v���C���[�I�u�W�F�N�g
    [SerializeField] private GameObject SnackObject;            // �X�i�b�N�I�u�W�F�N�g
    [SerializeField] private GameObject CameraObject;           // �J����
    [SerializeField] private GameObject StarObject;             // ��

    [Header("�J�����̐ݒ�")]
    [SerializeField] private float CameraTiltAngle = 0f; // �J�����̌X���p�x

    [Header("���̐ݒ�")]
    [SerializeField] private float StarHeight = 300f; // ���̍���
    [SerializeField] private float StarToStarDistance = 200f; // ���Ɛ��̋���
    //[SerializeField] private int[] StarScoreThresholdArray; // �X�R�A��臒l

    // �N���A��^�C�}�[
    private float AfterTimer = 0f;

    // �N���A���o���t���O
    private bool IsClearSequence = false;

    // �v���C���[��~�t���O
    private bool IsPlayerStop = false;

    // UI�\���t���O
    private bool IsUIVisible = false;

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

        PlayerSpeedManager PlayerSpeedManager = PlayerObject.GetComponent<PlayerSpeedManager>();

        BlownAway_Ver2 BlownAway = SnackObject.GetComponent<BlownAway_Ver2>();
        ObjectGravity SnackGravity = SnackObject.GetComponent<ObjectGravity>();

        CameraFunction CameraFunction = CameraObject.GetComponent<CameraFunction>();

        if (PlayerSpeedManager == null ||BlownAway == null || SnackGravity == null|| CameraFunction == null)
        {
            Debug.LogError("GameClearSequence >> �g�p����X�N���v�g���Q�Ɛ�ɃA�^�b�`����Ă��܂���");
            return false;
        }

        // �X�i�b�N�̃��X�|�[���𖳌���
        BlownAway.OnClear();

        // CameraFunction�𖳌���
        CameraFunction.enabled = false;

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
        ClearUI.SetActive(false);
    }

    void Update()
    {
        if(!IsClearSequence) return;

        // �^�C�}�[�i�s
        AfterTimer += Time.deltaTime;

        // �L�[�E�{�^�����͂ŃV�[���J��
        if (Input.anyKeyDown)
        {
            ClearConditions.TriggerSceneTransition();
        }

        // �J�����ɃX�i�b�N��ǐՂ�����
        // ���W
        if (!IsCameraStop)
        {
            float TargetPosY = SnackObject.transform.position.y;
            Vector3 CameraPos = CameraObject.transform.position;
            CameraPos.y = TargetPosY;   // �������X�i�b�N�ɍ��킹��
            CameraObject.transform.position = CameraPos;
        }

        // ����
        CameraObject.transform.LookAt(SnackObject.transform.position);
        //float FocusTime = AfterTimer * CameraFocusSpeed;
        //FocusTime = Mathf.Clamp01(FocusTime);
        //Vector3 TargetFocus = Vector3.Lerp(CameraObject.transform.position, SnackObject.transform.position, FocusTime);
        //CameraObject.transform.LookAt(TargetFocus);

        // �X��
        float TiltTime = AfterTimer * 1f;
        TiltTime = Mathf.Clamp01(TiltTime);
        float CurrentTiltAngle = Mathf.LerpAngle(0f, CameraTiltAngle, TiltTime);
        CameraObject.transform.Rotate(0f, 0f, CurrentTiltAngle);


        // �X�i�b�N�𐁂���΂�����v���C���[���~
        if (!IsPlayerStop && AfterTimer > 0.1f)
        {
            // �v���C���[�̈ړ����x��0�ɂ���
            PlayerObject.GetComponent<PlayerSpeedManager>().SetOverSpeed(0f);

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
            // UI��\��
            ClearUI.SetActive(true);

            // UI�\���t���O�𗧂Ă�
            IsUIVisible = true;

            // �J�����̓������~�߂�
            IsCameraStop = true;

            //// �Q�[�����~�܂��Ă��Ȃ������炱���Ŏ~�߂�
            //if (Time.timeScale != 0f)
            //{
            //    Time.timeScale = 0f;
            //}
        }
    }
}
