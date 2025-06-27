//======================================================
// HitWallSound�X�N���v�g
// �쐬�ҁF�{��
// �ŏI�X�V���F4/16
// 
// [Log]4/16 �{�� �ǂɓ�����������SE��ǉ�
// 6/27 ���� SE���ʒ�������
//======================================================
using UnityEngine;
public class HitWallSound : MonoBehaviour
{
    //�ǂƂ̏Փˉ���AudioClip��ݒ肷�邽�߂̕ϐ�
    public AudioClip WallSound;

    //AudioSource�R���|�[�l���g��ێ����邽�߂̕ϐ�
    private AudioSource AudioSource;

    //SE�̉��ʂ𒲐����邽�߂̕ϐ�(0.0�`1.0)
    [Range(0.0f, 1.0f)]
    public float volume = 0.5f;

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
            //�w�肳�ꂽ���ʂ�SE���Đ�
            AudioSource.PlayOneShot(WallSound,volume);
        }
    }
}