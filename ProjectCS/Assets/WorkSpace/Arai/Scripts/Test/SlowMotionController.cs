//======================================================
// [SlowMotionController]
// �쐬�ҁF�r��C
// �ŏI�X�V���F04/26
// 
// [Log]
// 04/26�@�r��@
//======================================================
using UnityEngine;

public class SlowMotionController : MonoBehaviour
{
    // �X���[���[�V�����̓x����
    [SerializeField] private float SlowMotionFactor = 0.1f;

    public void StartSlowMotion()
    {
        Time.timeScale = SlowMotionFactor;
    }

    public void StopSlowMotion()
    {
        Time.timeScale = 1.0f;
    }
}
