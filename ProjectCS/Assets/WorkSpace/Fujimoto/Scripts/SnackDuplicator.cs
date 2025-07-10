//====================================================
// �X�N���v�g���FSnackDuplicator
// �쐬�ҁF���{
// ���e�F���Ԍo�߂�Snack�𕡐�
// [Log]
// 06/09 ���{�@�X�N���v�g�쐬
// 06/13 ���� ��������BlownAway_Ver3�̊֐����Ăяo��������ǉ�
// 06/25 �r�� �X�i�b�N�̍�����UI�ւ̑����ǉ�
//====================================================

using UnityEngine;
using System.Collections;

public class SnackDuplicator : MonoBehaviour
{
    [Header("�������錳�I�u�W�F�N�g")]
    [SerializeField] private GameObject originalObject;

    [Header("�������郊�t�e�B���O�G���A�I�u�W�F�N�g")]
    [SerializeField] private GameObject LiftingAreaObject;

    [Header("�����ʒu")]
    [SerializeField] private Transform spawnArea;

    [Header("�����Ԋu�i�b�j")]
    [SerializeField] private float interval = 5f;

    [Header("���������_�~�[�X�i�b�N�̍ő吔")]
    [SerializeField] private int maxCount = 10;

    [Header("�X�i�b�N�����X�N���v�g")]
    [SerializeField] private CreateSnackAnnounce AnnounceComponent;


   [Header("�_�~�[�ɓK�p����Q�ƃI�u�W�F�N�g")]
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

        Debug.Log("��������");
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
