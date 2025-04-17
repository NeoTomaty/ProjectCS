using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

//======================================================
// [GoalTrigger]
// 作成者：藤本
// 最終更新日：04/17
// 
// ゴールオブジェクトに触れてからシーン遷移するまでの間お菓子をフォーカスする処理が未完成
//
// [Log]
// 04/17　藤本　UIを数秒表示後、シーン遷移
// 
//======================================================
public class GoalTrigger : MonoBehaviour
{
    // 遷移先シーン名
    [Header("遷移先シーン")]
    public string nextSceneName;

    // クリア時のUIテキスト
    [Header("UIクリアテキスト")]
    public TextMeshProUGUI clearTextObject;

    // 遷移までの待機時間
    [Header("遷移待機時間")]
    public float delaySecond;

    // クリア時に注視するオブジェクト
    [Header("注目する対象")]
    public Transform focusTarget;

    [Header("カメラが寄る距離（targetからの距離）")]
    public float focusDistance = 5.0f;

    [Header("カメラの移動速度")]
    public float cameraMoveSpeed = 3.0f;

    private Camera mainCamera;
    private bool isFocusing = false;

    // UIを非アクティブ化
    private void Start()
    {
        mainCamera = Camera.main;

        // ゲーム開始時に一度だけ、UIを非表示に
        if (clearTextObject != null)
        {
            clearTextObject.gameObject.SetActive(false);
            Debug.Log($"Start() -> clearTextObject [{clearTextObject.name}] を非表示にしました");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 接触相手が "GoalWall" タグかどうかを判定
        if (other.CompareTag("GoalWall"))
        {
            Debug.Log("GoalWall に当たった");
            // シーン遷移実行
            // クリアUIを表示してから遷移
            StartCoroutine(ShowClearAndLoad());
        }
    }

    // UI表示
    private IEnumerator ShowClearAndLoad()
    {
        // UI表示
        if (clearTextObject != null)    
            clearTextObject.gameObject.SetActive(true);
        else
            Debug.LogWarning("clearTextObject が設定されていません！");

        // カメラフォーカス開始
        isFocusing = true;

        // 数秒待機
        yield return new WaitForSeconds(delaySecond);

        // シーン遷移
        TriggerSceneTransition();
    }

    private void Update()
    {
        if (isFocusing && focusTarget != null && mainCamera != null)
        {
            Debug.Log("clearTextObject フォーカス開始");
            // 注目ポジション：ターゲットの少し後方（カメラが見る位置）
            //Vector3 focusPos = focusTarget.position - focusTarget.forward * focusDistance + Vector3.up * 2.0f;

            // カメラを滑らかに移動
            //mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, focusPos, Time.deltaTime * cameraMoveSpeed);

            // カメラをターゲットに向ける
            mainCamera.transform.LookAt(focusTarget);
        }
    }

    // シーン遷移実行関数
    private void TriggerSceneTransition()
    {
        if (!string.IsNullOrEmpty(nextSceneName)) // シーン名が設定されている場合のみ実行
        {
            Debug.Log("Loading Scene: " + nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("次のシーン名が設定されていません！");
        }
    }
}
