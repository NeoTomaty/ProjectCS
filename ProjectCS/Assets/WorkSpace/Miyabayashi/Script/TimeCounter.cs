//======================================================
// TimeCounter�X�N���v�g
// �쐬�ҁF�{��
// �ŏI�X�V���F4/20
// 
// [Log]4/20 �{�с@�^�C�}�[�̒ǉ�
//      4/22 �{�с@�V�[���J�ڂ̒ǉ�      
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeCounter : MonoBehaviour
{
    public float countdown = 5.0f;
    public Text timeText;

    [Header("�J�ڐ�̃V�[����")]
    public string nextSceneName;

    private bool hasTransitioned = false;

    void Update()
    {
        countdown -= Time.deltaTime;

        if (countdown > 0)
        {
            timeText.text = countdown.ToString("f1") + "�b";
        }
        else
        {
            timeText.text = "0.00�b";

            if (!hasTransitioned)
            {
                hasTransitioned = true;

                if (!string.IsNullOrEmpty(nextSceneName))
                {
                    SceneManager.LoadScene(nextSceneName);
                }
                else
                {
                    Debug.LogWarning("���̃V�[�������ݒ肳��Ă��܂���I");
                }
            }
        }
    }
}
