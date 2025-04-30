using UnityEngine;

public class DestroyWallOnCollision : MonoBehaviour
{
    public GameObject DestructionEffect;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Instantiate(DestructionEffect, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}