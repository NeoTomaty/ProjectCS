//BidirectionalRapidMove.cs
//作成者:中町雷我
//最終更新日:2025/06/10
//アタッチ:Playerにアタッチ
//[Log]
//06/10　中町　Playerが強制移動ギミックに触れると強制移動する処理
//06/19　中町　強制移動中のSE実装

using UnityEngine;
using System.Collections;

public class BidirectionalRapidMove : MonoBehaviour
{
    //プレイヤーが進むポイント(StartGate→EndGate)
    public Transform[] ForwardPoints;

    //プレイヤーが戻るポイント(EndGate→StartGate)
    public Transform[] BackwardPoints;

    //プレイヤーの移動速度
    public float Speed = 10.0f;

    //現在強制移動中かどうかのフラグ
    private bool IsMoving = false;

    //プレイヤーのRigidbody(物理挙動制御用)
    private Rigidbody rb;

    //プレイヤーの操作スクリプト(強制移動を制御)
    private MovePlayer PlayerController;

    [Header("強制移動ギミックに触れたときのSE")]
    //再生するSE
    public AudioClip MoveSE;

    //AudioSourceコンポーネント
    private AudioSource audioSource;

    private void Start()
    {
        //RigidbodyとMovePlayerスクリプトを取得
        rb = GetComponent<Rigidbody>();
        PlayerController = GetComponent<MovePlayer>();
        audioSource = GetComponent<AudioSource>();

        //ForwardPointsとBackwardPointsのコライダーを無視設定
        IgnoreCollisions(ForwardPoints);
        IgnoreCollisions(BackwardPoints);
    }

    //各ポイントとの衝突を無視する設定
    private void IgnoreCollisions(Transform[] Points)
    {
        foreach (Transform Point in Points)
        {
            Collider PointCollider = Point.GetComponent<Collider>();
            Collider PlayerCollider = GetComponent<Collider>();

            if(PointCollider != null && PlayerCollider != null)
            {
                Physics.IgnoreCollision(PlayerCollider, PointCollider);
            }
        }
    }

    //ゲートに入ったときの処理
    private void OnTriggerEnter(Collider other)
    {
        //すでに移動中、またはプレイヤーでないときは無視
        if(IsMoving || !gameObject.CompareTag("Player"))
        {
            return;
        }

        //StartGateに入ったら前進ルートへ
        if(other.CompareTag("StartGate"))
        {
            StartCoroutine(MoveThroughPoints(ForwardPoints));
        }
        //EndGateに入ったら後退ルートへ
        else if (other.CompareTag("EndGate"))
        {
            StartCoroutine(MoveThroughPoints(BackwardPoints));
        }
    }

    //ポイントを順に移動するコルーチン
    private IEnumerator MoveThroughPoints(Transform[] Points)
    {
        //移動中フラグを立てる
        IsMoving = true;

        //プレイヤー操作を無効化
        if(PlayerController != null)
        {
            PlayerController.enabled = false;
        }

        //Rigidbodyの物理挙動を停止
        if (rb != null)
        {
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        //SEを再生
        if (audioSource != null && MoveSE != null)
        {
            audioSource.clip = MoveSE;
            audioSource.loop = true;
            audioSource.Play();
        }

        //各ポイントに向かって順に移動
        foreach (Transform Target in Points)
        {
            while (Vector3.Distance(transform.position, Target.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(
                transform.position,
                Target.position,
                Speed * Time.deltaTime
                );
                yield return null;
            }
            //各ポイントで少し待機
            yield return new WaitForSeconds(0.1f);
        }

        //SEを停止
        if(audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }

        //移動完了後、物理挙動と操作を元に戻す
        if(rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        if(PlayerController != null)
        {
            PlayerController.enabled = true;
        }

        //3秒後に再び移動可能にする
        yield return new WaitForSeconds(3.0f);

        IsMoving = false;
    }
}