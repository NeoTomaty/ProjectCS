//StartButton.cs
//�쐬��:��������
//�ŏI�X�V��:2025/05/06
//�A�^�b�`:StartButton�ɃA�^�b�`
//[Log]
//05/06�@�����@StartButton���N���b�N������X�e�[�W�Z���N�g�V�[���ɑJ�ڂ��鏈��

using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    //�V�[���̖��O��ݒ肷��ϐ�
    public string SceneName;

    //�X�^�[�g�{�^�����N���b�N���ꂽ�Ƃ��̊֐�
    public void OnStartButtonClicked()
    {
        Debug.Log("�X�^�[�g�{�^�����N���b�N����");

        //�V�[�������[�h
        SceneManager.LoadScene(SceneName);
    }
}