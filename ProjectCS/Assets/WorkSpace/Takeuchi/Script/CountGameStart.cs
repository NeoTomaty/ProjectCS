using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountGameStart : MonoBehaviour
{
    public Text countdownText;
    public float countdownSpeed = 1f; // �b�Ԋu

    void Start()
    {
        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        Time.timeScale = 0f; // �Q�[����~

        string[] countdownStrings = { "3", "2", "1", "GO!" };

        foreach (string count in countdownStrings)
        {
            countdownText.text = count;
            yield return WaitForUnscaledSeconds(countdownSpeed);
        }

        countdownText.text = "";
        Time.timeScale = 1f; // �Q�[���ĊJ
    }

    // Unscaled�ȑҋ@�p���\�b�h
    private IEnumerator WaitForUnscaledSeconds(float seconds)
    {
        float elapsed = 0f;
        while (elapsed < seconds)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
    }
}
