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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioSource SnackSound;

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Snack"))
        {
            SnackSound.Play();
        }
    }
}
