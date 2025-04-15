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
