using UnityEngine;

public class TestMove : MonoBehaviour
{
    private Rigidbody Rb;
    public float constantSpeed = 1.0f;

    void Start()
    {
        Rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Rb.linearVelocity = Vector3.forward * constantSpeed;
    }
}
