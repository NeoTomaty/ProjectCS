using UnityEngine;

public class BezierMover : MonoBehaviour
{
    public CubicBezierCurve curve;  // 曲線スクリプト（ベジェ情報）
    [Range(0.1f, 10f)]
    public float moveDuration = 3f; // 曲線を移動し終えるまでの秒数

    private bool IsReverse = false;

    private float timer = 0f;

    private bool isMoving = false;

    [SerializeField] private Transform RotatingModel;
    [SerializeField] private float radius = 0.01f;

    private Vector3 previousPosition;

    void Update()
    {
        if (!isMoving || curve == null) return;

        timer += IsReverse ? -Time.deltaTime : Time.deltaTime;

        float t = Mathf.Clamp01(timer / moveDuration);
        Vector3 currentPosition = curve.CalculateBezierPoint(t);
        transform.position = currentPosition;

        // 進行方向ベクトル
        Vector3 delta = currentPosition - previousPosition;
        float distance = delta.magnitude;

        // モデルの見た目方向を進行方向に合わせる（Y軸を上にして向きを変える）
        if (delta != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(delta.normalized, Vector3.up);
            RotatingModel.rotation = lookRotation;
        }

        // 転がるような回転（進行方向とY軸の外積で回転軸を作成）
        if (distance > 0f && radius > 0f)
        {
            Vector3 rotationAxis = Vector3.Cross(delta.normalized, Vector3.up);
            float angleDeg = Mathf.Rad2Deg * (distance / radius);
            RotatingModel.Rotate(rotationAxis, angleDeg, Space.World);
        }

        previousPosition = currentPosition;

        if ((!IsReverse && timer >= moveDuration) || (IsReverse && timer <= 0f))
        {
            isMoving = false;
        }
    }

    public void StartMove(bool isReverse, CubicBezierCurve cubicBezierCurve)
    {
        IsReverse = isReverse;
        curve = cubicBezierCurve;
        isMoving = true;

        
        if (timer <= 0f || timer >= moveDuration)
        {
            timer = isReverse ? moveDuration : 0f;
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
