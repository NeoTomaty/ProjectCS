//====================================================
// スクリプト名：AutoMovement
// 作成者：竹内
// 
// [Log]
// 05/08 竹内 スクリプト作成
//====================================================
using UnityEngine;

public class AutoMovement : MonoBehaviour
{
    [SerializeField] private Vector3 moveDistance = new Vector3(3f, 3f, 3f);
    [SerializeField] private float speed = 1f;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float offsetX = Mathf.Sin(Time.time * speed) * moveDistance.x;
        float offsetY = Mathf.Sin(Time.time * speed) * moveDistance.y;
        float offsetZ = Mathf.Sin(Time.time * speed) * moveDistance.z;
        transform.position = startPosition + new Vector3(offsetX, offsetY, offsetZ);
    }
}