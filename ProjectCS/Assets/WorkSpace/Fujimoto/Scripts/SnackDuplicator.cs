//====================================================
// スクリプト名：SnackDuplicator
// 作成者：藤本
// 内容：時間経過でSnackを複製
// [Log]
// 06/09 藤本　スクリプト作成
//====================================================

using UnityEngine;
using System.Collections;

public class SnackDuplicator : MonoBehaviour
{
    [Header("複製する元オブジェクト")]
    [SerializeField] private GameObject originalObject;

    [Header("生成位置")]
    [SerializeField] private Transform spawnArea;

    [Header("複製間隔（秒）")]
    [SerializeField] private float interval = 5f;

    [Header("最大複製数")]
    [SerializeField] private int maxCount = 10;

    private int currentCount = 0;

    private void Start()
    {
        if (spawnArea == null)
        {
            Debug.LogError("spawnArea が設定されていません！");
            return;
        }

        StartCoroutine(DuplicateRoutine());
    }

    private IEnumerator DuplicateRoutine()
    {
        while (currentCount < maxCount)
        {
            yield return new WaitForSeconds(interval);

            // 範囲内のX,Z座標をランダムに取得
            Vector3 areaCenter = spawnArea.position;
            Vector3 areaSize = spawnArea.localScale;

            float randomX = Random.Range(areaCenter.x - areaSize.x / 2f, areaCenter.x + areaSize.x / 2f);
            float randomZ = Random.Range(areaCenter.z - areaSize.z / 2f, areaCenter.z + areaSize.z / 2f);
            float fixedY = areaCenter.y;

            Vector3 spawnPos = new Vector3(randomX, fixedY, randomZ);

            Instantiate(originalObject, spawnPos, Quaternion.identity);
            currentCount++;
        }

        Debug.Log("複製完了");
    }
}
