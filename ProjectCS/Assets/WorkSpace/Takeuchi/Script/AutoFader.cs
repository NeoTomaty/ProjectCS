using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AutoFader : MonoBehaviour
{
    [Header("フェード設定")]
    public Image fadeImage;
    public float fadeDuration = 1.0f;
    public float waitTime = 3.0f;

    [Header("表示オブジェクト")]
    public List<GameObject> objectsToToggle;

    private int currentIndex = 0;

    void Start()
    {
        // 全て非表示にして、最初のオブジェクトのみ表示
        for (int i = 0; i < objectsToToggle.Count; i++)
        {
            objectsToToggle[i].SetActive(i == 0);
        }

        // 初期状態はフェードイン済み（透明）
        SetAlpha(0f);

        // 自動フェード処理開始
        StartCoroutine(FadeLoop());
    }

    IEnumerator FadeLoop()
    {
        while (true)
        {
            // 待機
            yield return new WaitForSeconds(waitTime);

            // フェードアウト
            yield return StartCoroutine(FadeOut());

            // オブジェクト切り替え
            SwitchObject();

            // フェードイン
            yield return StartCoroutine(FadeIn());
        }
    }

    void SwitchObject()
    {
        // 現在のオブジェクト非表示
        objectsToToggle[currentIndex].SetActive(false);

        // 次のインデックスに移動（ループ処理）
        currentIndex = (currentIndex + 1) % objectsToToggle.Count;

        // 次のオブジェクトを表示
        objectsToToggle[currentIndex].SetActive(true);
    }

    IEnumerator FadeOut()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            SetAlpha(Mathf.Clamp01(elapsed / fadeDuration));
            yield return null;
        }
        SetAlpha(1f);
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            SetAlpha(1f - Mathf.Clamp01(elapsed / fadeDuration));
            yield return null;
        }
        SetAlpha(0f);
    }

    void SetAlpha(float alpha)
    {
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = alpha;
            fadeImage.color = c;
        }
    }
}
