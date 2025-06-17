//====================================================
// スクリプト名：StageSelectSEPlayer
// 作成者：高下
// 内容：ステージ選択画面専用のSE再生スクリプト
// 最終更新日：06/17
// 
// [Log]
// 06/17 高下 スクリプト作成
//====================================================
using UnityEngine;

public class StageSelectSEPlayer : MonoBehaviour
{
    public enum StageSelectSE
    {
        Confirm,  // 決定
        Cancel,   // 戻る
        Select    // カーソル移動
    }

    [Header("決定SE")]
    [SerializeField] private AudioClip ConfirmSE;
    [Header("戻るSE")]
    [SerializeField] private AudioClip CancelSE;
    [Header("カーソル移動SE")]
    [SerializeField] private AudioClip SelectSE;

    private AudioSource AudioSourceComponent;

    void Start()
    {
        AudioSourceComponent = GetComponent<AudioSource>();
    }

    public void PlaySE(StageSelectSE se)
    {
        switch(se)
        {
            case StageSelectSE.Confirm:
                PlaySE(ConfirmSE);
                break;
            case StageSelectSE.Cancel:
                PlaySE(CancelSE);
                break;
            case StageSelectSE.Select:
                PlaySE(SelectSE);
                break;
        }
        
    }

    private void PlaySE(AudioClip clip)
    {
        if (clip != null && AudioSourceComponent != null)
        {
            AudioSourceComponent.PlayOneShot(clip);
        }
    }
}
