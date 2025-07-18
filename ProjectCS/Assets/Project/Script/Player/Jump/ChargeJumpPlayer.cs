//====================================================
// �X�N���v�g���FChargeJumpPlayer
// �쐬�ҁF�{��
// ���e�F�v���C���[�̃W�����v����
// �ŏI�X�V���F06/23
//
// [Log]
// 04/26�@�{�с@�X�N���v�g�쐬
// 04/26  �{�с@�����N�̃X�N���v�g�R�s�[
// 04/26�@�{�с@�������W�����v�̒ǉ�
// 04/27�@�X�e�@�G�t�F�N�g����̒ǉ�
// 04/29�@�r��@�`���[�W��̃W�����v�����Ƀ��t�e�B���O�W�����v�ւ̕����ǉ�
// 05/03�@�r��@�W�����v���ɃX�s�[�h�����ɖ߂�������switch������Ɉړ�
// 05/03�@�X�e�@�W�����v�O���֓n��
// 06/05�@�r��@���ʉ��̍Đ�������ǉ��i�W�����v�����n�j
// 06/19�@���{�@�`���[�W�G�t�F�N�g�J�n������ǋL
// 06/23�@�r��@�J�E���g�_�E������������W�����v�����Ȃ��悤�ɕύX
//====================================================
using UnityEngine;
using UnityEngine.InputSystem;

public class ChargeJumpPlayer : MonoBehaviour
{
    [Header("�W�����v�ݒ�")]
    [SerializeField] private float baseJumpForce = 10.0f;

    [SerializeField] private float groundCheckRadius = 0.2f;

    [SerializeField] private float NormalJumpForce = 10.0f;

    [Header("�W�����v�{���ݒ�")]
    [SerializeField] private float ChargeBluePower = 1.0f; // �����Ă�Ԃ�0.8�{�I

    [SerializeField] private float ChargeYellowPower = 1.5f; // �����Ă�Ԃ�0.8�{�I
    [SerializeField] private float ChargeRedPower = 2.0f; // �����Ă�Ԃ�0.8�{�I

    [Header("�`���[�W�ݒ�")]
    [SerializeField] private float chargeYellowTime = 1.0f;

    [SerializeField] private float chargeRedTime = 2.0f;
    [SerializeField] private float overheatTime = 3.0f;

    [Header("�X�s�[�h�ݒ�")]
    [SerializeField] private float chargingSpeedMultiplier = 0.8f; // �����Ă�Ԃ�0.8�{�I

    [SerializeField] private ParticleSystem chargeEffectPrefab; // �v���n�u���w��I
    private ParticleSystem chargeEffectInstance; // �C���X�^���X��ۑ�����p

    [Header("�G�t�F�N�g")]
    [SerializeField] private PlayerEffectController playerEffectController;

    [Header("�J�E���g�_�E���Q��")]
    [SerializeField] private GameStartCountdown GameStartCountdownScript;

    private Rigidbody rb;
    private bool isGrounded;
    private string groundTag = "Ground";

    private PlayerInput playerInput;
    private InputAction jumpAction;

    private bool isCharging = false;
    private float chargeTimer = 0.0f;

    private PlayerSpeedManager speedManager;
    private float originalSpeed;
    private bool isOverheated = false; // �I�[�o�[�q�[�g������

    private PlayerStateManager PlayerStateManager;
    private LiftingJump LiftingJump;

    public float JumpPower = 0.0f;

    [SerializeField] private float chargeThresholdTime = 0.3f; // �ǉ��F0.3�b�ȏ�Ń`���[�W�J�n
    private float jumpButtonHoldTime = 0.0f;
    private bool isJumpButtonPressed = false;

    private bool isJumping = false;
    public bool IsJumping => isJumping;

    private bool requestStartCharging = false;//�󒆂ŃW�����v�{�^���̒��������n�߂��Ƃ��p

    private bool isAreaJump = false;

