using UnityEngine;
using UnityEngine.InputSystem;

public class TestCameraFunction : MonoBehaviour
{
    public Transform Player;         // �v���C���[
    public Transform Target;         // �Ώ�

    [SerializeField] private PlayerInput PlayerInput;      // �v���C���[�̓��͂��Ǘ�����component
    [SerializeField] private PlayerSpeedManager PlayerSpeedManager; // PlayerSpeedManager�R���|�[�l���g

    [Tooltip("�J�����ƃv���C���[�̋���")]
    [SerializeField] private float Distance = 5.0f;               // �J�����ƃv���C���[�̋���
    [Tooltip("�J�����̍����ő�l�i�������j")]
    [SerializeField] private float MaxCameraHeight = 5.0f;        // �J�����̍����i�ő�j
    [Tooltip("�J�����̍����ŏ��l�i�ᑬ���j")]
    [SerializeField] private float MinCameraHeight = 3.0f;        // �J�����̍����i�ŏ��j
    [SerializeField] private float DefaultRotationSpeed = 50.0f;         // ��]���x
    [SerializeField] private float VerticalMin = -80.0f;          // �c�����ړ��̍ŏ��l
    [SerializeField] private float VerticalMax = 80.0f;           // �c�����ړ��̍ő�l
    [SerializeField] private float AutoCorrectSpeed = 1.5f;       // �����␳���x

    [Header("�⊮�ݒ�")]
    [SerializeField] private float TransitionDuration = 0.5f;       // ���b�N�I���⊮����
    [SerializeField] private float ReturnDuration = 0.4f;           // �ʏ펋�_�֖߂�⊮����
    [SerializeField] private float TransitionEndThreshold = 0.01f;  // �⊮�I���Ƃ݂Ȃ�臒l

    [Header("���b�N�I�����̃J�����ݒ�")]
    [SerializeField] private float LockOnDistance = 3.0f;           // ���b�N�I�����̃J��������
    [SerializeField] private float LockOnHeight = 2.0f;             // ���b�N�I�����̍����I�t�Z�b�g

    [Header("�������b�N�I�����̃J�����ݒ�")]
    [Tooltip("���t�e�B���O�Ŕ�΂�����̃��b�N�I������")]
    [SerializeField] private float ForceLockOnTime = 2.0f;          // ���t�e�B���O�Ŕ�΂�����̃��b�N�I������

    private float yaw = 0f;
    private float pitch = 20f;

    private InputAction LookTargetAction; // �^�[�Q�b�g�p��InputAction
    private InputAction RotateAction;     // ��]�p��InputAction
    private Vector2 RotateInput;          // ���͂��ꂽ�l��ێ�����i��]�p�j

    // ���b�N�I����Ԃ̐���p
    private bool IsLookingTarget = false;
    private bool IsTransitioningToTarget = false;
    private float TransitionTime = 0f;
    private Vector3 TransitionStartPos;
    private Quaternion TransitionStartRot;

    // �ʏ펋�_�֖߂�����p
    private bool IsReturningToNormal = false;
    private float ReturnTime = 0f;
    private Vector3 ReturnStartPos;
    private Quaternion ReturnStartRot;

    // ���t�e�B���O���̋������b�N�I������p
    private bool IsForceLockOn = false;
    private float ForceLockOnTimeLeft = 0f;

    //�J�������x�����p
    private float RotationSpeed;
    private float RotationRatio = 1.0f;


    private bool IsCameraInterpolating = false; // �ʏ펞�̃J�����̕⊮�������������ǂ���
    private bool IsPlayerWallHit = false;
    private Vector3 NewDirection = new Vector3(0f, 0f, 0f);

    private void Start()
    {
        if (!PlayerInput)
        {
            Debug.LogError("�ǔ��Ώۃv���C���[��PlayerInput���A�^�b�`����Ă��܂���");
        }
        if (!PlayerSpeedManager)
        {
            Debug.LogError("�v���C���[��PlayerSpeedManager���ݒ肳��Ă��܂���");
        }

        // �Ή�����InputAction���擾
        LookTargetAction = PlayerInput.actions["LookTargetSnack"];
        RotateAction = PlayerInput.actions["RotateCamera"];

        //�f�t�H���g�̃J�������x���󂯓n��
        RotationSpeed = DefaultRotationSpeed;
    }

