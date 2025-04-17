using UnityEngine;




public class DropProjectorController : MonoBehaviour
{
    public Transform targetObject;       // �R���΂��I�u�W�F�N�g
    public float baseSize = 1f;
    public float sizeMultiplier = 0.1f;
    public float groundY = 0f;           // �n�ʂ�Y���W
    public float projectorOffsetY = -0.05f;  // �n�ʂ�菭����
    public float minYThreshold = 0.2f;   // �����艺�Ȃ�projector���I�t

    private Projector projector;

    void Start()
    {
        projector = GetComponent<Projector>();
    }

    void Update()
    {
        if (targetObject == null || projector == null) return;

        float targetY = targetObject.position.y;

        // Y���W������艺�Ȃ�Projector�𖳌���
        if (targetY < minYThreshold)
        {
            projector.enabled = false;
            return; // ����ȏ㏈�����Ȃ�
        }
        else
        {
            projector.enabled = true;
        }

        // Projector�̈ʒu�iXZ�Ǐ]�AY�͌Œ�j
        Vector3 pos = targetObject.position;
        pos.y = groundY + projectorOffsetY;
        transform.position = pos;

        // ��]�Œ�i�^���j
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        // �����ɉ����ăT�C�Y����
        float height = Mathf.Max(0.01f, targetY - groundY);
        projector.orthographicSize = baseSize + height * sizeMultiplier;
    }
}