//======================================================
// SceneTransitionスクリプト
// 作成者：宮林
// 最終更新日：4/25
// 
// [Log]4/22 宮林　コントローラーのボタンを押したときに遷移する
//      4/25 宮林　フェード処理対応
//======================================================
using UnityEngine;
using UnityEngine.InputSystem;

public class SceneTransition : MonoBehaviour
{
    public string sceneName; // 遷移先のシーン名

    private bool isTransitioning = false;

    [SerializeField] private TitleOptionManager OptionManager;

    private void Start()
    {
        OptionManager = FindFirstObjectByType<TitleOptionManager>();

        if(OptionManager == null)
        {
            Debug.LogWarning("TitleOptionManager が見つかりませんでした。");
        }
    }

    private void Update()
    {
        //if (isTransitioning) return;

        //if (OptionManager != null && OptionManager.IsOpen())
        //{
        //    return;
        //}

        //// Xbox コントローラーの A ボタン
        //if (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame)
        //{
        //    StartSceneTransition();
        //}

        //// キーボードの Enter キー
        //if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
        //{
        //    StartSceneTransition();
        //}
    }

    public  void StartSceneTransition()
    {
        isTransitioning = true;

        FadeManager fade = Object.FindFirstObjectByType<FadeManager>();
        if (fade != null)
        {
            fade.FadeToScene(sceneName);
        }
        else
        {
            Debug.LogWarning("FadeManager が見つかりませんでした。CanvasにFadeManagerを付けたか確認してね！");
        }
    }
}