    private void LateUpdate()
    {
        if (Player == null) return;

        // RT�{�^����������Ă��邩���m�F
        bool rtPressed = LookTargetAction.ReadValue<float>() > 0.5f;
        bool isManual = false;

        // ���x�ɉ������J�����̍������v�Z
        float cameraHeight = Mathf.Lerp(MinCameraHeight, MaxCameraHeight, PlayerSpeedManager.GetSpeedRatio());

        RotationSpeed = DefaultRotationSpeed * RotationRatio;

        // �J������]�i�蓮����F�E�X�e�B�b�N���j
        if (RotateInput.x < -0.5f) { yaw -= RotationSpeed * Time.deltaTime; isManual = true; }
        if (RotateInput.x > 0.5f) { yaw += RotationSpeed * Time.deltaTime; isManual = true; }
        if (RotateInput.y > 0.5f) { pitch += RotationSpeed * Time.deltaTime; isManual = true; }
        if (RotateInput.y < -0.5f) { pitch -= RotationSpeed * Time.deltaTime; isManual = true; }

        pitch = Mathf.Clamp(pitch, VerticalMin, VerticalMax); // �c�p�x�̐���

        // �����쎞�̓v���C���[�̌����Ɏ����␳
        if (!isManual && !Input.GetKey(KeyCode.R) && IsPlayerWallHit)
        {
           // float targetYaw = Player.eulerAngles.y;
            float targetYaw = Vector3.SignedAngle(Vector3.forward, NewDirection, Vector3.up);
            float targetPitch = 20f;
            yaw = Mathf.LerpAngle(yaw, targetYaw, AutoCorrectSpeed * Time.deltaTime);
            pitch = Mathf.Lerp(pitch, targetPitch, AutoCorrectSpeed * Time.deltaTime);

            // �⊮���I���������ǂ����̔���
            bool isYawCompleted = Mathf.Abs(yaw - targetYaw) < 0.01f;  // �ڕWYaw�ɏ\���߂����ǂ���
            bool isPitchCompleted = Mathf.Abs(pitch - targetPitch) < 0.01f;  // �ڕWPitch�ɏ\���߂����ǂ���

            IsCameraInterpolating = isYawCompleted && isPitchCompleted;

            if (IsCameraInterpolating) IsPlayerWallHit = false;

            Debug.Log("�⊮��");
        }

        // �������b�N�I�����̏���
        if (IsForceLockOn)
        {
            ForceLockOnTimeLeft -= Time.deltaTime;

            if (ForceLockOnTimeLeft < 0f)
            {
                IsForceLockOn = false;
                Debug.Log("�������b�N�I���I��");
            }
        }

        // === ���b�N�I�����̃J�������� ===
        if ((rtPressed || IsForceLockOn) && Target != null)
        {
            if (!IsLookingTarget)
            {
                StartLockOn(false);
            }

            Vector3 playerPos = Player.position;
            Vector3 targetPos = Target.position;

            // �v���C���[���^�[�Q�b�g�̕���
            Vector3 toTarget = (targetPos - playerPos).normalized;

            // �J�����̓v���C���[�̌���i�^�[�Q�b�g�Ɣ��΁j�ɁuLockOnDistance�v�����z�u
            Vector3 desiredPos = playerPos - toTarget * LockOnDistance;

            // �����␳
            desiredPos.y += LockOnHeight;

            // �J�����̓^�[�Q�b�g�𒍎�
            Quaternion desiredRot = Quaternion.LookRotation((targetPos + Vector3.up * cameraHeight) - desiredPos);

            if (IsTransitioningToTarget)
            {
                TransitionTime += Time.deltaTime;
                float t = Mathf.Clamp01(TransitionTime / TransitionDuration);
                transform.position = Vector3.Lerp(TransitionStartPos, desiredPos, t);
                transform.rotation = Quaternion.Slerp(TransitionStartRot, desiredRot, t);

                if (t >= 1.0f || Vector3.Distance(transform.position, desiredPos) < TransitionEndThreshold)
                {
                    IsTransitioningToTarget = false;
                    transform.position = desiredPos;
                    transform.rotation = desiredRot;
                }
            }
            else
            {
                transform.position = desiredPos;
                transform.rotation = desiredRot;
            }
        }
        else
        {
            // === RT�𗣂����ꍇ�F�ʏ펋�_�ɖ߂��⊮ ===
            if (IsLookingTarget)
            {
                StopLockOn();
            }

            if (IsReturningToNormal)
            {
                // �ʏ펋�_�֕⊮��
                ReturnTime += Time.deltaTime;
                float t = Mathf.Clamp01(ReturnTime / ReturnDuration);

                Vector3 offset = Quaternion.Euler(pitch, yaw, 0) * new Vector3(0, 0, -Distance);
                Vector3 normalPos = Player.position + offset;

                Quaternion normalRot;
                if (Input.GetMouseButton(1) && Target != null)
                {
                    normalRot = Quaternion.LookRotation((Target.position + Vector3.up * cameraHeight) - normalPos);
                }
                else
                {
                    normalRot = Quaternion.LookRotation((Player.position + Vector3.up * cameraHeight) - normalPos);
                }

                transform.position = Vector3.Lerp(ReturnStartPos, normalPos, t);
                transform.rotation = Quaternion.Slerp(ReturnStartRot, normalRot, t);

                if (t >= 1.0f)
                {
                    IsReturningToNormal = false; // �⊮�I��
                }
            }
            else
            {
                // === �ʏ�̃J�����Ǐ]���� ===
                Vector3 offset = Quaternion.Euler(pitch, yaw, 0) * new Vector3(0, 0, -Distance);
                transform.position = Player.position + offset;

                // �E�N���b�N�Ń^�[�Q�b�g�𒍎��A�����łȂ���΃v���C���[�𒍎�
                if (Input.GetMouseButton(1) && Target != null)
                {
                    transform.LookAt(Target.position + Vector3.up * cameraHeight);
                }
                else
                {
                    transform.LookAt(Player.position + Vector3.up * cameraHeight);
                }
            }
        }
    }

