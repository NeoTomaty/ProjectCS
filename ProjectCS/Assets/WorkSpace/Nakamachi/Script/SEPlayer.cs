//SEPlayer.cs
//作成者:中町雷我
//最終更新日:2025/05/29
//アタッチ:PauseManagerにアタッチ
//[Log]
//05/29　中町　メニュー決定SE

using UnityEngine;

public class SEPlayer : MonoBehaviour
{
    //SEPlayerのインスタンスを保持する静的変数(シングルトンパターン)
    public static SEPlayer Instance;

    //効果音を再生するためのAudioSource(インスペクターで設定)
    [SerializeField] private AudioSource audioSource;

    //選択時に再生する効果音(インスペクターで設定)
    [SerializeField] private AudioClip SelectSE;

    //オブジェクトが生成されたときに呼び出されるメソッド
    private void Awake()
    {
        //まだインスタンスが存在しないときはこのオブジェクトをインスタンスとして設定
        if(Instance == null)
        {
            Instance = this;

            //このオブジェクトをシーンが切り替わっても破棄しないように設定
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //すでにインスタンスが存在するときは重複を避けるためにこのオブジェクトを破棄
            Destroy(gameObject);
        }
    }

    //選択効果音を再生するメソッド(ボタンなどから呼び出す)
    public void PlaySelectSE()
    {
        //効果音とAudioSourceが設定されているときのみ再生
        if (SelectSE != null && audioSource != null)
        {
            //一回だけ効果音を再生(重ねて再生可能)
            audioSource.PlayOneShot(SelectSE);
        }
    }
}