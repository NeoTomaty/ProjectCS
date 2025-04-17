//======================================================
// HitSnackSoundスクリプト
// 作成者：宮林
// 最終更新日：4/16
// 
// [Log]4/16 宮林　スナックに当たった時のSEを追加
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
