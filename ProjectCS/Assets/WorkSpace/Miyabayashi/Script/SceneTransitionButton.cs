//======================================================
// SceneTransitionButton�X�N���v�g
// �쐬�ҁF�{��
// �ŏI�X�V���F4/22
// 
// [Log]4/22 �{�с@�{�^�����������Ƃ��ɃV�[���J�ڂ���
//
//======================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionButton : MonoBehaviour
{
    // �C���X�y�N�^�r���[����ݒ肷��V�[����
    public string sceneName;

    
    //�V�[���J��
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }



}
