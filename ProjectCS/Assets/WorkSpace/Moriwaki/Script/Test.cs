using UnityEngine;

public class Test : MonoBehaviour
{
    // [SerializeField] public LiftingJump liftingJump;

    public void OnKickImpact()
    {
        Debug.Log("キックが当たったタイミングで呼び出された！");
        // 一時停止（デバッグ用）
        //Time.timeScale = 0f;
        // liftingJump.ForceFinishLiftingJump();
    }
}