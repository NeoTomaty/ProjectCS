//======================================================
// HitWallSoundスクリプト
// 作成者：宮林
// 最終更新日：4/16
// 
// [Log]4/16 宮林 壁に当たった時のSEを追加　
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