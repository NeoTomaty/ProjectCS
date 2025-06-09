using UnityEngine;
using System.Collections;

public class BidirectionalRapidMove : MonoBehaviour
{
    public Transform[] ForwardPoints;
    public Transform[] BackwardPoints;
    public float Speed = 10.0f;

    private bool IsMoving = false;
    private Rigidbody rb;
    private MovePlayer PlayerController;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        PlayerController = GetComponent<MovePlayer>();

        IgnoreCollisions(ForwardPoints);
        IgnoreCollisions(BackwardPoints);
    }

    private void IgnoreCollisions(Transform[] Points)
    {
        foreach (Transform Point in Points)
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
        if(IsMoving || !gameObject.CompareTag("Player"))
        {
            return;
        }

        if(other.CompareTag("StartGate"))
        {
            StartCoroutine(MoveThroughPoints(ForwardPoints));
        }
        else if(other.CompareTag("EndGate"))
        {
            StartCoroutine(MoveThroughPoints(BackwardPoints));
        }
    }

    private IEnumerator MoveThroughPoints(Transform[] Points)
    {
        IsMoving = true;

        if(PlayerController != null)
        {
            PlayerController.enabled = false;
        }

        if(rb != null)
        {
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }


        foreach (Transform Target in Points)
        {
            while (Vector3.Distance(transform.position, Target.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(
                transform.position,
                Target.position,
                Speed * Time.deltaTime
                );
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
        }

        if(rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        if(PlayerController != null)
        {
            PlayerController.enabled = true;
        }

        yield return new WaitForSeconds(3.0f);

        IsMoving = false;
    }
}