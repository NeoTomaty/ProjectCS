//OptionButton.cs
//�쐬��:��������
//�ŏI�X�V��:2025/05/07
//�A�^�b�`:OptionButton�ɃA�^�b�`
//[Log]
//05/07�@�����@OptionButton���N���b�N������I�v�V�����V�[���ɑJ�ڂ��鏈��

using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionButton : MonoBehaviour
{
    //�V�[���̖��O��ݒ肷��ϐ�
    public string SceneName;

    //�I�v�V�����{�^�����N���b�N���ꂽ�Ƃ��̊֐�
    public void OnOptionButtonClicked()
    {
        Debug.Log("�I�v�V�����{�^�����N���b�N����");

        //�V�[�������[�h
        SceneManager.LoadScene(SceneName);
    }
}