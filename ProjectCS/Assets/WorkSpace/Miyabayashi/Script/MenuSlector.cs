//======================================================
// MenuSlector スクリプト
// 作成者：宮林
// 最終更新日：4/23
// 
// [Log]4/23 宮林　最初に選ばれているボタンを割り当てる
//                 
//======================================================
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSelector : MonoBehaviour
{
    public GameObject firstSelected;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
}
