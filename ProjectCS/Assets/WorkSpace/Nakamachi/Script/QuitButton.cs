//QuitButton.cs
//�쐬��:��������
//�ŏI�X�V��:2025/05/06
//�A�^�b�`:QuitButton�ɃA�^�b�`
//[Log]
//05/06�@�����@QuitButton���N���b�N������Q�[�����I�����鏈��

using UnityEngine;

public class QuitButton : MonoBehaviour
{
    //�I���{�^�����N���b�N���ꂽ�Ƃ��̊֐�
    public void OnQuitButtonClicked()
    {
        Debug.Log("�I���{�^�����N���b�N���ꂽ");

        //Unity�G�f�B�^�Ŏ��s�����ǂ������m�F
        #if UNITY_EDITOR

        //Unity�G�f�B�^�Ŏ��s���̂Ƃ��A�v���C���[�h���I��
        UnityEditor.EditorApplication.isPlaying = false;
        #else

        //�r���h���ꂽ�A�v���P�[�V�����̂Ƃ��A�A�v���P�[�V�����I��
        Application.Quit();
        #endif
    }
}