//======================================================
// [GameClearSequence]
// �쐬�ҁF�r��C
// �ŏI�X�V���F05/08
// 
// [Log]
// 05/08�@�r��@���̃N���A���o���쐬
//======================================================
using UnityEngine;
using UnityEngine.UI;

// ClearUI�v���n�u�ɃA�^�b�`����Ă���
// ClearConditions�X�N���v�g�ɂ��̃X�N���v�g���Z�b�g���Ă��Ȃ��ꍇ�A�N���A���o���Đ����ꂸ�����ɃV�[���J�ڂ���
public class GameClearSequence : MonoBehaviour
{
    // �Q�[���I��������UI
    [SerializeField] GameObject ClearLogo;

    // �N���A���o���̔w�i
    [SerializeField] GameObject ClearBackImage;

    // �X�i�b�N�̃��X�|�[�����Ǘ�����X�N���v�g�i���X�|�[���𖳌������鏈��������Ă����j
    [SerializeField] BlownAway_Ver2 BlownAway;

    // �V�[���J�ڂ��Ǘ�����X�N���v�g
    [SerializeField] ClearConditions ClearConditions;

    // �N���A���o���t���O
    private bool IsClearSequence = false;

    // �N���A��^�C�}�[
    private float AfterTimer = 0f;

    // �N���A�����𖞂��������ɌĂяo���֐�
    public void OnGameClear()
    {
        if (ClearLogo == null || ClearBackImage == null || BlownAway == null || ClearConditions == null) return;

        // �X�i�b�N�̃��X�|�[���𖳌���
        BlownAway.OnClear();

        // �Q�[���I��������UI��\��
        ClearLogo.SetActive(true);

        // �N���A���o���̔w�i��\��
        ClearBackImage.SetActive(true);

        // �N���A���o���t���O�𗧂Ă�
        IsClearSequence = true;
    }

    private void Start()
    {
        ClearLogo.SetActive(false);
        ClearBackImage.SetActive(false);
    }

    void Update()
    {
        if(!IsClearSequence) return;

        // �L�[�E�{�^�����͂ŃV�[���J��
        if (Input.anyKeyDown)
        {
            ClearConditions.TriggerSceneTransition();
        }

        // �^�C�}�[�i�s
        AfterTimer += Time.deltaTime;

        if (AfterTimer > 1.5f)
        {
            // �Q�[�����~�܂��Ă��Ȃ������炱���Ŏ~�߂�
            if (Time.timeScale != 0f)
            {
                Time.timeScale = 0f;
            }
        }
    }
}
