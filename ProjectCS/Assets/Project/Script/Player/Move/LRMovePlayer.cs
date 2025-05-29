//====================================================
// �X�N���v�g���FLRMovePlayer
// �쐬�ҁF����
// ���e�F�v���C���[�̍��E�ړ�����
// 
// [Log]
// 03/27 ���� �X�N���v�g�쐬 
// 03/31 ���� ���E�ړ������ǉ�
// 04/01 �R���g���[���[����ǉ�
// 04/08 ���E�ړ��𑬓x�Ɉˑ�������
// 04/23 ���� ���͂Ɋւ���d�l�ύX(PlayerInput(InputActionAsset))
// 05/15 �|�� ���E�ړ��̋��������P
//====================================================
using UnityEngine;
using UnityEngine.InputSystem;

public class LRMovePlayer : MonoBehaviour
{
    [Header("�R�[�h���ŎQ�Ƃ������")]
    public PlayerSpeedManager PlayerSpeedManager; // ���x�Ǘ��N���X
    public MovePlayer MovePlayer; // �v���C���[�ړ��N���X

    [Header("�J�[�u�̂��₷��")]
    [SerializeField] private float TurnSmooth = 10.0f;  // �J�[�u�̂��₷��

    private PlayerInput PlayerInput; // �v���C���[�̓��͂��Ǘ�����component
    private InputAction TurnAction;  // ���E�ړ��p��InputAction

    // �v���C���[�̍��E�ړ������������ϐ�
    float moveX;

    private void Awake()
    {
        // �����ɃA�^�b�`����Ă���PlayerInput���擾
        PlayerInput = GetComponent<PlayerInput>();

        // �Ή�����InputAction���擾
        TurnAction = PlayerInput.actions["LRTurn"];
    }
    void Start()
    {
        if (PlayerSpeedManager == null)
        {
            Debug.LogWarning("MovePlayer�X�N���v�g���A�^�b�`����Ă��܂���B");
        }
    }

    void Update()
    {
        moveX = TurnAction.ReadValue<float>();
    }


    void FixedUpdate()
    {
        if (PlayerSpeedManager == null) return;

        // ���x���擾
        float speed = PlayerSpeedManager.GetPlayerSpeed;


        // ���͂����ȏ゠��ꍇ�̂ݏ���
        if (Mathf.Abs(moveX) > 0.1f)
        {
            // �J�����̉E�������擾�iY���𐅕��ɕۂj
            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();

            // ���͂ɉ������ړ��x�N�g�����쐬
            Vector3 moveDirection = cameraRight * moveX;

            // ��]���X���[�W���O�������ꍇ�i�C�Ӂj
            Vector3 currentDir = MovePlayer.GetMoveDirection;
            Vector3 smoothedDir = Vector3.Slerp(currentDir, moveDirection, TurnSmooth * Time.deltaTime);

            // �ŏI�I�Ȉړ��������Z�b�g
            MovePlayer.SetMoveDirection(smoothedDir.normalized);
        }

    }
}
