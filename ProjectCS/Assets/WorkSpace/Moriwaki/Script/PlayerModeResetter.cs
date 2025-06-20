//======================================================
// [PlayerModeResetter]
// 作成者：森脇
// 作成日：6/20
//
// [説明]
// プレイヤーの状態を監視し、特定の条件でモデルを強制的に
// 通常状態（rotationModel）に戻す
// - LiftingJump.IsJumpingがtrueになった時
// - アニメーションイベントからの呼び出し
//======================================================

using UnityEngine;

[RequireComponent(typeof(PlayerAnimationController))]
[RequireComponent(typeof(LiftingJump))]
public class PlayerModeResetter : MonoBehaviour
{
    // 制御対象のコンポーネント
    private PlayerAnimationController playerAnimationController;

    private LiftingJump liftingJump;

    // 前のフレームでのジャンプ状態を記憶するためのフラグ
    private bool wasJumpingLastFrame = false;

    private void Awake()
    {
        // このGameObjectにアタッチされているコンポーネントを自動で取得します。
        playerAnimationController = GetComponent<PlayerAnimationController>();
        liftingJump = GetComponent<LiftingJump>();
    }

    private void Update()
    {
        // ジャンプ状態を監視し、必要であればリセット処理を呼び出す
        CheckJumpAndReset();
    }

    private void CheckJumpAndReset()
    {
        // liftingJumpコンポーネントがなければ何もしない
        if (liftingJump == null)
        {
            return;
        }

        // 現在ジャンプ中(IsJumping == true) かつ、前のフレームではジャンプしていなかった場合
        if (liftingJump.IsLiftingPart && !wasJumpingLastFrame)
        {
            Debug.Log("ジャンプを検知しました。モデルをrotationModelに戻します。");
            ForceReturnToRotationModel();
        }

        // 現在のジャンプ状態を「前のフレームの状態」として保存し、次のフレームの判定に備える
        wasJumpingLastFrame = liftingJump.IsLiftingPart;
    }

    public void ForceReturnToRotationModel()
    {
        if (playerAnimationController != null)
        {
            playerAnimationController.ReturnToRotationModel();
        }
        else
        {
            Debug.LogError("PlayerAnimationControllerが見つかりません！", this);
        }
    }
}