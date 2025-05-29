//using UnityEditor.SceneManagement;
using UnityEngine;

public class StageSelector : MonoBehaviour
{
    public enum SelectStageNumber
    {
        Stage1,
        Stage2,
        Stage3
    }

    private SelectStageNumber SelectStage = SelectStageNumber.Stage1;

    // ���݂̃X�e�[�W�𐔒l�Ƃ��Ď擾
    private int CurrentIndex => (int)SelectStage;

    // �X�e�[�W�̍ő吔�i�Ō��index�j
    private int MaxIndex => System.Enum.GetValues(typeof(SelectStageNumber)).Length - 1;

    public static StageSelector Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �ŏ���1�����c��
        }
        else
        {
            Destroy(gameObject); // 2�ڈȍ~�͔j��
        }
    }

    public void SetStageNumber(int delta)
    {
        int newIndex = Mathf.Clamp(CurrentIndex + delta, 0, MaxIndex);
        SelectStage = (SelectStageNumber)newIndex;
        Debug.Log("���݂̃X�e�[�W: " + SelectStage);
    }

    public int GetStageNumber()
    {
        return (int)SelectStage;
    }
}
