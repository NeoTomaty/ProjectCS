//======================================================
// [SlowMotionController]
// 作成者：荒井修
// 最終更新日：04/26
// 
// [Log]
// 04/26　荒井　
//======================================================
using UnityEngine;

public class SlowMotionController : MonoBehaviour
{
    // スローモーションの度合い
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
