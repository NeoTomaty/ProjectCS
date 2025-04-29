//======================================================
// GameOverConditions�X�N���v�g
// �쐬�ҁF�X�e
// �ŏI�X�V���F4/15
//
// [Log]4/15 �X�e�@�Q�[���I�[�o�[�����̎���
//======================================================
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverConditions : MonoBehaviour
{
    [Header("�Q�[���I�[�o�[����ɕK�v�ȃ��t�e�B���O��")]
    public int gameOverThreshold = 5; // ���̉񐔈ȏ�ŗ��Ƃ��ƃQ�[���I�[�o�[

    [Header("�Q�[���I�[�o�[���̑J�ڐ�V�[��")]
    public string gameOverSceneName;

    private int currentCount;

    // ���t�e�B���O�񐔂��O������ݒ�
    public void SetLiftingCount(int count)
    {
        currentCount = count;
        Debug.Log("Current Lifting Count: " + currentCount);
    }

    // �n�ʂƂ̏Փ˂����o�i�n�ʂɂԂ�������Ăяo���j
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Ground hit detected.");

            if (currentCount >= gameOverThreshold)
            {
                TriggerGameOver();
            }
            else
            {
                Debug.Log("�Z�[�t�F���t�e�B���O�񐔂����Ȃ����߃Q�[���I�[�o�[�ɂ͂Ȃ�܂���B");
            }
        }
    }

    // �Q�[���I�[�o�[����
    private void TriggerGameOver()
    {
        if (!string.IsNullOrEmpty(gameOverSceneName))
        {
            Debug.Log("Game Over! Loading scene: " + gameOverSceneName);
            SceneManager.LoadScene(gameOverSceneName);
        }
        else
        {
            Debug.LogError("�Q�[���I�[�o�[�V�[�����ݒ肳��Ă��܂���I");
        }
    }
}