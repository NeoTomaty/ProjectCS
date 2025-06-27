//====================================================
// �X�N���v�g���FStageSelector
// �쐬�ҁF����
// ���e�F�I�𒆂̃X�e�[�W�̏�Ԃ�ێ�����
// �ŏI�X�V���F06/01
// 
// [Log]
// 06/01 ���� �X�N���v�g�쐬
//====================================================
using UnityEngine;

// ���ݑI�𒆂̃X�e�[�W�ԍ����Ǘ�����V���O���g��
public class StageSelector : MonoBehaviour
{
    // �X�e�[�W�̔ԍ��i�񋓌^�j
    public enum SelectStageNumber
    {
        Tutorial,
        Stage1,
        Stage2,
        Stage3,
        Stage4,
        Stage5,
        Stage6,
        Stage7,
        Stage8,
        Stage9,
    }

    // ���ݑI�𒆂̃X�e�[�W�istatic�F�ǂ̃V�[������ł��Q�Ɖ\�j
    private static SelectStageNumber SelectStage = SelectStageNumber.Tutorial;

    // ���݂̃X�e�[�W�ԍ��i0�`5�j�� int �Ŏ擾
    private int CurrentIndex => (int)SelectStage;

    // �X�e�[�W�̍ő�C���f�b�N�X�l�i�񋓌^�̗v�f�� - 1�j
    private int MaxIndex => System.Enum.GetValues(typeof(SelectStageNumber)).Length - 1;

    // �V���O���g���C���X�^���X�i�B���StageSelector�j
    public static StageSelector Instance { get; private set; }

    // �V���O���g������������
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �V�[�����܂����ł��ێ������
        }
        else
        {
            Destroy(gameObject); // ���łɑ��݂���ꍇ�͎����I�ɔj��
        }
    }

    // �X�e�[�W�ԍ���ύX����i+1, -1�Ȃǂ̑��Ύw��j
    /// <param name="delta">�ύX����l�i��: +1�Ŏ��̃X�e�[�W�j</param>
    public void SetStageNumber(int delta)
    {
        // ���݂̃C���f�b�N�X�� delta �����Z���A0�`�ő�l�͈̔͂ɐ���
        int newIndex = Mathf.Clamp(CurrentIndex + delta, 0, MaxIndex);
        SelectStage = (SelectStageNumber)newIndex;

        // �f�o�b�O�o��
        Debug.Log("���݂̃X�e�[�W: " + SelectStage);
    }

    // ���݂̃X�e�[�W�ԍ��iint�j���擾
    /// <returns>�X�e�[�W�ԍ��i0�`5�j</returns>
    public int GetStageNumber()
    {
        return (int)SelectStage;
    }
}
