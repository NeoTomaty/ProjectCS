//======================================================
// SceneTransition�X�N���v�g
// �쐬�ҁF�{��
// �ŏI�X�V���F4/25
// 
// [Log]4/22 �{�с@�R���g���[���[�̃{�^�����������Ƃ��ɑJ�ڂ���
//      4/25 �{�с@�t�F�[�h�����Ή�
//======================================================
using UnityEngine;
using UnityEngine.InputSystem;

public class SceneTransition : MonoBehaviour
{
    public string sceneName; // �J�ڐ�̃V�[����

    private bool isTransitioning = false;

    [SerializeField] private TitleOptionManager OptionManager;

    private void Start()
    {
        OptionManager = FindFirstObjectByType<TitleOptionManager>();

        if(OptionManager == null)
        {
            Debug.LogWarning("TitleOptionManager ��������܂���ł����B");
        }
    }

    private void Update()
    {
        //if (isTransitioning) return;

        //if (OptionManager != null && OptionManager.IsOpen())
        //{
        //    return;
        //}

        //// Xbox �R���g���[���[�� A �{�^��
        //if (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame)
        //{
        //    StartSceneTransition();
        //}

        //// �L�[�{�[�h�� Enter �L�[
        //if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
        //{
        //    StartSceneTransition();
        //}
    }

    public  void StartSceneTransition()
    {
        isTransitioning = true;

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
