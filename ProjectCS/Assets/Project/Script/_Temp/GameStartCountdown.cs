//====================================================
// �X�N���v�g���FGameStartCountdown
// �쐬�ҁF���{
// 
// [Log]
// 05/07 ���{�@�J�E���g�_�E����������
// 05/29�@�{�с@�|�[�Y��ʕ\���{�^���̒�~
//====================================================

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;

public class GameStartCountdown : MonoBehaviour
{
    [Header("�J�E���g�_�E���p��Text")]
    [SerializeField] private Text countdownText;

    [Header("�J�n���ɕ\�������Canvas")]
    [SerializeField] private Canvas countdownCanvas;

    [Header("GO!!�\����ɔ�\���ɂ���܂ł̎���")]
    [SerializeField] private float goDisplayTime = 0.5f;

    [Header("Snack�ł��グ�Ώ�")]
    [SerializeField] private SnackLauncher snackLauncher;

    [Header("������~�߂�input")]
    public PlayerInput PauseInput;                              //�|�[�Y��ʂ̑���󂯎��

    void Start()
    {
        StartCoroutine(CountdownCoroutine());
        PauseInput.actions.Disable(); // ���͂𖳌��ɂ���
    }

    private IEnumerator CountdownCoroutine()
    {
        // ���Ԓ�~
        Time.timeScale = 0f;

        // �J�E���g�_�E��Canvas�\��
        countdownCanvas.gameObject.SetActive(true);

        // �\�����X�V���Ȃ���J�E���g
        yield return StartCoroutine(ShowCount("3"));
        yield return StartCoroutine(ShowCount("2"));
        yield return StartCoroutine(ShowCount("1"));
        yield return StartCoroutine(ShowCount("GO!!", goDisplayTime));

        // �J�E���g�_�E����\��
        countdownCanvas.gameObject.SetActive(false);
        PauseInput.actions.Enable(); // ���͂�L���ɂ���
        // ���ԍĊJ
        Time.timeScale = 1f;

        // Snack�̑ł��グ���s
        if (snackLauncher != null)
        {
            snackLauncher.Launch();
        }
    }

    private IEnumerator ShowCount(string message, float customDuration = 1f)
    {
        countdownText.text = message;
        yield return WaitForRealtimeSeconds(customDuration);
    }

    // Time.timeScale = 0 �ł��ҋ@�ł���֐�
    private IEnumerator WaitForRealtimeSeconds(float seconds)
    {
        float endTime = Time.realtimeSinceStartup + seconds;
        while (Time.realtimeSinceStartup < endTime)
        {
            yield return null;
        }
    }
}
