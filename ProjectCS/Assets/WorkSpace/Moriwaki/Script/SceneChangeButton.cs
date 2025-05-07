//======================================================
// SceneChangeButton
// �쐬�ҁF�X�e
// �ŏI�X�V���F5/07
//
// [Log]4/16 �X�e�@�V�[���`�F���W�̃{�^���X�N���v�g�쐬
// [Log]5/07 �X�e�@�V�[���`�F���W�̃{�^���X�N���v�g�t�F�[�h�Ή�
//======================================================

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeButton : MonoBehaviour
{
    [SerializeField] private string sceneName;      // �J�ڐ�V�[����
    [SerializeField] private FadeController fadeController;  // FadeController���Q��

    public void ChangeScene()
    {
        // �t�F�[�h�A�E�g���J�n���A������ɃV�[�������[�h
        fadeController.FadeOut(() =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }
}