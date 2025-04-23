//====================================================
// スクリプト名：JumpPlayer
// 作成者：高下
// 内容：プレイヤーのジャンプ処理
// 最終更新日：04/01
// 
// [Log]
// 04/01 高下 スクリプト作成 
// 04/21 高下 重力関連を別スクリプト(ObjectGravity)に移動させました
// 04/23 高下 入力に関する仕様変更(PlayerInput(InputActionAsset))
//====================================================
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpPlayer : MonoBehaviour
{
    [SerializeField]
    private float JumpForce = 10.0f;        // ジャンプ力
    [SerializeField]
    private float GroundCheckRadius = 0.2f; // 地面判定半径

    private Rigidbody Rb;    // プレイヤーのRigidbody
    private bool IsGrounded; // 地面に接しているかどうか

    // 地面を確認するためのタグ
    private string GroundTag = "Ground";

    private PlayerInput PlayerInput; // プレイヤーの入力を管理するcomponent
    private InputAction JumpAction;  // ジャンプ用のInputAction

    private void Awake()
    {
        // 自分にアタッチされている PlayerInput を取得
        PlayerInput = GetComponent<PlayerInput>();
    }
    void Start()
    {
        Rb = GetComponent<Rigidbody>(); // Rigidbodyを取得

        // 対応するInputActionを取得
        JumpAction = PlayerInput.actions["Jump"];
    }
   
    void Update()
    {
        // 地面に接しているか確認
        IsGrounded = CheckIfGrounded();
    }

    // 地面判定
    bool CheckIfGrounded()
    {
        // 地面のタグを持つオブジェクトに接しているか確認
        // プレイヤーの足元から少し下にチェックを行い、半径内で地面のタグがあるか確認
        Collider[] colliders = Physics.OverlapSphere(transform.position - Vector3.up * 0.1f, GroundCheckRadius);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag(GroundTag))
            {
                return true; // 地面に接している場合
            }
        }
        return false; // 地面に接していない場合
    }

    // ジャンプ処理
    void Jump()
    {
        // ジャンプ力を加える
        Rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        Debug.Log("ジャンプ処理成功");
    }

    // 有効化されたときに「Jump」アクションにイベントハンドラを登録
    private void OnEnable()
    {
        // プレイヤー入力の "Jump" アクションが実行されたときに OnJump メソッドを呼び出す
        PlayerInput.actions["Jump"].performed += OnJump;
    }

    // 無効化されたときにイベントハンドラを解除
    private void OnDisable()
    {
        if (PlayerInput != null && PlayerInput.actions != null)
        {
            // "Jump" アクションに登録されていた OnJump を解除
            PlayerInput.actions["Jump"].performed -= OnJump;
        }
    }

    // 「Jump」アクションの入力が発生したときに呼ばれる処理
    public void OnJump(InputAction.CallbackContext context)
    {
        // 入力が完了（ボタンが押された瞬間）かつ地面に接地しているときにジャンプを実行
        if (context.performed && IsGrounded)
        {
            Jump();
        }
    }
}
