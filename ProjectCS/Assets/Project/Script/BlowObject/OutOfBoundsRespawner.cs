//======================================================
// [OutOfBoundsRespawner]
// �쐬�ҁF�r��C
// �ŏI�X�V���F04/22
// 
// [Log]
// 04/22�@�r��@�X�e�[�W�O�ɏo���u�ԂɎ������g�̕����𐶐�����悤�Ɏ���
// 04/22�@�r��@���X�|�[���ʒu�������_���ɂȂ�悤�ɕύX
//======================================================
using UnityEngine;

// �͈͊O�ɏo���I�u�W�F�N�g�����X�|�[��������N���X
// �X�e�[�W����]���Ă��Ȃ��l�p�`�͈͂ł��邱�Ƃ�O��Ƃ���
// ��΂��I�u�W�F�N�g�ɃA�^�b�`
public class OutOfBoundsRespawner : MonoBehaviour
{
    [SerializeField] private float RespawnPositionY;    // ���X�|�[���ʒu
    private Vector3 RespawnPosition;                    // ���X�|�[���ʒu
    private GameObject RespawnObject;                   // ���X�|�[���Ώ�

    [SerializeField] private GameObject Stage;  // �X�e�[�W
    private Rect StageRect;                     // �X�e�[�W�̔����`

    private bool IsRespawnComplete = false; // ���X�|�[�������t���O

    [SerializeField] private float WallThickness = 2.0f;    // �ǂ̌���

    // �����_���ȍ��W���v�Z
    private void CalcRandomPosition()
    {
        // �ǂ������������_�������̈���`
        float MinX = StageRect.xMin + WallThickness;
        float MaxX = StageRect.xMax - WallThickness;
        float MinY = StageRect.yMin + WallThickness;
        float MaxY = StageRect.yMax - WallThickness;

        // �͈͓��Ń����_���ȍ��W�𐶐�
        float RandomX = Random.Range(MinX, MaxX);
        float RandomY = Random.Range(MinY, MaxY);

        // ���X�|�[���ʒu��ݒ�
        RespawnPosition = new Vector3(RandomX, RespawnPositionY, RandomY); // Y���W�͌��̃I�u�W�F�N�g��Y���W���g�p
    }

    // �X�e�[�W������
    private bool IsInsideStage()
    {
        if(Stage == null) return false;

        // ���X�|�[���Ώۂ̍��W���擾
        Vector3 ObjectPosition = RespawnObject.transform.position;
        Vector2 ObjectPosition2D = new Vector2(ObjectPosition.x, ObjectPosition.z); // 2D���W�ɕϊ�

        // �X�e�[�W�͈͓̔��ɂ��邩����
        if (StageRect.Contains(ObjectPosition2D))
        {
            return true; // �X�e�[�W��
        }
        else
        {
            return false; // �X�e�[�W�O
        }
    }

    // ���X�|�[������
    private void Respawn()
    {
        // ���X�|�[���ʒu���v�Z
        CalcRandomPosition();

        // ���X�|�[���Ώۂ𕡐�
        GameObject RespawnedObject = Instantiate(RespawnObject, RespawnPosition, Quaternion.identity);

        // ���X�|�[�������t���O�𗧂Ă�
        IsRespawnComplete = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Stage == null) return;

        // ���X�|�[���Ώۂ����g�ɐݒ�
        RespawnObject = this.gameObject;

        // �X�e�[�W�̍��W�ƃT�C�Y���擾
        Vector3 StagePosition = Stage.transform.position;
        Vector3 StageSize = Stage.GetComponent<Renderer>().bounds.size;

        // �X�e�[�W�̔����`���쐬
        float halfWidth = StageSize.x / 2;
        float halfHeight = StageSize.z / 2;
        StageRect = new Rect(StagePosition.x - halfWidth, StagePosition.z - halfHeight, StageSize.x, StageSize.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRespawnComplete) return;

        if (!IsInsideStage())
        {
            Respawn(); // �X�e�[�W�O�ɏo���烊�X�|�[��
        }
    }
}
