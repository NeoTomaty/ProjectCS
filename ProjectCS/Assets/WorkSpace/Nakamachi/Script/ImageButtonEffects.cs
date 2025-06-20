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
        //ボタンのスケールを1.2倍に拡大
        transform.localScale = OriginalScale * 1.2f;
    }

    public void OnPointerExit(PointerEventData EventData)
    {
        transform.localScale = OriginalScale;
    }

    public void OnSelect(BaseEventData EventData)
    {
        transform.localScale = OriginalScale * 1.2f;
    }

    public void OnDeselect(BaseEventData EventData)
    {
        transform.localScale = OriginalScale;
    }
}
