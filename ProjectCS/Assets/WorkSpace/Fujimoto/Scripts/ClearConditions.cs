//======================================================
// ClearConditions�X�N���v�g
// �쐬�ҁF���{
// �ŏI�X�V���F4/29
// 
// [Log]4/29 ���{�@�N���A�����̒ǉ�
//======================================================

using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearConditions : MonoBehaviour
{
    [Header("�N���A����")]
    public int clearCondition = 10; // �N���A�����̃��t�e�B���O��

    [Header("�J�ڐ�V�[��")]
    public string nextSceneName; // �C���X�y�N�^�[�Őݒ�\�ȑJ�ڐ�V�[����

    private int currentCount; // ���݂̃��t�e�B���O�񐔁i�O������󂯎��j

    // �O�����烊�t�e�B���O�񐔂�ݒ肷�郁�\�b�h
    public void SetLiftingCount(int count)
    {
        currentCount = count;
        Debug.Log("Current Lifting Count: " + currentCount);

        // �N���A�����𖞂����Ă��邩�`�F�b�N
        if (currentCount >= clearCondition)
        {
            TriggerSceneTransition();
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
