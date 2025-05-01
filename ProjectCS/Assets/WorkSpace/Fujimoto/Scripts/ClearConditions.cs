//======================================================
// ClearConditions�X�N���v�g
// �쐬�ҁF���{
// �ŏI�X�V���F4/29
// 
// [Log]
// 04/29 ���{�@�N���A�����̒ǉ�
// 05/01 �|���@UI�ƘA�g���ă��t�e�B���O�񐔂��Q�Ƃł���悤�ɏC��
//======================================================
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClearConditions : MonoBehaviour
{
    [Header("�N���A����")]
    public int clearCondition = 3; // �N���A�����̃��t�e�B���O��

    [Header("�J�ڐ�V�[��")]
    public string nextSceneName; // �C���X�y�N�^�[�Őݒ�\�ȑJ�ڐ�V�[����

    [Header("UI")]
    public Text liftingCounterText; // Text��ݒ�


    private int currentCount; // ���݂̃��t�e�B���O�񐔁i�O������󂯎��j

    // �J�n������
    private void Start()
    {
        // �N���A�񐔂�ݒ�
        currentCount = clearCondition;
        UpdateLiftingCounterUI();
    }

    // �O�����烊�t�e�B���O�񐔂����炷���\�b�h
    public void CheckLiftingCount()
    {
        currentCount--;
        Debug.Log("Current Lifting Count: " + currentCount);

        UpdateLiftingCounterUI();

        // �N���A�����𖞂����Ă��邩�`�F�b�N
        if (currentCount <= 0)
        {
            Debug.Log("ClearConditions�X�N���v�g�F�����𖞂��������߃V�[���J�ڂ��s���܂�");
            TriggerSceneTransition();
        }
    }

    // ���t�e�B���O�񐔂̍X�V
    private void UpdateLiftingCounterUI()
    {
        if (liftingCounterText != null)
        {
            liftingCounterText.text = "Lift Count: " + currentCount.ToString();
        }
        else
        {
            Debug.LogWarning("liftingCounterText ���ݒ肳��Ă��܂���I");
        }
    }

    // �V�[���J�ڂ����s
    private void TriggerSceneTransition()
    {
        if (!string.IsNullOrEmpty(nextSceneName)) // �V�[�������ݒ肳��Ă���ꍇ�̂ݎ��s
        {
            Debug.Log("Loading Scene: " + nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("���̃V�[�������ݒ肳��Ă��܂���I");
        }
    }
}
