//ReturnTitleSceneButton.cs
//�쐬��:��������
//�ŏI�X�V��:2025/05/07
//�A�^�b�`:ReturnTitleSceneButton�ɃA�^�b�`
//[Log]
//05/07�@�����@ReturnTitleSceneButton���N���b�N������I�v�V�����V�[���ɑJ�ڂ��鏈��

using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnTitleSceneButton : MonoBehaviour
{
    //�V�[���̖��O��ݒ肷��ϐ�
    public string SceneName;

    //���^�[���{�^�����N���b�N���ꂽ�Ƃ��̊֐�
    public void OnReturnTitleSceneButtonClicked()
    {
        Debug.Log("���^�[���{�^�����N���b�N����");

        //�V�[�������[�h
        SceneManager.LoadScene(SceneName);
    }
}