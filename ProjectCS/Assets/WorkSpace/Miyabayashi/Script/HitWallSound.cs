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
    public AudioSource WallSound;

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            WallSound.Play();
        }
    }
}