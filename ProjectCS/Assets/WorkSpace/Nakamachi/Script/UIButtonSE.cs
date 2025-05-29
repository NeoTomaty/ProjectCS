//SEPlayer.cs
//作成者:中町雷我
//最終更新日:2025/05/29
//アタッチ:PauseCanvasのTittleButton,StageSelect,Retry,Optionボタンにアタッチ
//[Log]
//05/29　中町　メニュー決定SE

using UnityEngine;
using UnityEngine.UI;

//このスクリプトをアタッチするオブジェクトには必ずButtonコンポーネントが必要
[RequireComponent(typeof(Button))]

public class UIButtonSE : MonoBehaviour
{
    //スクリプトの開始時に呼び出されるメソッド
    private void Start()
    {
        //このオブジェクトにアタッチされているButtonコンポーネントを取得し、クリックイベントに効果音再生処理を追加
        //SEPlayerのインスタンスが存在すれば、選択効果音を再生
        GetComponent<Button>().onClick.AddListener(() => { SEPlayer.Instance?.PlaySelectSE(); });
    }
}