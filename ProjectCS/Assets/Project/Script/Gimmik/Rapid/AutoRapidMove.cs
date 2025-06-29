using UnityEngine;
using System.Collections;

public class AutoRapidMove : MonoBehaviour
{
    public Transform[] MovePoints;
    public float Speed = 10.0f;
    private bool IsMoving = false;
    private int CurrentPointIndex = 0;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        foreach(Transform Point in MovePoints)
        {
            Collider PointCollider = Point.GetComponent<Collider>();
            Collider PlayerCollider = GetComponent<Collider>();

            if(PointCollider != null && PlayerCollider != null)
            {
                Physics.IgnoreCollision(PlayerCollider, PointCollider);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StartGate") && gameObject.CompareTag("Player") && !IsMoving)
        {
            CurrentPointIndex = 0; // ポイントをリセット
            IsMoving = true;
            StartCoroutine(MoveThroughPoints());
        }
    }

    private IEnumerator MoveThroughPoints()
    {
        MovePlayer PlayerController = GetComponent<MovePlayer>();
        if (PlayerController != null)
        {
            PlayerController.enabled = false;
        }

        if(rb != null)
        {
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        while (CurrentPointIndex < MovePoints.Length)
        {
            Transform Target = MovePoints[CurrentPointIndex];

            while (Vector3.Distance(transform.position, Target.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    Target.position,
                    Speed * Time.deltaTime
                );
                yield return null;
            }

            CurrentPointIndex++;
            yield return new WaitForSeconds(0.1f);
        }

        if(rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        if (PlayerController != null)
        {
            PlayerController.enabled = true;
        }

        IsMoving = false;
    }
}
