//SphereSpawner.cs
//作成者:中町雷我
//アタッチ:SpherePrefabにアタッチ
//最終更新日:2025/04/23
//[Log]
//04/23　中町　フィーバータイムに入ったら球体が降ってくる処理

using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    //スポーンする球体のプレハブ
    public GameObject SpherePrefab;

    //球体のスポーン間隔
    public float SpawnInterval = 0.5f;

    //スポーンが有効かどうかを示すフラグ
    private bool IsSpawning = false;

    //再生成を防ぐフラグ
    private bool CanSpawn = true;

    void Start()
    {

    }

    //球体のスポーンを開始する関数
    public void StartSpawning()
    {
        //再生成が許可されているときのみスポーンを開始
        if (CanSpawn)
        {
            IsSpawning = true;

            //指定された間隔で球体をスポーンする
            InvokeRepeating("SpawnSphere", 0.0f, SpawnInterval);
        }
    }

    //球体のスポーンを停止する関数
    public void StopSpawning()
    {
        IsSpawning = false;

        //スポーンの繰り返し呼び出しをキャンセル
        CancelInvoke("SpawnSphere");
    }

    //再生成を防ぐ関数
    public void DisableSpawning()
    {
        CanSpawn = false; //再生成を防ぐ
    }

    //球体をスポーンする関数
    void SpawnSphere()
    {
        //スポーンが有効のときのみ球体を生成
        if (IsSpawning)
        {
            //球体をランダムな位置に生成
            GameObject Sphere = Instantiate(SpherePrefab,
                new Vector3(Random.Range(-10.0f, 10.0f), 10.0f,
                    Random.Range(-10.0f, 10.0f)), Quaternion.identity);

            //生成された球体にタグを追加
            Sphere.tag = "Sphere";
        }
    }
}
