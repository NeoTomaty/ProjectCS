//======================================================
// [TutorialGanarateSnack]
// 作成者：荒井修
// 最終更新日：06/26
// 
// [Log]
// 06/26　荒井　
// 06/27  高下 参照オブジェクトの追加
//======================================================
using UnityEngine;

public class TutorialGanarateSnack : MonoBehaviour
{
    [Header("生成するスナック")]
    [SerializeField] private GameObject SnackObject;

    [Header("生成までの衝突回数")]
    [SerializeField] private int ToGanarateIndex = 3;

    private Vector3 GanaretaSnackPosition = new Vector3(0, 80, 0);


    [Header("生成するリフティングエリアオブジェクト")]
    [SerializeField] private GameObject LiftingAreaObject;


    [Header("生成したスナックに適用する参照オブジェクト")]
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
    [SerializeField] private SnackHeightUIManager SnackHeightUIManagerComponent;
    [SerializeField] private AnimationFinishTrigger AnimationFinishComponent;

    private int CollidedIndex = 0;

    public void OnCollided()
    {
        CollidedIndex++;

        if (CollidedIndex != ToGanarateIndex) return;

        if (SnackObject != null)
        {
            GameObject SnackInstance = Instantiate(SnackObject, GanaretaSnackPosition, Quaternion.identity);
            Debug.Log(SnackInstance.transform.position);
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

            GameObject LiftingAreaInstance = Instantiate(LiftingAreaObject, new Vector3(0f, -1000f, 0f), Quaternion.identity, SceneObjectTransform);
            LiftingAreaManager LAM = LiftingAreaInstance.GetComponent<LiftingAreaManager>();
            LAM.SetTarget(PlayerObject, SnackInstance, ClearSequenceComponent, AnimationFinishComponent);

            FallPointCalculator FPC = SnackInstance.GetComponent<FallPointCalculator>();
            FPC.SetTarget(LAM);

            SnackRespawner SR = SnackInstance.GetComponent<SnackRespawner>();
            SR.SetTarget(SnackRespawnPoint);

            SnackHeightUIManagerComponent.AddSnack(SnackInstance);
        }
    }
}
