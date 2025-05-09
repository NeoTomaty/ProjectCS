//======================================================
// [GameClearSequence]
// 作成者：荒井修
// 最終更新日：05/08
// 
// [Log]
// 05/08　荒井　仮のクリア演出を作成
//======================================================
using UnityEngine;
using UnityEngine.UI;

// ClearUIプレハブにアタッチされている
// ClearConditionsスクリプトにこのスクリプトをセットしていない場合、クリア演出が再生されず即座にシーン遷移する
public class GameClearSequence : MonoBehaviour
{
    // ゲーム終了を示すUI
    [SerializeField] GameObject ClearLogo;

    // クリア演出中の背景
    [SerializeField] GameObject ClearBackImage;

    // スナックのリスポーンを管理するスクリプト（リスポーンを無効化する処理を作っておく）
    [SerializeField] BlownAway_Ver2 BlownAway;

    // シーン遷移を管理するスクリプト
    [SerializeField] ClearConditions ClearConditions;

    // クリア演出中フラグ
    private bool IsClearSequence = false;

    // クリア後タイマー
    private float AfterTimer = 0f;

    // クリア条件を満たした時に呼び出す関数
    public void OnGameClear()
    {
        if (ClearLogo == null || ClearBackImage == null || BlownAway == null || ClearConditions == null) return;

        // スナックのリスポーンを無効化
        BlownAway.OnClear();

        // ゲーム終了を示すUIを表示
        ClearLogo.SetActive(true);

        // クリア演出中の背景を表示
        ClearBackImage.SetActive(true);

        // クリア演出中フラグを立てる
        IsClearSequence = true;
    }

    private void Start()
    {
        ClearLogo.SetActive(false);
        ClearBackImage.SetActive(false);
    }

    void Update()
    {
        if(!IsClearSequence) return;

        // キー・ボタン入力でシーン遷移
        if (Input.anyKeyDown)
        {
            ClearConditions.TriggerSceneTransition();
        }

        // タイマー進行
        AfterTimer += Time.deltaTime;

        if (AfterTimer > 1.5f)
        {
            // ゲームが止まっていなかったらここで止める
            if (Time.timeScale != 0f)
            {
                Time.timeScale = 0f;
            }
        }
    }
}
