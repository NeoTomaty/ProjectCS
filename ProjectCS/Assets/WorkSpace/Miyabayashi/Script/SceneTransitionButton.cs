//======================================================
// SceneTransitionButton�X�N���v�g
// �쐬�ҁF�{��
// �ŏI�X�V���F4/25
// 
// [Log]4/22 �{�с@�{�^�����������Ƃ��ɃV�[���J�ڂ���
//�@�@�@4/25 �{�с@�t�F�[�h�����Ή�
//======================================================
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionButton : MonoBehaviour
{
    // �C���X�y�N�^�r���[����ݒ肷��V�[����
    public string sceneName;
 
    //FadeManager fade = Object.FindFirstObjectByType<FadeManager>();

    //// �V�[���J��
    //public void LoadScene()
    //{
    //    if (fade != null)
    //    {
    //        StartCoroutine(fade.FadeAndLoadScene(sceneName)); // Fade�������J�n���A�V�[���J��
    //    }
    //}



   public void StartSceneTransition()
    {
       

        FadeManager fade = Object.FindFirstObjectByType<FadeManager>();
        if (fade != null)
        {
            fade.FadeToScene(sceneName);
        }
        else
        {
            Debug.LogWarning("FadeManager ��������܂���ł����BCanvas��FadeManager��t�������m�F���ĂˁI");
        }
    }
}
