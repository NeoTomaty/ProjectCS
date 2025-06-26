//TitleSceneLooper.cs
//�쐬��:��������
//�ŏI�X�V��:2025/06/24
//�A�^�b�`:�e�^�C�g���V�[���ɃA�^�b�`
//[Log]
//06/24�@�����@�^�C�g���V�[���̐؂�ւ�����

using UnityEngine;
using UnityEngine.SceneManagement;

//�^�C�g���V�[������莞�Ԃ��Ƃɐ؂�ւ��ă��[�v������X�N���v�g
public class TitleSceneLooper : MonoBehaviour
{
    //�V�[����؂�ւ���܂ł̑҂�����(�b)
    public float Delay = 10.0f;

    //�^�C�g���V�[���̖��O�����ԂɊi�[����z��
    public string[] SceneNames;

    //�Q�[���J�n���ɌĂ΂��֐�
    private void Start()
    {
        //�w�肵���b�����LoadNextScene�֐����Ăяo��
        Invoke("LoadNextScene", Delay);
    }

    //���̃V�[����ǂݍ��ޏ���
    void LoadNextScene()
    {
        //���݃A�N�e�B�u�ȃV�[���̖��O���擾
        string CurrentScene = SceneManager.GetActiveScene().name;

        //���݂̃V�[����SceneNames�z��̉��Ԗڂ����擾
        int CurrentIndex = System.Array.IndexOf(SceneNames, CurrentScene);

        //���ɓǂݍ��݃V�[���̃C���f�b�N�X���v�Z(�Ō�̃V�[���̎��͍ŏ��ɖ߂�)
        int NextIndex = (CurrentIndex + 1) % SceneNames.Length;

        //���̃V�[����ǂݍ���
        SceneManager.LoadScene(SceneNames[NextIndex]);
    }
}