//====================================================
// スクリプト名：SnackDuplicator
// 作成者：藤本
// 内容：時間経過でSnackを複製
// [Log]
// 06/09 藤本　スクリプト作成
// 06/13 高下 生成時にBlownAway_Ver3の関数を呼び出す処理を追加
// 06/25 荒井 スナックの高さのUIへの操作を追加
//====================================================

using UnityEngine;
using System.Collections;

public class SnackDuplicator : MonoBehaviour
{
    [Header("複製する元オブジェクト")]
    [SerializeField] private GameObject originalObject;

    [Header("複製するリフティングエリアオブジェクト")]
    [SerializeField] private GameObject LiftingAreaObject;

    [Header("生成位置")]
    [SerializeField] private Transform spawnArea;

    [Header("複製間隔（秒）")]
    [SerializeField] private float interval = 5f;

    [Header("複製されるダミースナックの最大数")]
    [SerializeField] private int maxCount = 10;

    [Header("スナック生成スクリプト")]
    [SerializeField] private CreateSnackAnnounce AnnounceComponent;


   [Header("ダミーに適用する参照オブジェクト")]
    [SerializeField] private CameraFunction CameraFunctionComponent;
    [SerializeField] private FlyingPoint FlyingPointComponent;
    [SerializeField] private ClearConditions ClearConditionsComponent;
    [SerializeField] private LiftingJump LiftingJumpComponent;
    [SerializeField] private GameObject PlayerObject;
    [SerializeField] private Transform SnackRespawnPoint;
    [SerializeField] private Transform GroundArea;
    [SerializeField] private GameClearSequence ClearSequenceComponent;
    [SerializeField] private Transform SceneObjectTransform;
    [SerializeField] private PlayerAnimationController PlayerAnimationController;
    [SerializeField] private SnackHeightUIManager_Ver2 SnackHeightUIManagerVer2Component;
    [SerializeField] private SnackHeightUIManager SnackHeightUIManagerComponent;
    [SerializeField] private AnimationFinishTrigger AnimationFinishComponent;

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

            GameObject SnackInstance = Instantiate(originalObject, spawnPos, Quaternion.identity, SceneObjectTransform);
            BlownAway_Ver3 BA3 = SnackInstance.GetComponent<BlownAway_Ver3>();
            BA3.SetTarget(
                CameraFunctionComponent,
                FlyingPointComponent, 
                ClearConditionsComponent, 
                LiftingJumpComponent,
                SnackRespawnPoint,
                GroundArea,
                PlayerAnimationController
                );

            if(SnackHeightUIManagerVer2Component) SnackHeightUIManagerVer2Component.SetSnackObject(SnackInstance);

           
            GameObject LiftingAreaInstance = Instantiate(LiftingAreaObject, new Vector3(0f, -1000f, 0f), Quaternion.identity, SceneObjectTransform);
            LiftingAreaManager LAM = LiftingAreaInstance.GetComponent<LiftingAreaManager>();
            LAM.SetTarget(PlayerObject, SnackInstance, ClearSequenceComponent, AnimationFinishComponent);

            FallPointCalculator FPC = SnackInstance.GetComponent<FallPointCalculator>();
            FPC.SetTarget(LAM);

            SnackRespawner SR = SnackInstance.GetComponent<SnackRespawner>();
            SR.SetTarget(SnackRespawnPoint);

            if(SnackHeightUIManagerComponent) SnackHeightUIManagerComponent.AddSnack(SnackInstance);

            if(AnnounceComponent) AnnounceComponent.DisplayMessage();

            currentCount++;
        }

        Debug.Log("複製完了");
    }

    public int GetMaxSnackCount()
    {
        return maxCount + 1;
    }
    public int GetCurrentSnackCound()
    {
        return currentCount + 1;
    }
}
