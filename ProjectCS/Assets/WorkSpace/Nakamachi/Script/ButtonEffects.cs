//ButtonEffects.cs
//作成者:中町雷我
//最終更新日:2025/05/08
//アタッチ:StartButton、ExitButtonにアタッチ
//[Log]
//05/08　中町　ボタンにマウスカーソルを合わすとテキストの色と文字が1.2倍拡大する処理

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //ボタンのテキストを格納する変数
    public Text ButtonText;
    //元のスケールを格納する変数
    private Vector3 OriginalScale;
    //元の色を格納する変数
    private Color OriginalColor;

    //初期化処理を行う関数
    void Start()
    {
        //ボタンテキストの元のスケールを取得
        OriginalScale = ButtonText.transform.localScale;

        //ボタンテキストの元の色を取得
        OriginalColor = ButtonText.color;
    }

    //マウスポインターがボタンに入ったときに呼び出される関数
    public void OnPointerEnter(PointerEventData EventData)
    {
        //ボタンテキストのスケールを1.2倍に拡大
        ButtonText.transform.localScale = OriginalScale * 1.2f;

        //ボタンテキストの色を赤に変更
        ButtonText.color = Color.red;
    }

    //マウスポインターがボタンから出たときに呼び出される関数
    public void OnPointerExit(PointerEventData EventData)
    {
        //ボタンテキストのスケールを元に戻す
        ButtonText.transform.localScale = OriginalScale;

        //ボタンテキストの色を元に戻す
        ButtonText.color = OriginalColor;
    }
}
