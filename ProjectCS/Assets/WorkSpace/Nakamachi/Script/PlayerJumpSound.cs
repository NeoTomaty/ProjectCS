//PlayerJumpSound.cs
//作成者:中町雷我
//最終更新日:2025/04/15
//[Log]
//04/15　中町　Playerがジャンプしたときの効果音処理

using UnityEngine;

public class PlayerJumpSound : MonoBehaviour
{
    public AudioSource JumpSound;
    private bool IsGrounded;

    // Update is called once per frame
    void Update()
    {
        //地面にいるかどうかをチェック
        IsGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        //ジャンプしたとき
        if(IsGrounded&&Input.GetButtonDown("Jump"))
        {
            JumpSound.Play();
        }
    }
}
