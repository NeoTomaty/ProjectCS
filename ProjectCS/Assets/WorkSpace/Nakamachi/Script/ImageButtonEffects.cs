//ImageButtonEffects.cs
//作成者:中町雷我
//最終更新日:2025/05/11
//[Log]
//05/11　中町　ボタンの拡大表示

using UnityEngine;
using UnityEngine.EventSystems;

//このクラスは、UIボタンに対してマウスオーバーや選択時に拡大する視覚効果を与えるためのもの
//IPointerEnterHandler,IPointerExitHandler,ISelectHandler,IDeselectHandlerを実装することで、マウスやキーボードによるインタラクションに対応する
public class ImageButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    //元のスケール(サイズ)を保持するための変数
    private Vector3 OriginalScale;

    //ゲームオブジェクトの元のスケールを保存する
    void Start()
    {
        OriginalScale = transform.localScale;
    }

    //マウスカーソルがボタン上に乗ったときに呼ばれる処理
    public void OnPointerEnter(PointerEventData EventData)
    {
        //ボタンのスケールを1.2倍に拡大して、視覚的に強調する
        transform.localScale = OriginalScale * 1.2f;
    }

    //マウスカーソルがボタンから離れたときに呼ばれる処理
    public void OnPointerExit(PointerEventData EventData)
    {
        //スケールを元に戻して、通常のサイズに戻す
        transform.localScale = OriginalScale;
    }

    //ボタンが選択されたとき(キーボードやゲームパッドで選択されたときなど)
    public void OnSelect(BaseEventData EventData)
    {
        //スケールを1.2倍に拡大して、選択されていることを視覚的に示す
        transform.localScale = OriginalScale * 1.2f;
    }

    //ボタンの選択が解除されたときに呼ばれる処理
    public void OnDeselect(BaseEventData EventData)
    {
        //スケールを元に戻して、通常のサイズに戻す
        transform.localScale = OriginalScale;
    }
}
