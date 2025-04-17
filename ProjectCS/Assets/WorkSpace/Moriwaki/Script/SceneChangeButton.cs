//======================================================
// SceneChangeButton
// �쐬�ҁF�X�e
// �ŏI�X�V���F4/17
//
// [Log]4/16 �X�e�@�V�[���`�F���W�̃{�^���X�N���v�g�쐬
//======================================================

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeButton : MonoBehaviour
{
    // �J�ڐ�̃V�[�����iInspector�Őݒ�\�j
    [SerializeField] private string sceneName;

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}