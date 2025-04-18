//====================================================
// �X�N���v�g���FChangeMiniMapSize
// �쐬�ҁF�|��
// ���e�F�~�j�J�����ɃA�^�b�`���邱�Ƃ�
// �@�@�@�L�[���͂Ń~�j�}�b�v�̃T�C�Y��ς�����
// �ŏI�X�V���F04/16
// 
// [Log]
// 04/16 �|�� �X�N���v�g�쐬
//====================================================
using UnityEngine;

public class ChangeMiniMapSize : MonoBehaviour
{
    private Camera miniMapCamera;
    private float[] sizes = { 20f, 40f, 60f };  // �T�C�Y���
    private int currentSizeIndex = 0;          // ���݂̃C���f�b�N�X
    private float targetSize;                  // �ڕW�T�C�Y
    [SerializeField] private float smoothSpeed = 5f; // ��ԑ��x�i�����j

    void Start()
    {
        miniMapCamera = GetComponent<Camera>();
        if (miniMapCamera == null)
        {
            Debug.LogError("MiniMapCamera���A�^�b�`����Ă��܂���B");
            return;
        }

        targetSize = sizes[currentSizeIndex];
        miniMapCamera.orthographicSize = targetSize;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ���̃C���f�b�N�X�Ɉړ��i���[�v�j
            currentSizeIndex = (currentSizeIndex + 1) % sizes.Length;
            targetSize = sizes[currentSizeIndex];
        }

        // ���炩�ɃT�C�Y����
        miniMapCamera.orthographicSize = Mathf.Lerp(
            miniMapCamera.orthographicSize,
            targetSize,
            Time.deltaTime * smoothSpeed
        );
    }
}
