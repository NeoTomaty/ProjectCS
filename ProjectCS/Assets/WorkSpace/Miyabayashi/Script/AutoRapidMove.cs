//======================================================
// [スクリプト名]AutoRapidMove
// 作成者：宮林朋輝
// 最終更新日：3/31
// 
// [Log]
// 3/31  宮林　スクリプト作成
//======================================================
using UnityEngine;


public class AutoRapidMove : MonoBehaviour
{
    public Transform targetPosition; // 移動先の位置
    public float speed = 10.0f;     // 移動速度

    private void OnTriggerEnter(Collider other)
    {
        // "RapidGate" に触れたら移動開始
        if (other.CompareTag("RapidGate"))
        {
            StartCoroutine(MoveToPosition());
        }
    }

    private System.Collections.IEnumerator MoveToPosition()
    {
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
    }
}

