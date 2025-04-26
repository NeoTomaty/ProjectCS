using UnityEngine;

// テスト用の移動スクリプト
public class TestMove : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 5f;

    private Vector3 MoveVector;

    public float GetMoveSpeed()
    {
        return MoveSpeed;
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        MoveSpeed = moveSpeed;
    }

    public Vector3 GetMoveVector()
    {
        return MoveVector;
    }

    public void SetMoveVector(Vector3 moveVector)
    {
        MoveVector = moveVector;
        MoveVector.Normalize(); // 正規化して方向ベクトルを設定
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveVector = Vector3.zero; // 初期化
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(MoveVector * MoveSpeed * Time.deltaTime);
    }
}
