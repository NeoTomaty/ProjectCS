using UnityEngine;

public class DestroyWallOnCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("DestructionWall"))
        {
            Destroy(collision.gameObject);
        }
    }
}