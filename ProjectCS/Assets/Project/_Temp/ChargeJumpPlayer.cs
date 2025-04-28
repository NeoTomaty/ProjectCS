//====================================================
// スクリプト名：ChargeJumpPlayer
// 作成者：宮林
// 内容：プレイヤーのジャンプ処理
// 最終更新日：04/27
//
// [Log]
// 04/26　宮林　スクリプト作成
// 04/26  宮林　高下君のスクリプトコピー
// 04/26　宮林　長押しジャンプの追加
// 04/27　森脇　エフェクト制御の追加
//====================================================
using UnityEngine;
using UnityEngine.InputSystem;

public class ChargeJumpPlayer : MonoBehaviour
{
    [Header("ジャンプ設定")]
    [SerializeField] private float baseJumpForce = 10.0f;

    [SerializeField] private float groundCheckRadius = 0.2f;

    [Header("チャージ設定")]
    [SerializeField] private float chargeYellowTime = 1.0f;

    [SerializeField] private float chargeRedTime = 2.0f;
    [SerializeField] private float overheatTime = 3.0f;

    [Header("スピード設定")]
    [SerializeField] private float chargingSpeedMultiplier = 0.8f; // 押してる間は0.8倍！

    [SerializeField] private ParticleSystem chargeEffectPrefab; // プレハブを指定！
    private ParticleSystem chargeEffectInstance; // インスタンスを保存する用

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
    }

    private void Update()
    {
        isGrounded = CheckIfGrounded();

        if (isCharging)
        {
            chargeTimer += Time.deltaTime;

            if (!isOverheated && chargeTimer < overheatTime)
            {
                // ボタン押している間は常に0.8倍
                float targetSpeed = originalSpeed * chargingSpeedMultiplier;
                SetSpeedDirectly(targetSpeed);
            }
            else if (chargeTimer >= overheatTime)
            {
                // オーバーヒート到達
                isOverheated = true;
                // もう速度はいじらない（押し続けてもそのまま）
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
            // オーバーヒートしてたらジャンプキャンセル
            Debug.Log("オーバーヒートしてジャンプ失敗");
            chargeTimer = 0.0f;
            isCharging = false;
            isOverheated = false;

            //if (speedManager != null)
            //{
            //    SetSpeedDirectly(originalSpeed); // スピードも元に戻す
            //}
            return;
        }

        float jumpMultiplier = 1.0f;

        // チャージ時間によってジャンプ倍率決定
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
        Debug.Log($"ジャンプ成功！倍率: {jumpMultiplier}倍, チャージ時間: {chargeTimer:F2}秒");

        if (speedManager != null)
        {
            // ジャンプ後スピード戻す（オーバーヒートしてない場合）
            SetSpeedDirectly(originalSpeed);
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

    // 押した瞬間（チャージ開始）
    private void OnJumpStarted(InputAction.CallbackContext context)
    {
     
        if (isGrounded)
        {
            isCharging = true;
            chargeTimer = 0.0f;
            isOverheated = false;
            originalSpeed = speedManager.GetPlayerSpeed; // 押した時の速度を保存
            Debug.Log("チャージ開始");

            if (chargeEffectPrefab != null)
            {
                if (chargeEffectInstance == null)
                {
                    // まだインスタンス作ってなかったら作る
                    chargeEffectInstance = Instantiate(chargeEffectPrefab, transform);
                    chargeEffectInstance.transform.localPosition = Vector3.zero;
                }

                chargeEffectInstance.Play();
            }
        }
    }

    // 離した瞬間（ジャンプする）
    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        if (isCharging)
        {
            if (isGrounded)
            {
                Jump(); // ここでジャンプ！！
            }
            else
            {
                // 空中でボタン離したらリセットだけ
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
}