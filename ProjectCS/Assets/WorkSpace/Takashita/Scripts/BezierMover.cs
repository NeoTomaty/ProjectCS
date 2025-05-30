using UnityEngine;

public class BezierMover : MonoBehaviour
{
    public CubicBezierCurve curve;  // 曲線スクリプト（ベジェ情報）
    [Range(0.1f, 10f)]
    public float moveDuration = 3f; // 曲線を移動し終えるまでの秒数

    private bool IsReverse = false;

    private float timer = 0f;

    private bool isMoving = false;

    private void Start()
    {
        //timer = moveDuration;
        
    }

    void Update()
    {
        if (!isMoving || curve == null) return;

        timer += IsReverse ? -Time.deltaTime : Time.deltaTime;

        timer = Mathf.Clamp(timer, 0f, moveDuration);

        if ((!IsReverse && timer >= moveDuration) || (IsReverse && timer <= 0f))
        {
            isMoving = false; // 停止フラグ
            return;
        }

        float t = Mathf.Clamp01(timer / moveDuration);
        transform.position = curve.CalculateBezierPoint(t);
    }

    public void StartMove(bool isReverse, CubicBezierCurve cubicBezierCurve)
    {
        IsReverse = isReverse;
        curve = cubicBezierCurve;
        isMoving = true;

        Debug.Log("timer = " + timer);
        if (timer == 0f || timer == moveDuration)
        {
            timer = isReverse ? 1f : 0f;
            Debug.Log("処理通過");
        }
        
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public bool GetIsMoving()
    {
        return isMoving;
    }
}
