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
    //壁との衝突音のAudioClipを設定するための変数
    public AudioClip WallSound;

    //AudioSourceコンポーネントを保持するための変数
    private AudioSource AudioSource;

    void Start()
    {
        //AudioSourceコンポーネントを取得
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