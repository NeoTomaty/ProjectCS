//====================================================
// �X�N���v�g���FStageSelectManager_Ver2
// �쐬�ҁF����
// ���e�F�X�e�[�W�Z���N�g���ꊇ�Ǘ�����X�N���v�g
// �ŏI�X�V���F05/23
// 
// [Log]
// 05/23 ���� �X�N���v�g�쐬
//====================================================
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StageSelectManager_Ver2 : MonoBehaviour
{


    [SerializeField] private GameObject StageObject;
    [SerializeField] private GameObject BezierCurveObject;
    [SerializeField] private GameObject StageImageUI;
    [SerializeField] private Image StageImage;
    [SerializeField] private BezierMover BezierMoverComponent;
 

    private Transform[] StageChildArray;
    private Transform[] StageModelTransform;
    private CubicBezierCurve[] BezierCurveChildArray;
    private StageSelector StageSelectorComponent;


    [SerializeField] private string[] GameSceneNames = new string[6];

    [SerializeField] private string TitleSceneName;
    [SerializeField] private Sprite[] StageImageSprites = new Sprite[5];

    private bool IsReverse = false;
    private bool IsInputEnabled = false;
    private float ScaleChangeTimer = 0f;
    [SerializeField] private float ScaleChangeDuration = 0.5f;
    private bool IsScaling = true;
    [SerializeField] private Vector3 SmallStageModelSize = new Vector3(0.5f, 0.5f, 0.5f);


    void Start()
    {
        if (!StageObject) Debug.LogError("StageObject���ݒ肳��Ă��܂���");
        if (!BezierCurveObject) Debug.LogError("BezierCurveObject���ݒ肳��Ă��܂���");

        int childCount = StageObject.transform.childCount;
        StageChildArray = new Transform[childCount];
        StageModelTransform = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            StageChildArray[i] = StageObject.transform.GetChild(i);
            StageModelTransform[i] = StageChildArray[i].transform.GetChild(0);
            StageModelTransform[i].transform.localScale = SmallStageModelSize;
        }


        childCount = BezierCurveObject.transform.childCount;
        BezierCurveChildArray = new CubicBezierCurve[childCount];

        for (int i = 0; i < childCount; i++)
        {
            BezierCurveChildArray[i] = BezierCurveObject.transform.GetChild(i).GetComponent<CubicBezierCurve>();
        }

        StageSelectorComponent = Object.FindFirstObjectByType<StageSelector>();

        if (!StageSelectorComponent)
        {
            Debug.LogError("StageSelector��������܂���");
            return;
        }
       
        if(StageSelectorComponent.GetStageNumber() == BezierCurveChildArray.Length)
        {
            BezierMoverComponent.SetPosition(BezierCurveChildArray[StageSelectorComponent.GetStageNumber() - 1].StageObject2.position);
        }
        else
        {
            BezierMoverComponent.SetPosition(BezierCurveChildArray[StageSelectorComponent.GetStageNumber()].StageObject1.position);
        }

        StageImage.sprite = StageImageSprites[StageSelectorComponent.GetStageNumber()];

        StageImageUI.transform.localScale = Vector3.zero;

    }

    private void OnValidate()
    {
        if (GameSceneNames.Length != StageImageSprites.Length)
        {

            Sprite[] newArray = new Sprite[GameSceneNames.Length];
            for (int i = 0; i < Mathf.Min(GameSceneNames.Length, StageImageSprites.Length); i++)
            {
                newArray[i] = StageImageSprites[i]; // �����̒l��ێ�
            }
            StageImageSprites = newArray;
        }
    }


    void Update()
    {
        if (StageChildArray.Length - 1 != BezierCurveChildArray.Length) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (StageSelectorComponent.GetStageNumber() <= 0) return;

            if (BezierMoverComponent.GetIsMoving() && IsReverse) return;

            IsReverse = true;

            BezierMoverComponent.StartMove(true, BezierCurveChildArray[StageSelectorComponent.GetStageNumber() - 1]);

            StageSelectorComponent.SetStageNumber(-1);
            
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            if (StageSelectorComponent.GetStageNumber() == BezierCurveChildArray.Length) return;

            if (BezierMoverComponent.GetIsMoving() && !IsReverse) return;

            IsReverse = false;

            BezierMoverComponent.StartMove(false, BezierCurveChildArray[StageSelectorComponent.GetStageNumber()]);

            StageSelectorComponent.SetStageNumber(1);
           
        }

        //if (Input.GetKeyDown(KeyCode.Return) && !BezierMoverComponent.GetIsMoving())
        //{
        //    if(GameSceneNames.Length == StageSelectorComponent.GetStageNumber()) return;

        //    ChangeScene(GameSceneNames[StageSelectorComponent.GetStageNumber()]);
           
        //}

        if(!BezierMoverComponent.GetIsMoving())
        {
            ScaleChangeTimer += Time.deltaTime;
            float t = Mathf.Clamp01(ScaleChangeTimer / ScaleChangeDuration);
            StageImageUI.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);

            for (int i = 0; i < StageModelTransform.Length; i++)
            {
                if (i == StageSelectorComponent.GetStageNumber())
                {
                    if(StageModelTransform[i].transform.localScale != Vector3.one)
                    {
                        StageModelTransform[i].transform.localScale = Vector3.Lerp(SmallStageModelSize, Vector3.one, t);
                    }

                    StageModelTransform[i].transform.Rotate(0f, 10.0f * Time.deltaTime, 0f);
                }
                else
                {
                    StageModelTransform[i].transform.rotation = Quaternion.identity;
                }

               
            }


            if (t >= 1.0f)
            {
                ScaleChangeTimer = ScaleChangeDuration;
            }
        }
        else
        {
            ScaleChangeTimer -= Time.deltaTime;
            float t = Mathf.Clamp01(ScaleChangeTimer / ScaleChangeDuration);
            StageImageUI.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
           

            for(int i = 0; i < StageModelTransform.Length;i++)
            {
                if(StageModelTransform[i].transform.localScale != SmallStageModelSize)
                {
                    StageModelTransform[i].transform.localScale = Vector3.Lerp(SmallStageModelSize, Vector3.one, t);
                }

            }

            if (t <= 0.0f)
            {
                ScaleChangeTimer = 0f;
                StageImage.sprite = StageImageSprites[StageSelectorComponent.GetStageNumber()];
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace) && !BezierMoverComponent.GetIsMoving())
        {
            ChangeScene(TitleSceneName);
        }

        

    }

    private void ChangeScene(string sceneName)
    {
        FadeManager fade = Object.FindFirstObjectByType<FadeManager>();
        if (fade)
        {
            fade.FadeToScene(sceneName);
        }
        else
        {
            Debug.LogError("FadeManager��������܂���");
        }
    }
}
