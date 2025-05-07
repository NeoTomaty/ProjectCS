//====================================================
// スクリプト名：FadeController
// 作成者：森脇
// 内容：Fadeコントロール
// 最終更新日：05/04
//
// [Log]
// 05/04 森脇 スクリプト作成
//====================================================

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class FadeController : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1.0f;

    private void Awake()
    {
        // 最初は透明にしておく
        if (fadeImage != null)
            fadeImage.color = new Color(0, 0, 0, 0);
    }

    public void FadeOut(Action onComplete = null)
    {
        StartCoroutine(Fade(0f, 1f, onComplete));
    }

    public void FadeIn(Action onComplete = null)
    {
        StartCoroutine(Fade(1f, 0f, onComplete));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, Action onComplete)
    {
        float timer = 0f;
        Color color = fadeImage.color;

        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            timer += Time.deltaTime;
            yield return null;
        }

        // 最終値を明示的にセット
        fadeImage.color = new Color(color.r, color.g, color.b, endAlpha);

        onComplete?.Invoke();
    }
}