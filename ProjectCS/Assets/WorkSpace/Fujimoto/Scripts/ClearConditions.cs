//======================================================
// ClearConditions�X�N���v�g
// �쐬�ҁF���{
// �ŏI�X�V���F06/05
// 
// [Log]
// 04/29 ���{�@�N���A�����̒ǉ�
// 05/01 �|���@UI�ƘA�g���ă��t�e�B���O�񐔂��Q�Ƃł���悤�ɏC��
// 05/08 �r��@�N���A���o�����s�ł���悤�ɕύX
// 05/10 �r��@�N���A���o�Ɋւ����O������ǉ�
// 06/05 �r��@�V�[���J�ڎ��Ƀt�F�[�h�A�E�g���鏈����ǉ�
//======================================================
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClearConditions : MonoBehaviour
{
    [Header("�N���A����")]
    public int clearCondition = 3; // �N���A�����̃��t�e�B���O��

    [Header("�J�ڐ�V�[��")]
    public string nextSceneName; // �C���X�y�N�^�[�Őݒ�\�ȑJ�ڐ�V�[����

    [Header("UI")]
    public Text liftingCounterText; // Text��ݒ�

    [SerializeField]
    private GameClearSequence GameClearSequence; // �N���A���o�̃X�N���v�g


    private int currentCount; // ���݂̃��t�e�B���O�񐔁i�O������󂯎��j

    // �J�n������
    private void Start()
    {
        // �N���A�񐔂�ݒ�
        currentCount = clearCondition;
        UpdateLiftingCounterUI();
    }

    // �O�����烊�t�e�B���O�񐔂����炷���\�b�h
    public void CheckLiftingCount()
    {
        currentCount--;
        Debug.Log("Current Lifting Count: " + currentCount);

        UpdateLiftingCounterUI();

        // �N���A�����𖞂����Ă��邩�`�F�b�N
        if (currentCount <= 0)
        {
            Debug.Log("ClearConditions�X�N���v�g�F�����𖞂��������߃V�[���J�ڂ��s���܂�");

            if(GameClearSequence == null)
            {
                TriggerSceneTransition();
            }
            else
            {
                bool Res = GameClearSequence.OnGameClear();

                // OnGameClear�֐�������ɏI�����Ȃ������ꍇ
                // ���o�����ŃV�[���J��
                if (!Res)
                {
                    Debug.LogError("ClearConditions�X�N���v�g�FOnGameClear�֐�������ɏI�����܂���ł���");
                    TriggerSceneTransition();
                }
            }
        }
    }

    // ���t�e�B���O�񐔂̍X�V
    private void UpdateLiftingCounterUI()
    {
        if (liftingCounterText != null)
        {
            liftingCounterText.text = "Lift Count: " + currentCount.ToString();
        }
        else
        {
            Debug.LogWarning("liftingCounterText ���ݒ肳��Ă��܂���I");
        }
    }

    // �V�[���J�ڂ����s
    public void TriggerSceneTransition()
    {
        if (!string.IsNullOrEmpty(nextSceneName)) // �V�[�������ݒ肳��Ă���ꍇ�̂ݎ��s
        {
            Debug.Log("Loading Scene: " + nextSceneName);

            // �t�F�[�h�A�E�g���ăV�[���J��
            FadeManager fade = FindFirstObjectByType<FadeManager>();
            if (fade != null)
            {
                fade.FadeToScene(nextSceneName);
            }
            else
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
        else
        {
            Debug.LogError("���̃V�[�������ݒ肳��Ă��܂���I");
        }
    }
}
