//====================================================
// スクリプト名：StageSelectSEPlayer
// 作成者：高下
// 内容：ステージ選択画面専用のSE再生スクリプト
// 最終更新日：06/17
// 
// [Log]
// 06/17 高下 スクリプト作成
// 06/26 中町 SE音量調整
//====================================================
using UnityEngine;

public class StageSelectSEPlayer : MonoBehaviour
{
    //ステージ選択画面で使用するSEの種類を定義
    public enum StageSelectSE
    {
        Confirm,  // 決定ボタンを押したときのSE
        Cancel,   // 戻るボタンを押したときのSE
        Select    // カーソル移動を移動したときのSE
    }

    //決定時に再生するSE
    [Header("決定SE")]
    [SerializeField] private AudioClip ConfirmSE;

    //戻る時に再生するSE
    [Header("戻るSE")]
    [SerializeField] private AudioClip CancelSE;

    //カーソル移動時に再生するSE
    [Header("カーソル移動SE")]
    [SerializeField] private AudioClip SelectSE;

    //SEの音量調整
    [Header("SE音量(0.0～1.0)")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float SEVolume = 0.5f;

    //SE再生用のAudioSourceコンポーネント
    private AudioSource AudioSourceComponent;

    //初期化処理
    void Start()
    {
        //このGameObjectにアタッチされているAudioSourceを取得
        AudioSourceComponent = GetComponent<AudioSource>();

        //AudioSourceが取得できていれば、音量を設定
        if (AudioSourceComponent != null)
        {
            AudioSourceComponent.volume = SEVolume;
        }
    }

    //指定された種類のSEを再生する関数
    public void PlaySE(StageSelectSE se)
    {
        switch(se)
        {
            case StageSelectSE.Confirm:
                PlaySE(ConfirmSE); //決定SEを再生
                break;
            case StageSelectSE.Cancel:
                PlaySE(CancelSE); //戻るSEを再生
                break;
            case StageSelectSE.Select:
                PlaySE(SelectSE); //カーソル移動SEを再生
                break;
        }
        
    }

    //実際にAudioClipを再生する内部関数
    private void PlaySE(AudioClip clip)
    {
        //AudioClipとAudioSourceが有効であれば、指定音量で再生
        if (clip != null && AudioSourceComponent != null)
        {
            AudioSourceComponent.PlayOneShot(clip,SEVolume);
        }
    }

    //外部からSE音量を変更するための関数
    public void SetSEVolume(float volume)
    {
        //0.0～1.0の範囲に制限
        SEVolume = Mathf.Clamp01(volume);

        //AudioSourceが有効であれば、音量を更新
        if (AudioSourceComponent != null)
        {
            AudioSourceComponent.volume = SEVolume;
        }
    }
}
