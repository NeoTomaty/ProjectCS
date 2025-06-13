using UnityEngine;

public class AnimationFinishTrigger : MonoBehaviour
{
    [SerializeField] private GameObject snackObject;  // ← snackObject を定義

    // [追加] カメラ連携のための設定
    [Header("カメラ連携")]
    [Tooltip("カメラ制御スクリプト")]
    [SerializeField] private CameraFunction cameraFunction;

    [Tooltip("特別視点時のプレイヤーからの相対的なカメラ位置")]
    [SerializeField] private Vector3 specialViewPosition = new Vector3(0, 2, -4);

    public void OnKickImpact()
    {
        Debug.Log("キックが当たったタイミングで呼び出された");
        // snackのスクリプトを参照し、ヒットストップを解除
        snackObject.GetComponent<BlownAway_Ver3>().EndHitStop();
        if (cameraFunction != null)
        {
            cameraFunction.StopSpecialView();
        }
    }
}