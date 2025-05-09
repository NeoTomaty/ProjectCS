//======================================================
// CameraFunctionスクリプト
// 作成者：竹内
// 最終更新日：05/04
// 
// [Log]
// 04/23 高下 入力に関する仕様変更(PlayerInput(InputActionAsset))
// 04/25 高下 ターゲットにロックオンしたときと解除時に補完するように調整
// 05/02 高下 リフティング時の強制ロックオン処理を追加
// 05/04 高下 プレイヤーの速度に応じて、カメラの高さを変更できるように調整
// 05/08 宮林 カメラ感度調整用のコードを追加
//======================================================
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFunction : MonoBehaviour
{
    public Transform Player;         // プレイヤー
    public Transform Target;         // 対象

    [SerializeField] private PlayerInput PlayerInput;      // プレイヤーの入力を管理するcomponent
    [SerializeField] private PlayerSpeedManager PlayerSpeedManager; // PlayerSpeedManagerコンポーネント

    [Tooltip("カメラとプレイヤーの距離")]
    [SerializeField] private float Distance = 5.0f;               // カメラとプレイヤーの距離
    [Tooltip("カメラの高さ最大値（高速時）")]
    [SerializeField] private float MaxCameraHeight = 5.0f;        // カメラの高さ（最大）
    [Tooltip("カメラの高さ最小値（低速時）")]
    [SerializeField] private float MinCameraHeight = 3.0f;        // カメラの高さ（最小）
    [SerializeField] private float DefaultRotationSpeed = 50.0f;         // 回転速度
    [SerializeField] private float VerticalMin = -80.0f;          // 縦方向移動の最小値
    [SerializeField] private float VerticalMax = 80.0f;           // 縦方向移動の最大値
    [SerializeField] private float AutoCorrectSpeed = 1.5f;       // 自動補正速度

    [Header("補完設定")]
    [SerializeField] private float TransitionDuration = 0.5f;       // ロックオン補完時間
    [SerializeField] private float ReturnDuration = 0.4f;           // 通常視点へ戻る補完時間
    [SerializeField] private float TransitionEndThreshold = 0.01f;  // 補完終了とみなす閾値

    [Header("ロックオン時のカメラ設定")]
    [SerializeField] private float LockOnDistance = 3.0f;           // ロックオン時のカメラ距離
    [SerializeField] private float LockOnHeight = 2.0f;             // ロックオン時の高さオフセット

    [Header("強制ロックオン時のカメラ設定")]
    [Tooltip("リフティングで飛ばした後のロックオン時間")]
    [SerializeField] private float ForceLockOnTime = 2.0f;          // リフティングで飛ばした後のロックオン時間

    private float yaw = 0f;
    private float pitch = 20f;

    private InputAction LookTargetAction; // ターゲット用のInputAction
    private InputAction RotateAction;     // 回転用のInputAction
    private Vector2 RotateInput;          // 入力された値を保持する（回転用）

    // ロックオン状態の制御用
    private bool IsLookingTarget = false;
    private bool IsTransitioningToTarget = false;
    private float TransitionTime = 0f;
    private Vector3 TransitionStartPos;
    private Quaternion TransitionStartRot;

    // 通常視点へ戻す制御用
    private bool IsReturningToNormal = false;
    private float ReturnTime = 0f;
    private Vector3 ReturnStartPos;
    private Quaternion ReturnStartRot;

    // リフティング時の強制ロックオン制御用
    private bool IsForceLockOn = false;
    private float ForceLockOnTimeLeft = 0f;

    //カメラ感度調整用
    private float RotationSpeed;
    private float RotationRatio=1.0f;

    private void Start()
    {
        if(!PlayerInput)
        {
            Debug.LogError("追尾対象プレイヤーのPlayerInputがアタッチされていません");
        }
        if (!PlayerSpeedManager)
        {
            Debug.LogError("プレイヤーのPlayerSpeedManagerが設定されていません");
        }

        // 対応するInputActionを取得
        LookTargetAction = PlayerInput.actions["LookTargetSnack"];
        RotateAction = PlayerInput.actions["RotateCamera"];

        //デフォルトのカメラ感度を受け渡す
        RotationSpeed = DefaultRotationSpeed;
    }

    private void LateUpdate()
    {
        if (Player == null) return;

        // RTボタンが押されているかを確認
        bool rtPressed = LookTargetAction.ReadValue<float>() > 0.5f;
        bool isManual = false;

        // 速度に応じたカメラの高さを計算
        float cameraHeight = Mathf.Lerp(MinCameraHeight, MaxCameraHeight, PlayerSpeedManager.GetSpeedRatio());

        RotationSpeed = DefaultRotationSpeed * RotationRatio;

        // カメラ回転（手動操作：右スティック等）
        if (RotateInput.x < -0.5f) { yaw -= RotationSpeed * Time.deltaTime; isManual = true; }
        if (RotateInput.x > 0.5f) { yaw += RotationSpeed * Time.deltaTime; isManual = true; }
        if (RotateInput.y > 0.5f) { pitch += RotationSpeed * Time.deltaTime; isManual = true; }
        if (RotateInput.y < -0.5f) { pitch -= RotationSpeed * Time.deltaTime; isManual = true; }

        pitch = Mathf.Clamp(pitch, VerticalMin, VerticalMax); // 縦角度の制限

        // 無操作時はプレイヤーの向きに自動補正
        if (!isManual && !Input.GetKey(KeyCode.R))
        {
            float targetYaw = Player.eulerAngles.y;
            float targetPitch = 20f;
            yaw = Mathf.LerpAngle(yaw, targetYaw, AutoCorrectSpeed * Time.deltaTime);
            pitch = Mathf.Lerp(pitch, targetPitch, AutoCorrectSpeed * Time.deltaTime);
        }

        // 強制ロックオン時の処理
        if(IsForceLockOn)
        {
            ForceLockOnTimeLeft -= Time.deltaTime;

            if (ForceLockOnTimeLeft < 0f)
            {
                IsForceLockOn = false;
                Debug.Log("強制ロックオン終了");
            }
        }

        // === ロックオン時のカメラ制御 ===
        if ((rtPressed || IsForceLockOn) && Target != null)
        {
            if (!IsLookingTarget)
            {
                StartLockOn(false);
            }

            // 目標位置と回転を計算
            Vector3 direction = (Target.position - Player.position).normalized;
            Vector3 desiredPos = Player.position - direction * LockOnDistance;
            desiredPos.y = Player.position.y + LockOnHeight;

            Quaternion desiredRot = Quaternion.LookRotation((Target.position + Vector3.up * cameraHeight) - desiredPos);

            if (IsTransitioningToTarget)
            {
                // ロックオン補完中
                TransitionTime += Time.deltaTime;
                float t = Mathf.Clamp01(TransitionTime / TransitionDuration);
                transform.position = Vector3.Lerp(TransitionStartPos, desiredPos, t);
                transform.rotation = Quaternion.Slerp(TransitionStartRot, desiredRot, t);

                if (t >= 1.0f || Vector3.Distance(transform.position, desiredPos) < TransitionEndThreshold)
                {
                    // 補完終了
                    IsTransitioningToTarget = false;
                    transform.position = desiredPos;
                    transform.rotation = desiredRot;
                }
            }
            else
            {
                // 補完後：そのままロックオン位置に追従
                transform.position = desiredPos;
                transform.rotation = desiredRot;
            }

        }
        else
        {
            // === RTを離した場合：通常視点に戻す補完 ===
            if (IsLookingTarget)
            {
                StopLockOn();
            }

            if (IsReturningToNormal)
            {
                // 通常視点へ補完中
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
                    IsReturningToNormal = false; // 補完終了
                }
            }
            else
            {
                // === 通常のカメラ追従処理 ===
                Vector3 offset = Quaternion.Euler(pitch, yaw, 0) * new Vector3(0, 0, -Distance);
                transform.position = Player.position + offset;

                // 右クリックでターゲットを注視、そうでなければプレイヤーを注視
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
        // カメラ回転アクション
        PlayerInput.actions["RotateCamera"].performed += OnRotate;

        // 入力終了時
        PlayerInput.actions["RotateCamera"].canceled += OnRotate;
    }

    private void OnDisable()
    {
        if (PlayerInput != null && PlayerInput.actions != null)
        {
            // イベント登録解除
            PlayerInput.actions["RotateCamera"].performed -= OnRotate;
            PlayerInput.actions["RotateCamera"].canceled -= OnRotate;
        }
    }

    // 入力イベント（押された/離された）で呼ばれる
    public void OnRotate(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // 入力が行われた
            RotateInput = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            // 入力がキャンセルされた
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

        // 渡された引数によって、任意ロックオンか強制ロックオンを判断
        IsForceLockOn = isForceLockOn;
        if(IsForceLockOn)
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

}