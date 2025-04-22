//======================================================
// [SelfDestroy]
// �쐬�ҁF�r��C
// �ŏI�X�V���F04/22
// 
// [Log]
// 04/22�@�r��@�Փ˂����I�u�W�F�N�g������̃^�O�������玩�����g���폜����悤�Ɏ���
//======================================================
using UnityEngine;

// ����̃I�u�W�F�N�g�ƏՓ˂������Ɏ������g���폜����N���X
// ��΂��I�u�W�F�N�g�ɃA�^�b�`
public class SelfDestroy : MonoBehaviour
{
    [SerializeField] private GameObject GroundObject;       // �n�ʃI�u�W�F�N�g
    [SerializeField] private GameObject DestroyerObject;    // �������폜����I�u�W�F�N�g

    // �^�O
    private string GroundTag = "Ground";        // �n��
    private string DestroyerTag = "Destroyer";  // �������폜����I�u�W�F�N�g


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(GroundObject != null)
        {
            GroundTag = GroundObject.tag;   // �n�ʃI�u�W�F�N�g�̃^�O���擾
        }

        if (DestroyerObject != null)
        {
            DestroyerTag = DestroyerObject.tag; // �������폜����I�u�W�F�N�g�̃^�O���擾
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �Փ˂����I�u�W�F�N�g�̃^�O���擾
        string collidedTag = collision.gameObject.tag;

        // �n�ʂ������ȃI�u�W�F�N�g�ƏՓ˂����ꍇ�Ɏ������g���폜
        if (collidedTag == GroundTag || collidedTag == DestroyerTag)
        {
            // �������g���폜
            Destroy(this.gameObject);
        }
    }
}