    private void OnEnable()
    {
        // �J������]�A�N�V����
        PlayerInput.actions["RotateCamera"].performed += OnRotate;

        // ���͏I����
        PlayerInput.actions["RotateCamera"].canceled += OnRotate;
    }

    private void OnDisable()
    {
        if (PlayerInput != null && PlayerInput.actions != null)
        {
            // �C�x���g�o�^����
            PlayerInput.actions["RotateCamera"].performed -= OnRotate;
            PlayerInput.actions["RotateCamera"].canceled -= OnRotate;
        }
    }

    // ���̓C�x���g�i�����ꂽ/�����ꂽ�j�ŌĂ΂��
    public void OnRotate(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // ���͂��s��ꂽ
            RotateInput = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            // ���͂��L�����Z�����ꂽ
            RotateInput = Vector2.zero;
        }
    }

    public void StartLockOn(bool isForceLockOn)
    {
        if (Target == null || Player == null) return;
        IsLookingTarget = true;
        IsTransitioningToTarget = true;
        TransitionTime = 0.0f;
        TransitionStartPos = transform.position;
        TransitionStartRot = transform.rotation;

        // �n���ꂽ�����ɂ���āA�C�Ӄ��b�N�I�����������b�N�I���𔻒f
        IsForceLockOn = isForceLockOn;
        if (IsForceLockOn)
        {
            ForceLockOnTimeLeft = ForceLockOnTime;
        }
    }

    public void StopLockOn()
    {
        if (!IsLookingTarget) return;

        IsLookingTarget = false;
        IsTransitioningToTarget = false;
        IsReturningToNormal = true;
        ReturnTime = 0.0f;
        ReturnStartPos = transform.position;
        ReturnStartRot = transform.rotation;
    }

    public void SetLockOnTarget(Transform target)
    {
        Target = target;
    }

    public void SetRatio(float ratio)
    {
        RotationRatio = ratio;
    }

    public bool GetIsCameraInterpolating()
    {
        return IsCameraInterpolating;
    }

    public void SetIsPlayerWallHit(bool isHit, Vector3 newDirection)
    {
        IsPlayerWallHit = isHit;
        NewDirection = newDirection;
    }
}
