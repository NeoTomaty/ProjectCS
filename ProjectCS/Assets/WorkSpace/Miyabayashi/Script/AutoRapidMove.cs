//======================================================
// [スクリプト名]AutoRapidMove
// 作成者：宮林朋輝
// 最終更新日：4/8
// 
// [Log]
// 3/31  宮林　スクリプト作成
// 4/8　中町　プレイヤーが高速エリアに入ったらプレイヤーの操作を一切受け付けない処理追加
// 4/8　中町　IsMovingフラグ追加
// 4/8　中町　MoveToPosition関数にプレイヤーの操作を無効化する処理を追加
// 4/8　中町　MoveToPosition関数に移動完了後、プレイヤーの操作を再度有効化する処理追加
//======================================================
using UnityEngine;


public class AutoRapidMove : MonoBehaviour
{
    public Transform targetPosition; // 移動先の位置
    public float speed = 10.0f;     // 移動速度
    private bool IsMoving = false; // 移動中かどうかのフラグ

    private void OnTriggerEnter(Collider other)
    {
        // "RapidGate" に触れたら移動開始
        if (other.CompareTag("RapidGate")&&!IsMoving)
        {
            IsMoving = true;
            StartCoroutine(MoveToPosition());
        }
    }

    private System.Collections.IEnumerator MoveToPosition()
    {
        // プレイヤーの操作を無効化
        PlayerMoveUp PlayerController = GetComponent<PlayerMoveUp>();
        if (PlayerController != null)
        {
            PlayerController.enabled = false;
        }

        // 現在位置から目的地へ移動
        while (Vector3.Distance(transform.position, targetPosition.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition.position,
                speed * Time.deltaTime 
            );
            yield return null;
        }

        // 移動完了後、プレイヤーの操作を再度有効化(必要に応じて)
        if (PlayerController != null)
        {
            PlayerController.enabled = true;
        }

        IsMoving = false;
    }
}