//======================================================
// HitSnackSound�X�N���v�g
// �쐬�ҁF�{��
// �ŏI�X�V���F4/16
// 
// [Log]4/16 �{�с@�X�i�b�N�ɓ�����������SE��ǉ�
//
//======================================================
using UnityEngine;

public class HitSnackSound : MonoBehaviour
{
    //�ǂƂ̏Փˉ���AudioClip��ݒ肷�邽�߂̕ϐ�
    public AudioClip SnackSound;

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
        if (collision.gameObject.CompareTag("Snack"))
        {
            AudioSource.PlayOneShot(SnackSound);
        }
    }
}
