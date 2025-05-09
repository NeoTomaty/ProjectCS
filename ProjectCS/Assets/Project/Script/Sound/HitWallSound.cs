//======================================================
// HitWallSound�X�N���v�g
// �쐬�ҁF�{��
// �ŏI�X�V���F4/16
// 
// [Log]4/16 �{�� �ǂɓ�����������SE��ǉ��@
//======================================================
using UnityEngine;
public class HitWallSound : MonoBehaviour
{
    //�ǂƂ̏Փˉ���AudioClip��ݒ肷�邽�߂̕ϐ�
    public AudioClip WallSound;

    //AudioSource�R���|�[�l���g��ێ����邽�߂̕ϐ�
    private AudioSource AudioSource;

    void Start()
    {
        //AudioSource�R���|�[�l���g���擾
        AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AudioSource.PlayOneShot(WallSound);
        }
    }
}