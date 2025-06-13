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
// 05/22 高下 通常ロックオン時と強制ロックオン時の補完速度を分けて処理できるように調整
// 06/13　森脇 フィニッシュ時のカメラ制御
// 06/13 高下 通常のロックオンを廃止
// 06/13 高下 ターゲットを変更するSetSnack関数を追加
//======================================================
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFunction : MonoBehaviour
{
    public Transform Player;     // プレイヤーのTransform
    public Transform Target;     // ロックオン対象のTransform

    [SerializeField] private PlayerInput PlayerInput;            // プレイヤーの入力処理コンポーネント
    [SerializeField] private PlayerSpeedManager PlayerSpeedManager;  // プレイヤーの速度管理コンポーネント

    [Tooltip("カメラとプレイヤーの距離")]
    [SerializeField] private float Distance = 5.0f;              // 通常時のカメラ距離

    [Tooltip("カメラの高さ最大値（高速時）")]
    [SerializeField] private float MaxCameraHeight = 5.0f;       // 高速時のカメラ高さ

    [Tooltip("カメラの高さ最小値（低速時）")]
    [SerializeField] private float MinCameraHeight = 3.0f;       // 低速時のカメラ高さ

    [SerializeField] private float DefaultRotationSpeed = 50.0f; // カメラ回転速度（初期値）
    [SerializeField] private float VerticalMin = -80.0f;         // ピッチの下限
    [SerializeField] private float VerticalMax = 80.0f;          // ピッチの上限
    [SerializeField] private float AutoCorrectSpeed = 1.5f;      // 無操作時の自動補正速度

    [Header("通常ロックオン補完設定")]
    [SerializeField] private float NormalLockOnTransitionDuration = 0.5f; // 通常ロックオン開始時の補完時間

    [SerializeField] private float NormalLockOnReturnDuration = 0.4f;     // 通常ロックオン解除時の補完時間

    [Header("強制ロックオン補完設定")]
    [SerializeField] private float ForceLockOnTransitionDuration = 0.2f;  // 強制ロックオン開始時の補完時間

    [SerializeField] private float ForceLockOnReturnDuration = 0.3f;      // 強制ロックオン解除時の補完時間

    [SerializeField] private float TransitionEndThreshold = 0.01f;

    private float currentTransitionDuration = 0.5f; // ロックオン補完時間（動的切替）
    private float currentReturnDuration = 0.4f;     // ロックオン解除補完時間（動的切替）

    [Header("ロックオン時のカメラ設定")]
    [SerializeField] private float LockOnDistance = 3.0f;           // ロックオン時のカメラ距離

    [SerializeField] private float LockOnHeight = 2.0f;             // ロックオン時の高さオフセット

    [Header("強制ロックオン設定")]
    [Tooltip("強制ロックオン持続時間（例：リフティング後）")]
    [SerializeField] private float ForceLockOnTime = 2.0f;          // 強制ロックオンの持続秒数

    private bool isSpecialViewActive = false;    // 特別視点モードが有効かどうかのフラグ
    private Vector3 specialViewRelativePosition; // プレイヤーからの相対座標を保持する変数

    // カメラの回転状態（ヨー・ピッチ）
    private float yaw = 0f;

    private float pitch = 20f;

    // 入力アクションと入力値
    private InputAction LookTargetAction;   // ロックオンボタン入力

    private InputAction RotateAction;       // カメラ回転入力
    private Vector2 RotateInput;            // 回転入力ベクトル

    // 状態フラグ
    private bool IsLookingTarget = false;       // ロックオン中かどうか

    private bool IsTransitioningToTarget = false;  // ロックオン補完中かどうか
    private bool IsReturningToNormal = false;     // 通常視点へ復帰補完中かどうか
    private bool IsForceLockOn = false;           // 強制ロックオン中かどうか

    // 補完処理用タイマーと補完開始時の状態記録
    private float TransitionTime = 0f;

    private Vector3 TransitionStartPos;
    private Quaternion TransitionStartRot;

    private float ReturnTime = 0f;
    private Vector3 ReturnStartPos;
    private Quaternion ReturnStartRot;

    private float ForceLockOnTimeLeft = 0f;       // 強制ロックオン残り時間

    private float RotationSpeed;                 // 実際の回転速度（感度補正含む）
    private float RotationRatio = 1.0f;          // 回転感度の倍率

    [Header("特別視点設定")]
    [Tooltip("特別視点への移行速度")]
    [SerializeField] private float SpecialViewTransitionSpeed = 3.0f;

    [SerializeField] private float specialViewTransitionDuration = 1.0f;
    private Vector3 specialViewStartPos;
    private Quaternion specialViewStartRot;
    private float specialViewElapsedTime = 0f;
    private bool wasInSpecialView = false;

    private void Start()
    {
        if (!PlayerInput) Debug.LogError("PlayerInput が設定されていません");
        if (!PlayerSpeedManager) Debug.LogError("PlayerSpeedManager が設定されていません");

        // 各InputActionを取得
        LookTargetAction = PlayerInput.actions["LookTargetSnack"];
        RotateAction = PlayerInput.actions["RotateCamera"];

        RotationSpeed = DefaultRotationSpeed;
    }

    private void LateUpdate()
    {
        if (Player == null) return;

        if (isSpecialViewActive)
        {
            if (!wasInSpecialView)
            {
                specialViewStartPos = transform.position;
                specialViewStartRot = transform.rotation;
                specialViewElapsedTime = 0f;
                wasInSpecialView = true;
            }

            specialViewElapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(specialViewElapsedTime / specialViewTransitionDuration);

            Vector3 desiredPosition = Player.transform.TransformPoint(specialViewRelativePosition);
            Vector3 lookAtPoint = Player.position + Vector3.up * 1.0f;
            Quaternion desiredRotation = Quaternion.LookRotation(lookAtPoint - desiredPosition);

            transform.position = Vector3.Lerp(specialViewStartPos, desiredPosition, t);
            transform.rotation = Quaternion.Slerp(specialViewStartRot, desiredRotation, t);

            return;
        }
        else
        {
            if (wasInSpecialView)
            {
                wasInSpecialView = false;
                specialViewElapsedTime = 0f;
            }
        }

        //bool rtPressed = LookTargetAction.ReadValue<float>() > 0.5f;

        bool rtPressed = false;

        bool isManual = false;

        // プレイヤーの速度に応じてカメラの高さを補間
        float cameraHeight = Mathf.Lerp(MinCameraHeight, MaxCameraHeight, PlayerSpeedManager.GetSpeedRatio());

        RotationSpeed = DefaultRotationSpeed * RotationRatio;

        // カメラのマニュアル回転操作（右スティック）
        if (RotateInput.x < -0.5f) { yaw -= RotationSpeed * Time.deltaTime; isManual = true; }
        if (RotateInput.x > 0.5f) { yaw += RotationSpeed * Time.deltaTime; isManual = true; }
        if (RotateInput.y > 0.5f) { pitch += RotationSpeed * Time.deltaTime; isManual = true; }
        if (RotateInput.y < -0.5f) { pitch -= RotationSpeed * Time.deltaTime; isManual = true; }

        pitch = Mathf.Clamp(pitch, VerticalMin, VerticalMax);

        // 無操作時はプレイヤー方向に補正
        if (!isManual && !Input.GetKey(KeyCode.R))
        {
            yaw = Mathf.LerpAngle(yaw, Player.eulerAngles.y, AutoCorrectSpeed * Time.deltaTime);
            pitch = Mathf.Lerp(pitch, 20f, AutoCorrectSpeed * Time.deltaTime);
        }

        // 強制ロックオン時間のカウントダウン
        if (IsForceLockOn)
        {
            ForceLockOnTimeLeft -= Time.deltaTime;
            if (ForceLockOnTimeLeft <= 0f)
            {
                StopLockOn();
                IsForceLockOn = false;
                Debug.Log("強制ロックオン終了");
            }
        }

        // ロックオン処理（RT押下 または 強制ロックオン）
        if ((rtPressed || IsForceLockOn) && Target != null)
        {
            if (!IsLookingTarget)
            {
                StartLockOn(false);
            }

            Vector3 playerPos = Player.position;
            Vector3 targetPos = Target.position;
            Vector3 toTarget = (targetPos - playerPos).normalized;

            // プレイヤーから後方に配置（ターゲット反対方向）
            Vector3 desiredPos = playerPos - toTarget * LockOnDistance;
            desiredPos.y += LockOnHeight;

            Quaternion desiredRot = Quaternion.LookRotation((targetPos + Vector3.up * cameraHeight) - desiredPos);

            if (IsTransitioningToTarget)
            {
                TransitionTime += Time.deltaTime;
                float t = Mathf.Clamp01(TransitionTime / currentTransitionDuration); // ←切り替えた補完時間を使用
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
            // ロックオン解除時
            if (IsLookingTarget) StopLockOn();

            if (IsReturningToNormal)
            {
                ReturnTime += Time.deltaTime;
                float t = Mathf.Clamp01(ReturnTime / currentReturnDuration); // ← ここで切り替えた値を使用

                Vector3 offset = Quaternion.Euler(pitch, yaw, 0) * new Vector3(0, 0, -Distance);
                Vector3 normalPos = Player.position + offset;

                Quaternion normalRot = (Input.GetMouseButton(1) && Target != null)
                    ? Quaternion.LookRotation((Target.position + Vector3.up * cameraHeight) - normalPos)
                    : Quaternion.LookRotation((Player.position + Vector3.up * cameraHeight) - normalPos);

                transform.position = Vector3.Lerp(ReturnStartPos, normalPos, t);
                transform.rotation = Quaternion.Slerp(ReturnStartRot, normalRot, t);

                if (t >= 1.0f)
                {
                    IsReturningToNormal = false;
                }
            }
            else
            {
                Vector3 offset = Quaternion.Euler(pitch, yaw, 0) * new Vector3(0, 0, -Distance);
                transform.position = Player.position + offset;

                if (Input.GetMouseButton(1) && Target != null)
                    transform.LookAt(Target.position + Vector3.up * cameraHeight);
                else
                    transform.LookAt(Player.position + Vector3.up * cameraHeight);
            }
        }
    }

    private void OnEnable()
    {
        PlayerInput.actions["RotateCamera"].performed += OnRotate;
        PlayerInput.actions["RotateCamera"].canceled += OnRotate;
    }

    private void OnDisable()
    {
        if (PlayerInput != null && PlayerInput.actions != null)
        {
            PlayerInput.actions["RotateCamera"].performed -= OnRotate;
            PlayerInput.actions["RotateCamera"].canceled -= OnRotate;
        }
    }

    // 回転入力を取得（スティック操作）
    public void OnRotate(InputAction.CallbackContext context)
    {
        RotateInput = context.performed ? context.ReadValue<Vector2>() : Vector2.zero;
    }

    // ロックオン開始
    public void StartLockOn(bool isForceLockOn)
    {
        if (Target == null || Player == null) return;

        IsLookingTarget = true;
        IsTransitioningToTarget = true;
        TransitionTime = 0.0f;
        TransitionStartPos = transform.position;
        TransitionStartRot = transform.rotation;

        IsForceLockOn = isForceLockOn;

        // 状態に応じて補完時間を切り替え
        currentTransitionDuration = isForceLockOn
            ? ForceLockOnTransitionDuration
            : NormalLockOnTransitionDuration;

        if (IsForceLockOn)
        {
            ForceLockOnTimeLeft = ForceLockOnTime;
        }
    }

    // ロックオン解除処理
    public void StopLockOn()
    {
        if (!IsLookingTarget) return;

        IsLookingTarget = false;
        IsTransitioningToTarget = false;
        IsReturningToNormal = true;
        ReturnTime = 0.0f;
        ReturnStartPos = transform.position;
        ReturnStartRot = transform.rotation;

        // 状態に応じてロックオン解除補完時間を切り替え
        currentReturnDuration = IsForceLockOn
            ? ForceLockOnReturnDuration
            : NormalLockOnReturnDuration;

        Debug.Log("ロックオン終了速度" + currentReturnDuration);
    }

    // ターゲットを外部から指定
    public void SetLockOnTarget(Transform target)
    {
        Target = target;
    }

    // 回転感度倍率の設定
    public void SetRatio(float ratio)
    {
        RotationRatio = ratio;
    }


    public void StartSpecialView(Vector3 relativePosition)
    {
        // 特別視点モードを有効にし、目標の相対位置を保存
        this.isSpecialViewActive = true;
        this.specialViewRelativePosition = relativePosition;

        Debug.Log("特別視点モード開始。目標相対位置: " + relativePosition);
    }

    public void StopSpecialView()
    {
        // モードが有効でなければ何もしない
        if (!isSpecialViewActive) return;

        isSpecialViewActive = false;

        // 通常視点へのスムーズな復帰処理を開始する
        // 既存の復帰ロジック(IsReturningToNormal)を再利用
        IsReturningToNormal = true;
        ReturnTime = 0.0f;
        ReturnStartPos = transform.position;
        ReturnStartRot = transform.rotation;

        // 復帰後のカメラ角度をプレイヤーの現在の向きに合わせることで、自然な視点復帰を促す
        yaw = Player.eulerAngles.y;
        pitch = 20f; // デフォルトのピッチ角に戻す

        Debug.Log("特別視点モード終了。通常視点へ復帰します。");
    }

    // ロックオンする対象を設定する
    public void SetSnack(Transform Snack)
    {
        Target = Snack;

    }
}