    public bool IsAreaJump
    {
        get => isAreaJump;
        set => isAreaJump = value;
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        speedManager = GetComponent<PlayerSpeedManager>();

        if (playerInput != null)
        {
            jumpAction = playerInput.actions["Jump"];
        }
        else
        {
            Debug.LogError("PlayerInput��������܂���I");
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpAction = playerInput.actions["Jump"];

        if (speedManager != null)
        {
            originalSpeed = speedManager.GetPlayerSpeed;
        }
        else
        {
            Debug.LogError("PlayerSpeedManager��������܂���I");
        }

        PlayerStateManager = GetComponent<PlayerStateManager>();
        LiftingJump = GetComponent<LiftingJump>();
    }

    void Update()
    {
        bool wasGrounded = isGrounded;
        isGrounded = CheckIfGrounded();

        // ���n���Ƀ`���[�W�J�n����
        if (!wasGrounded && isGrounded)
        {
            if (!isCharging && isJumpButtonPressed && jumpButtonHoldTime >= chargeThresholdTime)
            {
                StartCharging();
            }

            // ���n��
            var soundScript = GetComponent<PlayerLandingSound>();
            soundScript?.PlayLandingSound();
        }

        if (isJumpButtonPressed)
        {
            jumpButtonHoldTime += Time.deltaTime;

            if (!isCharging && isGrounded && jumpButtonHoldTime >= chargeThresholdTime)
            {
                StartCharging();
            }
        }

        if (isCharging)
        {
            chargeTimer += Time.deltaTime;
            if (!isOverheated && chargeTimer < overheatTime)
            {
                // �`���[�W�G�t�F�N�g�J�n
                playerEffectController.StartChargeJumpEffect();

                float targetSpeed = originalSpeed * chargingSpeedMultiplier;
                SetSpeedDirectly(targetSpeed);
            }
            else if (chargeTimer >= overheatTime)
            {
                isOverheated = true;
                chargeEffectInstance.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }

            UpdateChargeEffect();
        }

        if(!isGrounded && IsAreaJump && isJumpButtonPressed && PlayerStateManager.GetLiftingState() == PlayerStateManager.LiftingState.LiftingPart)
        {
            LiftingJump.SetJumpPower(1f);
            LiftingJump.StartLiftingJump();
            IsAreaJump = false;
        }
    }

    private bool CheckIfGrounded()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position - Vector3.up * 0.1f, groundCheckRadius);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag(groundTag))
            {
                isJumping = false;
                return true;
            }
        }
        return false;
    }

    private void Jump()
    {
        if (isOverheated)
        {
            // �I�[�o�[�q�[�g���Ă���W�����v�L�����Z��
            Debug.Log("�I�[�o�[�q�[�g���ăW�����v���s");
            chargeTimer = 0.0f;
            isCharging = false;
            isOverheated = false;

            return;
        }

        float jumpMultiplier = 1.0f;

        // �`���[�W���Ԃɂ���ăW�����v�{������
        if (chargeTimer >= chargeRedTime)
        {
            jumpMultiplier = ChargeRedPower;
        }
        else if (chargeTimer >= chargeYellowTime)
        {
            jumpMultiplier = ChargeYellowPower;
        }
        else
        {
            jumpMultiplier = ChargeBluePower;
        }

        switch (PlayerStateManager.GetLiftingState())
        {
            case PlayerStateManager.LiftingState.Normal:
                // �ʏ���
                JumpPower = baseJumpForce * jumpMultiplier;
                rb.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
                isJumping = true; // �W�����v���t���O�𗧂Ă�
                Debug.Log($"�W�����v�����I�{��: {jumpMultiplier}�{, �`���[�W����: {chargeTimer:F2}�b");
                if (speedManager != null)
                {
                    // �W�����v��X�s�[�h�߂��i�I�[�o�[�q�[�g���ĂȂ��ꍇ�j
                    SetSpeedDirectly(originalSpeed);
                }
                break;

            case PlayerStateManager.LiftingState.LiftingPart:
                // ���t�e�B���O���
                LiftingJump.SetJumpPower(jumpMultiplier);
                LiftingJump.StartLiftingJump();
                if (speedManager != null)
                {
                    // �W�����v��X�s�[�h�߂��i�I�[�o�[�q�[�g���ĂȂ��ꍇ�j
                    SetSpeedDirectly(originalSpeed);
                }
                break;
        }

        if (chargeEffectInstance != null)
        {
            chargeEffectInstance.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ResetChargeEffect();
        }

        // �ϐ����Z�b�g
        chargeTimer = 0.0f;
        isCharging = false;
        isOverheated = false;
    }

    private void OnEnable()
    {
        jumpAction.started += OnJumpStarted;
        jumpAction.canceled += OnJumpCanceled;
    }

    private void OnDisable()
    {
        if (playerInput != null && playerInput.actions != null)
        {
            jumpAction.started -= OnJumpStarted;
            jumpAction.canceled -= OnJumpCanceled;
        }
    }

    // 0.3�b��������Ă΂��`���[�W�J�n����
    private void StartCharging()
    {
        isCharging = true;
        chargeTimer = 0.0f;

        if (chargeEffectPrefab != null)
        {
            if (chargeEffectInstance == null)
            {
                chargeEffectInstance = Instantiate(chargeEffectPrefab, transform);
                chargeEffectInstance.transform.localPosition = Vector3.zero;
            }

            chargeEffectInstance.Play();
        }

        Debug.Log("�`���[�W�J�n");
    }

    // �������u�ԁi�`���[�W�J�n�j
    // �W�����v�{�^���������Ƃ�
    private void OnJumpStarted(InputAction.CallbackContext context)
    {
        // �J�E���g�_�E�����͖�����
        if (GameStartCountdownScript != null && GameStartCountdownScript.IsCountingDown) return;

        isJumpButtonPressed = true;
        jumpButtonHoldTime = 0.0f;
        isOverheated = false;
        originalSpeed = speedManager.GetPlayerSpeed;
    }

    // �W�����v�{�^���������Ƃ�
    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        // �J�E���g�_�E�����͖�����
        if (GameStartCountdownScript != null && GameStartCountdownScript.IsCountingDown) return;

        isJumpButtonPressed = false;

        if (isCharging && isGrounded)
        {
            Jump(); // �`���[�W�W�����v
        }
        else if (!isCharging && isGrounded && jumpButtonHoldTime < chargeThresholdTime)
        {
            // �ʏ�W�����v�i���z�[���h���Ԃ��Z���������j
            float jumpMultiplier = 1.0f;
            switch (PlayerStateManager.GetLiftingState())
            {
                case PlayerStateManager.LiftingState.Normal:
                    rb.AddForce(Vector3.up * NormalJumpForce, ForceMode.Impulse);
                    isJumping = true;
                    break;

                case PlayerStateManager.LiftingState.LiftingPart:
                    LiftingJump.SetJumpPower(jumpMultiplier);
                    LiftingJump.StartLiftingJump();
                    break;
            }
        }

        // SE
        var soundScript = GetComponent<PlayerJumpSound>();
        soundScript?.PlayJumpSound();

        ResetChargeState();
    }

    private void SetSpeedDirectly(float newSpeed)
    {
        float current = speedManager.GetPlayerSpeed;
        float delta = newSpeed - current;

        if (delta > 0)
        {
            speedManager.SetAccelerationValue(delta);
        }
        else
        {
            speedManager.SetDecelerationValue(-delta);
        }
    }

    private void UpdateChargeEffect()
    {
        if (chargeEffectInstance == null) return;

        float chargeRatio = Mathf.Clamp01(chargeTimer / chargeRedTime);

        var main = chargeEffectInstance.main;
        Color targetColor = Color.cyan;

        if (chargeTimer >= chargeRedTime)
        {
            targetColor = Color.red;
        }
        else if (chargeTimer >= chargeYellowTime)
        {
            targetColor = Color.yellow;
        }
        else
        {
            targetColor = Color.cyan;
        }

        // �F���ς������G�t�F�N�g�����X�^�[�g���鏈����ǉ�
        if (main.startColor.color != targetColor)
        {
            main.startColor = targetColor;
            chargeEffectInstance.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            chargeEffectInstance.Play();
        }

        // �T�C�Y�ύX
        float startScale = 2.0f;
        float endScale = 0.5f;
        float scale = Mathf.Lerp(startScale, endScale, chargeRatio);
        chargeEffectInstance.transform.localScale = Vector3.one * scale;
    }

    private void ResetChargeEffect()
    {
        if (chargeEffectInstance == null) return;

        var main = chargeEffectInstance.main;
        main.startColor = Color.cyan; // �ŏ��̓V�A���ɖ߂�

        chargeEffectInstance.transform.localScale = Vector3.one * 2.0f; // �T�C�Y���߂�
    }

    private void ResetChargeState()
    {
        chargeTimer = 0.0f;
        isCharging = false;
        isOverheated = false;
        jumpButtonHoldTime = 0.0f;
        requestStartCharging = false; // ���ǉ��I

        if (chargeEffectInstance != null)
        {
            chargeEffectInstance.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ResetChargeEffect();
        }
    }

}