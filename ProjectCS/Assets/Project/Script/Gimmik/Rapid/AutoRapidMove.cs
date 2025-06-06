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
using UnityEngine.AI;


public class AutoRapidMove : MonoBehaviour
{
    public Transform targetPosition;
    public float speed = 10.0f;
    private bool IsMoving = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RapidGate")&&!IsMoving)
        {
            IsMoving = true;
            StartCoroutine(MoveToPosition());
        }
    }

    private System.Collections.IEnumerator MoveToPosition()
    {
        MovePlayer PlayerController = GetComponent<MovePlayer>();
        if (PlayerController != null)
        {
            PlayerController.enabled = false;
        }

        while (Vector3.Distance(transform.position, targetPosition.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition.position,
                speed * Time.deltaTime 
            );
            yield return null;
        }

        if (PlayerController != null)
        {
            PlayerController.enabled = true;
        }

        IsMoving = false;
    }
}