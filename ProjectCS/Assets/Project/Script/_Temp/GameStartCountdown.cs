//====================================================
// �X�N���v�g���FGameStartCountdown
// �쐬�ҁF���{
//
// [Log]
// 05/07 ���{�@�J�E���g�_�E����������
// 05/29�@�{�с@�|�[�Y��ʕ\���{�^���̒�~
// 06/06�@�X�e�@�J�E���g���̑ҋ@Animation����
// 06/27�@�r��@�`���[�g���A���p�̏�����ǉ�
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
    public PlayerInput PauseInput;

    [Header("�A�j���[�V��������p")]
    [SerializeField] private PlayerAnimationController playerAnimController;

    [Header("�J�E���g�_�E�����ɍĐ�����A�j���[�V�����g���K�[��")]
    [SerializeField] private string countdownAnimTrigger = "CountdownIdle";

    [Header("�`���[�g���A���p�i�`���[�g���A���ȊO�ł͊��蓖��NG�j")]
    [SerializeField] private TutorialDisplayTexts TutorialDisplayTexts;

    private bool isCountingDown = false;
    public bool IsCountingDown => isCountingDown;

    private void Start()
    {
        StartCoroutine(CountdownCoroutine());

        // �J�E���g�_�E�����ɓ���̃A�j���[�V�������Đ�
        if (playerAnimController != null)
        {
            playerAnimController.PlaySpecificAnimation(countdownAnimTrigger);
        }
    }

    private IEnumerator CountdownCoroutine()
    {
        isCountingDown = true;  // �J�E���g�_�E���J�n

        Time.timeScale = 0f;
        countdownCanvas.gameObject.SetActive(true);

        yield return StartCoroutine(ShowCount("3"));
        yield return StartCoroutine(ShowCount("2"));
        yield return StartCoroutine(ShowCount("1"));
        yield return StartCoroutine(ShowCount("GO!!", goDisplayTime));

        countdownCanvas.gameObject.SetActive(false);

        Time.timeScale = 1f;

        // model��\�� or �ʏ��Ԃɖ߂�
        if (playerAnimController != null)
        {
            playerAnimController.SetUseNormalModel(false);  // rotationModel �ɖ߂��Ȃ�
        }

        // Snack�̑ł��グ���s
        isCountingDown = false; // �J�E���g�_�E���I��

        if (snackLauncher != null)
        {
            snackLauncher.Launch();
        }

        if(TutorialDisplayTexts != null)
        {
            TutorialDisplayTexts.DisplayTutorialUI1();
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