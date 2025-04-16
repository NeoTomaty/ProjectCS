//PlayerLandingSound.cs
//作成者:中町雷我
//最終更新日:2025/04/15
//[Log]
//04/15　中町　Playerが着地したときの効果音処理

using UnityEngine;

public class PlayerLandingSound : MonoBehaviour
{
    public AudioSource LandingSound;
    private bool IsGrounded;
    private bool WasGrounded;

    // Update is called once per frame
    void Update()
    {
        //地面にいるかどうかをチェック
        IsGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        //着地したとき
        if(!WasGrounded&&IsGrounded)
        {
            LandingSound.Play();
        }

        //前のフレームの状態を更新
        WasGrounded = IsGrounded;
    }
}
