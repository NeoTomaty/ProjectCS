//====================================================
// �X�N���v�g���FSnackDuplicator
// �쐬�ҁF���{
// ���e�F���Ԍo�߂�Snack�𕡐�
// [Log]
// 06/09 ���{�@�X�N���v�g�쐬
//====================================================

using UnityEngine;
using System.Collections;

public class SnackDuplicator : MonoBehaviour
{
    [Header("�������錳�I�u�W�F�N�g")]
    [SerializeField] private GameObject originalObject;

    [Header("�����ʒu")]
    [SerializeField] private Transform spawnArea;

    [Header("�����Ԋu�i�b�j")]
    [SerializeField] private float interval = 5f;

    [Header("�ő啡����")]
    [SerializeField] private int maxCount = 10;

    private int currentCount = 0;

    private void Start()
    {
        if (spawnArea == null)
        {
            Debug.LogError("spawnArea ���ݒ肳��Ă��܂���I");
            return;
        }

        StartCoroutine(DuplicateRoutine());
    }

    private IEnumerator DuplicateRoutine()
    {
        while (currentCount < maxCount)
        {
            yield return new WaitForSeconds(interval);

            // �͈͓���X,Z���W�������_���Ɏ擾
            Vector3 areaCenter = spawnArea.position;
            Vector3 areaSize = spawnArea.localScale;

            float randomX = Random.Range(areaCenter.x - areaSize.x / 2f, areaCenter.x + areaSize.x / 2f);
            float randomZ = Random.Range(areaCenter.z - areaSize.z / 2f, areaCenter.z + areaSize.z / 2f);
            float fixedY = areaCenter.y;

            Vector3 spawnPos = new Vector3(randomX, fixedY, randomZ);

            Instantiate(originalObject, spawnPos, Quaternion.identity);
            currentCount++;
        }

        Debug.Log("��������");
    }
}
