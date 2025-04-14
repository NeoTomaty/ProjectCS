using UnityEngine;

public class PlayerJumpSound : MonoBehaviour
{
    public AudioSource JumpSound;
    private bool IsGrounded;

    // Update is called once per frame
    void Update()
    {
        //�n�ʂɂ��邩�ǂ������`�F�b�N
        IsGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        //�W�����v�����Ƃ�
        if(IsGrounded&&Input.GetButtonDown("Jump"))
        {
            JumpSound.Play();
        }
    }
}
