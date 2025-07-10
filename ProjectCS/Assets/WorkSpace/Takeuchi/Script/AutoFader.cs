using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AutoFader : MonoBehaviour
{
    [Header("�t�F�[�h�ݒ�")]
    public Image fadeImage;
    public float fadeDuration = 1.0f;
    public float waitTime = 3.0f;

    [Header("�\���I�u�W�F�N�g")]
    public List<GameObject> objectsToToggle;

    private int currentIndex = 0;

    void Start()
    {
        // �S�Ĕ�\���ɂ��āA�ŏ��̃I�u�W�F�N�g�̂ݕ\��
        for (int i = 0; i < objectsToToggle.Count; i++)
        {
            objectsToToggle[i].SetActive(i == 0);
        }

        // ������Ԃ̓t�F�[�h�C���ς݁i�����j
        SetAlpha(0f);

        // �����t�F�[�h�����J�n
        StartCoroutine(FadeLoop());
    }

    IEnumerator FadeLoop()
    {
        while (true)
        {
            // �ҋ@
            yield return new WaitForSeconds(waitTime);

            // �t�F�[�h�A�E�g
            yield return StartCoroutine(FadeOut());

            // �I�u�W�F�N�g�؂�ւ�
            SwitchObject();

            // �t�F�[�h�C��
            yield return StartCoroutine(FadeIn());
        }
    }

    void SwitchObject()
    {
        // ���݂̃I�u�W�F�N�g��\��
        objectsToToggle[currentIndex].SetActive(false);

        // ���̃C���f�b�N�X�Ɉړ��i���[�v�����j
        currentIndex = (currentIndex + 1) % objectsToToggle.Count;

        // ���̃I�u�W�F�N�g��\��
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
