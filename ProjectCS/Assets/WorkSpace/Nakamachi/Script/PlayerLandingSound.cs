//PlayerLandingSound.cs
//�쐬��:��������
//�ŏI�X�V��:2025/04/15
//[Log]
//04/15�@�����@Player�����n�����Ƃ��̌��ʉ�����

using UnityEngine;

public class PlayerLandingSound : MonoBehaviour
{
    public AudioSource LandingSound;
    private bool IsGrounded;
    private bool WasGrounded;

    // Update is called once per frame
    void Update()
    {
        //�n�ʂɂ��邩�ǂ������`�F�b�N
        IsGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        //���n�����Ƃ�
        if(!WasGrounded&&IsGrounded)
        {
            LandingSound.Play();
        }

        //�O�̃t���[���̏�Ԃ��X�V
        WasGrounded = IsGrounded;
    }
}
