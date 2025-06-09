using UnityEngine;

public class BezierMover : MonoBehaviour
{
    public CubicBezierCurve curve;  // �Ȑ��X�N���v�g�i�x�W�F���j
    [Range(0.1f, 10f)]
    public float moveDuration = 3f; // �Ȑ����ړ����I����܂ł̕b��

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

        // �i�s�����x�N�g��
        Vector3 delta = currentPosition - previousPosition;
        float distance = delta.magnitude;

        // ���f���̌����ڕ�����i�s�����ɍ��킹��iY������ɂ��Č�����ς���j
        if (delta != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(delta.normalized, Vector3.up);
            RotatingModel.rotation = lookRotation;
        }

        // �]����悤�ȉ�]�i�i�s������Y���̊O�ςŉ�]�����쐬�j
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
