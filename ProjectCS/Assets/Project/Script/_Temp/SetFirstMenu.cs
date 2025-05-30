//======================================================
// スクリプト名：SetFirstMenu
// 作成者：宮林
// 最終更新日：4/23
// 
// [Log]
// 4/23 宮林　最初に選ばれているボタンを割り当てる
// 4/28 竹内　スクリプト名変更
// 
//======================================================
using UnityEngine;
using UnityEngine.EventSystems;

public class SetFirstMenu : MonoBehaviour
{
    public GameObject firstSelected;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
}
