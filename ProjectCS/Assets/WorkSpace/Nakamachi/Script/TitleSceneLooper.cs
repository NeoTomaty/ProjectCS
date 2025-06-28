//TitleSceneLooper.cs
//�쐬��:��������
//�ŏI�X�V��:2025/06/24
//�A�^�b�`:�e�^�C�g���V�[���ɃA�^�b�`
//[Log]
//06/24�@�����@�^�C�g���V�[���̐؂�ւ�����

using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneLooper : MonoBehaviour
{
    public float Delay = 10.0f;             // �V�[����؂�ւ���܂ł̑҂�����
    public string[] SceneNames;            // �^�C�g���V�[���̖��O�z��

    [SerializeField]
    private GameObject optionUI;           // �I�v�V����UI�i�A�N�e�B�u��Ԃ�����j

    private void Start()
    {
        // �w��b���LoadNextScene�����s
        Invoke("LoadNextScene", Delay);
    }

    private void LoadNextScene()
    {
        // �I�v�V�������J���Ă���ꍇ�̓V�[���J�ڂ��Ȃ��i�ăX�P�W���[���j
        if (optionUI != null && optionUI.activeSelf)
        {
            // �I�v�V���������܂ōă`�F�b�N��ҋ@�i��Invoke�j
            Invoke("LoadNextScene", 1.0f); // 1�b��ɂ�����x�m�F
            return;
        }

        // ���݂̃V�[�������擾
        string currentScene = SceneManager.GetActiveScene().name;

        // ���݂̃V�[�����z��̉��Ԗڂ�
        int currentIndex = System.Array.IndexOf(SceneNames, currentScene);

        // ���̃C���f�b�N�X���v�Z�i�Ō�Ȃ�ŏ��ɖ߂�j
        int nextIndex = (currentIndex + 1) % SceneNames.Length;

        // ���̃V�[����ǂݍ���
        SceneManager.LoadScene(SceneNames[nextIndex]);
    }
}