//======================================================
// [Trampolin]
// 作成者：荒井修
// 最終更新日：06/19
// 
// [Log]
// 05/20　荒井　触れたプレイヤーをジャンプさせる処理を実装
// 06/19　中町　トランポリンSE実装
//======================================================
using UnityEngine;

public class Trampolin : MonoBehaviour
{
    [SerializeField] private float JumpPower = 100f; // ジャンプ力

    [SerializeField] private AudioClip JumpSE;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // プレイヤーに衝突したら
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーのRigidbodyを取得
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                // ジャンプ力を加える
                playerRigidbody.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);

                if(JumpSE != null)
                {
                    audioSource.PlayOneShot(JumpSE);
                }
            }
        }
    }
}
