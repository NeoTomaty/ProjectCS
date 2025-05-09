//====================================================
// スクリプト名：AutoRotation
// 作成者：竹内
// 
// [Log]
// 05/08 竹内 スクリプト作成
//====================================================
using UnityEngine;

public class AutoRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float frequency = 1f;

    private float elapsedTime = 0f;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        float sinValue = Mathf.Sin(elapsedTime * frequency);
        float rotationAngle = sinValue * rotationSpeed;
        transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
    }
}