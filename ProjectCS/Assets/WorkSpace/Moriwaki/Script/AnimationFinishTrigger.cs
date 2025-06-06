using UnityEngine;

public class AnimationFinishTrigger : MonoBehaviour
{
    [SerializeField] private GameObject snackObject;  // ← snackObject を定義

    public void OnKickImpact()
    {
        Debug.Log("キックが当たったタイミングで呼び出された");
        // snackのスクリプトを参照し、ヒットストップを解除
        snackObject.GetComponent<BlownAway_Ver3>().EndHitStop();
    }
}