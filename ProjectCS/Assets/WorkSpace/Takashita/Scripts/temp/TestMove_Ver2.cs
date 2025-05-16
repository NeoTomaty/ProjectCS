using UnityEngine;
using UnityEngine.InputSystem;

public class TestMove_Ver2 : MonoBehaviour
{
    public PlayerSpeedManager PlayerSpeedManager; // 速度管理クラス
    public MovePlayer MovePlayer; // プレイヤー移動クラス

    [SerializeField] private float TurnSpeed = 100.0f;  // カーブの回転速度
    [SerializeField] private float TurnResponse = 1.0f; // カーブのしやすさ

    [SerializeField] private Transform CameraTrangform;
    [SerializeField] private TestCameraFunction CameraFunction;

    [SerializeField] private float RotationSpeed = 5.0f;

    private PlayerInput PlayerInput; // プレイヤーの入力を管理するcomponent
    private InputAction TurnAction;  // 左右移動用のInputAction
    private void Awake()
    {
        // 自分にアタッチされているPlayerInputを取得
        PlayerInput = GetComponent<PlayerInput>();

        // 対応するInputActionを取得
        TurnAction = PlayerInput.actions["LRTurn"];

        Vector3 vec = CameraTrangform.transform.forward;
        vec.y = 0f;

        transform.rotation = Quaternion.LookRotation(vec);
    }
    void Start()
    {
        if (PlayerSpeedManager == null)
        {
            Debug.LogWarning("MovePlayerスクリプトがアタッチされていません。");
        }
       
    }

    void Update()
    {
        if (PlayerSpeedManager == null) return;

        // 速度を取得
        float speed = PlayerSpeedManager.GetPlayerSpeed;

        // 現在の速度に応じて曲がりやすくする
        // カーブ量 = 回転速度 × 速度 × deltaTime
        float rotationAmount = TurnSpeed * (speed * TurnResponse) * Time.deltaTime;

        // プレイヤーの左右移動方向を示す変数
        float moveX = TurnAction.ReadValue<float>();

        Vector3 vec = CameraTrangform.transform.forward;
        vec.y = 0f;

        if (!PlayerInput.actions.enabled || CameraFunction.GetIsCameraInterpolating()) return;

        Debug.Log("処理通過");
        MovePlayer.SetMoveDirection(vec);
    }
}
