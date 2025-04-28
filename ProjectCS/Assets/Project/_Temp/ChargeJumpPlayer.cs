//====================================================
// �X�N���v�g���FChargeJumpPlayer
// �쐬�ҁF�{��
// ���e�F�v���C���[�̃W�����v����
// �ŏI�X�V���F04/27
//
// [Log]
// 04/26�@�{�с@�X�N���v�g�쐬
// 04/26  �{�с@�����N�̃X�N���v�g�R�s�[
// 04/26�@�{�с@�������W�����v�̒ǉ�
// 04/27�@�X�e�@�G�t�F�N�g����̒ǉ�
//====================================================
using UnityEngine;
using UnityEngine.InputSystem;

public class ChargeJumpPlayer : MonoBehaviour
{
    [Header("�W�����v�ݒ�")]
    [SerializeField] private float baseJumpForce = 10.0f;

    [SerializeField] private float groundCheckRadius = 0.2f;

    [Header("�`���[�W�ݒ�")]
    [SerializeField] private float chargeYellowTime = 1.0f;

    [SerializeField] private float chargeRedTime = 2.0f;
    [SerializeField] private float overheatTime = 3.0f;

    [Header("�X�s�[�h�ݒ�")]
    [SerializeField] private float chargingSpeedMultiplier = 0.8f; // �����Ă�Ԃ�0.8�{�I

    [SerializeField] private ParticleSystem chargeEffectPrefab; // �v���n�u���w��I
    private ParticleSystem chargeEffectInstance; // �C���X�^���X��ۑ�����p

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
    }

    private void Update()
    {
        isGrounded = CheckIfGrounded();

        if (isCharging)
        {
            chargeTimer += Time.deltaTime;

            if (!isOverheated && chargeTimer < overheatTime)
            {
                // �{�^�������Ă���Ԃ͏��0.8�{
                float targetSpeed = originalSpeed * chargingSpeedMultiplier;
                SetSpeedDirectly(targetSpeed);
            }
            else if (chargeTimer >= overheatTime)
            {
                // �I�[�o�[�q�[�g���B
                isOverheated = true;
                // �������x�͂�����Ȃ��i���������Ă����̂܂܁j
                chargeEffectInstance.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            UpdateChargeEffect();
        }
    }

    private bool CheckIfGrounded()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position - Vector3.up * 0.1f, groundCheckRadius);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag(groundTag))
            {
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

            //if (speedManager != null)
            //{
            //    SetSpeedDirectly(originalSpeed); // �X�s�[�h�����ɖ߂�
            //}
            return;
        }

        float jumpMultiplier = 1.0f;

        // �`���[�W���Ԃɂ���ăW�����v�{������
        if (chargeTimer >= chargeRedTime)
        {
            jumpMultiplier = 2.0f;
        }
        else if (chargeTimer >= chargeYellowTime)
        {
            jumpMultiplier = 1.5f;
        }
        else
        {
            jumpMultiplier = 1.0f;
        }

        float jumpPower = baseJumpForce * jumpMultiplier;
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        Debug.Log($"�W�����v�����I�{��: {jumpMultiplier}�{, �`���[�W����: {chargeTimer:F2}�b");

        if (speedManager != null)
        {
            // �W�����v��X�s�[�h�߂��i�I�[�o�[�q�[�g���ĂȂ��ꍇ�j
            SetSpeedDirectly(originalSpeed);
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

    // �������u�ԁi�`���[�W�J�n�j
    private void OnJumpStarted(InputAction.CallbackContext context)
    {
     
        if (isGrounded)
        {
            isCharging = true;
            chargeTimer = 0.0f;
            isOverheated = false;
            originalSpeed = speedManager.GetPlayerSpeed; // ���������̑��x��ۑ�
            Debug.Log("�`���[�W�J�n");

            if (chargeEffectPrefab != null)
            {
                if (chargeEffectInstance == null)
                {
                    // �܂��C���X�^���X����ĂȂ���������
                    chargeEffectInstance = Instantiate(chargeEffectPrefab, transform);
                    chargeEffectInstance.transform.localPosition = Vector3.zero;
                }

                chargeEffectInstance.Play();
            }
        }
    }

    // �������u�ԁi�W�����v����j
    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        if (isCharging)
        {
            if (isGrounded)
            {
                Jump(); // �����ŃW�����v�I�I
            }
            else
            {
                // �󒆂Ń{�^���������烊�Z�b�g����
                chargeTimer = 0.0f;
                isCharging = false;
                isOverheated = false;
                if (chargeEffectInstance != null)
                {
                    chargeEffectInstance.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    ResetChargeEffect();
                }
            }
        }
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
}