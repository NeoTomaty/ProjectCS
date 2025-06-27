//======================================================
// [TutorialGanarateSnack]
// �쐬�ҁF�r��C
// �ŏI�X�V���F06/26
// 
// [Log]
// 06/26�@�r��@
// 06/27  ���� �Q�ƃI�u�W�F�N�g�̒ǉ�
//======================================================
using UnityEngine;

public class TutorialGanarateSnack : MonoBehaviour
{
    [Header("��������X�i�b�N")]
    [SerializeField] private GameObject SnackObject;

    [Header("�����܂ł̏Փˉ�")]
    [SerializeField] private int ToGanarateIndex = 3;

    private Vector3 GanaretaSnackPosition = new Vector3(0, 80, 0);


    [Header("�������郊�t�e�B���O�G���A�I�u�W�F�N�g")]
    [SerializeField] private GameObject LiftingAreaObject;


    [Header("���������X�i�b�N�ɓK�p����Q�ƃI�u�W�F�N�g")]
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
