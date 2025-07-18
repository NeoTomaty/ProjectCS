//====================================================
// スクリプト名：ChargeJumpPlayer
// 作成者：宮林
// 内容：プレイヤーのジャンプ処理
// 最終更新日：06/23
//
// [Log]
// 04/26　宮林　スクリプト作成
// 04/26  宮林　高下君のスクリプトコピー
// 04/26　宮林　長押しジャンプの追加
// 04/27　森脇　エフェクト制御の追加
// 04/29　荒井　チャージ後のジャンプ処理にリフティングジャンプへの分岐を追加
// 05/03　荒井　ジャンプ時にスピードを元に戻す処理をswitch分岐内に移動
// 05/03　森脇　ジャンプ外部へ渡す
// 06/05　荒井　効果音の再生処理を追加（ジャンプ＆着地）
// 06/19　藤本　チャージエフェクト開始処理を追記
// 06/23　荒井　カウントダウン中だったらジャンプをしないように変更
//====================================================
using UnityEngine;
using UnityEngine.InputSystem;

public class ChargeJumpPlayer : MonoBehaviour
{
    [Header("ジャンプ設定")]
    [SerializeField] private float baseJumpForce = 10.0f;

    [SerializeField] private float groundCheckRadius = 0.2f;

    [SerializeField] private float NormalJumpForce = 10.0f;

    [Header("ジャンプ倍率設定")]
    [SerializeField] private float ChargeBluePower = 1.0f; // 押してる間は0.8倍！

    [SerializeField] private float ChargeYellowPower = 1.5f; // 押してる間は0.8倍！
    [SerializeField] private float ChargeRedPower = 2.0f; // 押してる間は0.8倍！

    [Header("チャージ設定")]
    [SerializeField] private float chargeYellowTime = 1.0f;

    [SerializeField] private float chargeRedTime = 2.0f;
    [SerializeField] private float overheatTime = 3.0f;

    [Header("スピード設定")]
    [SerializeField] private float chargingSpeedMultiplier = 0.8f; // 押してる間は0.8倍！

    [SerializeField] private ParticleSystem chargeEffectPrefab; // プレハブを指定！
    private ParticleSystem chargeEffectInstance; // インスタンスを保存する用

    [Header("エフェクト")]
    [SerializeField] private PlayerEffectController playerEffectController;

    [Header("カウントダウン参照")]
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
    private bool isOverheated = false; // オーバーヒートしたか

    private PlayerStateManager PlayerStateManager;
    private LiftingJump LiftingJump;

    public float JumpPower = 0.0f;

    [SerializeField] private float chargeThresholdTime = 0.3f; // 追加：0.3秒以上でチャージ開始
    private float jumpButtonHoldTime = 0.0f;
    private bool isJumpButtonPressed = false;

    private bool isJumping = false;
    public bool IsJumping => isJumping;

    private bool requestStartCharging = false;//空中でジャンプボタンの長押しを始めたとき用

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
            Debug.LogError("PlayerInputが見つかりません！");
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
            Debug.LogError("PlayerSpeedManagerが見つかりません！");
        }

        PlayerStateManager = GetComponent<PlayerStateManager>();
        LiftingJump = GetComponent<LiftingJump>();
    }

    void Update()
    {
        bool wasGrounded = isGrounded;
        isGrounded = CheckIfGrounded();

        // 着地時にチャージ開始判定
        if (!wasGrounded && isGrounded)
        {
            if (!isCharging && isJumpButtonPressed && jumpButtonHoldTime >= chargeThresholdTime)
            {
                StartCharging();
            }

            // 着地音
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
                // チャージエフェクト開始
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
            // オーバーヒートしてたらジャンプキャンセル
            Debug.Log("オーバーヒートしてジャンプ失敗");
            chargeTimer = 0.0f;
            isCharging = false;
            isOverheated = false;

            return;
        }

        float jumpMultiplier = 1.0f;

        // チャージ時間によってジャンプ倍率決定
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
                // 通常状態
                JumpPower = baseJumpForce * jumpMultiplier;
                rb.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
                isJumping = true; // ジャンプ中フラグを立てる
                Debug.Log($"ジャンプ成功！倍率: {jumpMultiplier}倍, チャージ時間: {chargeTimer:F2}秒");
                if (speedManager != null)
                {
                    // ジャンプ後スピード戻す（オーバーヒートしてない場合）
                    SetSpeedDirectly(originalSpeed);
                }
                break;

            case PlayerStateManager.LiftingState.LiftingPart:
                // リフティング状態
                LiftingJump.SetJumpPower(jumpMultiplier);
                LiftingJump.StartLiftingJump();
                if (speedManager != null)
                {
                    // ジャンプ後スピード戻す（オーバーヒートしてない場合）
                    SetSpeedDirectly(originalSpeed);
                }
                break;
        }

        if (chargeEffectInstance != null)
        {
            chargeEffectInstance.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ResetChargeEffect();
        }

        // 変数リセット
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

    // 0.3秒超えたら呼ばれるチャージ開始処理
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

        Debug.Log("チャージ開始");
    }

    // 押した瞬間（チャージ開始）
    // ジャンプボタン押したとき
    private void OnJumpStarted(InputAction.CallbackContext context)
    {
        // カウントダウン中は無効化
        if (GameStartCountdownScript != null && GameStartCountdownScript.IsCountingDown) return;

        isJumpButtonPressed = true;
        jumpButtonHoldTime = 0.0f;
        isOverheated = false;
        originalSpeed = speedManager.GetPlayerSpeed;
    }

    // ジャンプボタン離したとき
    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        // カウントダウン中は無効化
        if (GameStartCountdownScript != null && GameStartCountdownScript.IsCountingDown) return;

        isJumpButtonPressed = false;

        if (isCharging && isGrounded)
        {
            Jump(); // チャージジャンプ
        }
        else if (!isCharging && isGrounded && jumpButtonHoldTime < chargeThresholdTime)
        {
            // 通常ジャンプ（※ホールド時間が短い時だけ）
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

        // 色が変わったらエフェクトをリスタートする処理を追加
        if (main.startColor.color != targetColor)
        {
            main.startColor = targetColor;
            chargeEffectInstance.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            chargeEffectInstance.Play();
        }

        // サイズ変更
        float startScale = 2.0f;
        float endScale = 0.5f;
        float scale = Mathf.Lerp(startScale, endScale, chargeRatio);
        chargeEffectInstance.transform.localScale = Vector3.one * scale;
    }

    private void ResetChargeEffect()
    {
        if (chargeEffectInstance == null) return;

        var main = chargeEffectInstance.main;
        main.startColor = Color.cyan; // 最初はシアンに戻す

        chargeEffectInstance.transform.localScale = Vector3.one * 2.0f; // サイズも戻す
    }

    private void ResetChargeState()
    {
        chargeTimer = 0.0f;
        isCharging = false;
        isOverheated = false;
        jumpButtonHoldTime = 0.0f;
        requestStartCharging = false; // ←追加！

        if (chargeEffectInstance != null)
        {
            chargeEffectInstance.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ResetChargeEffect();
        }
    }

}