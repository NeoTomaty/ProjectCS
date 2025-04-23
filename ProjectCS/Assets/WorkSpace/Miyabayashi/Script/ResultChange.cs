//======================================================
// resultChangeスクリプト
// 作成者：宮林
// 最終更新日：4/22
// 
// [Log]4/22 宮林　リザルト画面でAボタン（Enter）を押したときに
//                 ボタンが表示される
//
//======================================================

using UnityEngine;

public class ResultChange : MonoBehaviour
{
    [Header("メニューUIルート")]
    public GameObject menuOverlay;

    private bool menuOpen = false;

    void Update()
    {
        // Aボタン or エンターキーで開く
        if (!menuOpen && (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Return)))
        {
            OpenMenu();
        }
    }


    void OpenMenu()
    {
        menuOverlay.SetActive(true);
        menuOpen = true;
    }
}
