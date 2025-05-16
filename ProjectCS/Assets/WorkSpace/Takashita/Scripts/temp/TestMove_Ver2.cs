using UnityEngine;
using UnityEngine.InputSystem;

public class TestMove_Ver2 : MonoBehaviour
{
    public PlayerSpeedManager PlayerSpeedManager; // ���x�Ǘ��N���X
    public MovePlayer MovePlayer; // �v���C���[�ړ��N���X

    [SerializeField] private float TurnSpeed = 100.0f;  // �J�[�u�̉�]���x
    [SerializeField] private float TurnResponse = 1.0f; // �J�[�u�̂��₷��

    [SerializeField] private Transform CameraTrangform;
    [SerializeField] private TestCameraFunction CameraFunction;

    [SerializeField] private float RotationSpeed = 5.0f;

    private PlayerInput PlayerInput; // �v���C���[�̓��͂��Ǘ�����component
    private InputAction TurnAction;  // ���E�ړ��p��InputAction
    private void Awake()
    {
        // �����ɃA�^�b�`����Ă���PlayerInput���擾
        PlayerInput = GetComponent<PlayerInput>();

        // �Ή�����InputAction���擾
        TurnAction = PlayerInput.actions["LRTurn"];

        Vector3 vec = CameraTrangform.transform.forward;
        vec.y = 0f;

        transform.rotation = Quaternion.LookRotation(vec);
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
        if (PlayerSpeedManager == null) return;

        // ���x���擾
        float speed = PlayerSpeedManager.GetPlayerSpeed;

        // ���݂̑��x�ɉ����ċȂ���₷������
        // �J�[�u�� = ��]���x �~ ���x �~ deltaTime
        float rotationAmount = TurnSpeed * (speed * TurnResponse) * Time.deltaTime;

        // �v���C���[�̍��E�ړ������������ϐ�
        float moveX = TurnAction.ReadValue<float>();

        Vector3 vec = CameraTrangform.transform.forward;
        vec.y = 0f;

        if (!PlayerInput.actions.enabled || CameraFunction.GetIsCameraInterpolating()) return;

        Debug.Log("�����ʉ�");
        MovePlayer.SetMoveDirection(vec);
    }
}
