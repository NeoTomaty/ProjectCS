//====================================================
// スクリプト名：SnackLauncher
// 作成者：藤本
// 
// [Log]
// 05/07 藤本　カウントダウン終了後にSnackを打ち上げる処理実装
//====================================================

using UnityEngine;

public class SnackLauncher : MonoBehaviour
{
    [Header("打ち上げる力")]
    [SerializeField] private float launchForce = 300f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Snackを打ち上げる関数
    public void Launch()
    {
        if (rb != null)
        {
            rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
            Debug.Log("Snackを打ち上げました！");
        }
    }
}
