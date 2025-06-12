//====================================================
// �X�N���v�g���FUILineConnector
// �쐬�ҁF����
// ���e�F���ݑI�𒆂̃X�e�[�W��UI�ŕ\��
// �ŏI�X�V���F06/01
// 
// [Log]
// 06/01 ���� �X�N���v�g�쐬
//====================================================
using UnityEngine;
using System.Collections.Generic;

// ������UI����ׁA�I�𒆂�UI�̈ʒu�Ƀv���C���[UI���ړ�������UI����N���X
public class UILineConnector : MonoBehaviour
{
    [SerializeField] private GameObject UIPrefab;     // UI�v�f�̃v���n�u�i��F�I���A�C�R���Ȃǁj
    [SerializeField] private int Count = 5;           // ��������UI�̌�
    [SerializeField] private float TotalWidth = 400f; // ���ׂ�UI�S�̂̉���

    // �������ꂽUI�I�u�W�F�N�g��RectTransform��ێ�
    private List<RectTransform> UIList = new List<RectTransform>();

    // UI�̐e�ƂȂ�Canvas��RectTransform�i���g�p�����g�����Ɏg����j
    private RectTransform CanvasRect;

    // ���g�i���̃X�N���v�g���A�^�b�`���ꂽUI�I�u�W�F�N�g�j��RectTransform
    private RectTransform SelfRect;

    // ����`�悷��UI�i���������o�I�ɕ⏕����p�j
    private RectTransform LineRect;

    // �v���C���[�ʒu�Ȃǂ�����UI�i�I���J�[�\���I��UI�j
    private RectTransform PlayerUIRect;

    void Start()
    {
        // �������g��RectTransform���擾
        SelfRect = GetComponent<RectTransform>();

        // �q�I�u�W�F�N�g0�Ԗڂ���p��RectTransform�Ƃ݂Ȃ��Ď擾
        LineRect = transform.GetChild(0).GetComponent<RectTransform>();

        // ���̕���UI�S�̂̕��ɍ��킹�Ē���
        Vector2 size = LineRect.sizeDelta;
        size.x = TotalWidth;
        LineRect.sizeDelta = size;

        // �q�I�u�W�F�N�g1�Ԗڂ��u�v���C���[UI�v�i�I���J�[�\���j�Ƃ݂Ȃ��Ď擾
        PlayerUIRect = transform.GetChild(1).GetComponent<RectTransform>();

        // �Ԋu���v�Z�iCount��1�ȉ��̏ꍇ��0�j
        float spacing = (Count > 1) ? TotalWidth / (Count - 1) : 0f;

        // ���g�̒��S�ʒu�����UI�����E�ɕ��ׂ�
        Vector2 center = SelfRect.anchoredPosition;

        for (int i = 0; i < Count; i++)
        {
            // UI�v�f�𐶐����A�e�� SelfRect �ɐݒ�
            GameObject obj = Instantiate(UIPrefab, SelfRect);
            RectTransform rt = obj.GetComponent<RectTransform>();

            // x���W�𓙊Ԋu�Ɍv�Z���z�u�i������0�Ƃ��č�����E�֕��ׂ�j
            float x = -TotalWidth / 2f + i * spacing;
            rt.anchoredPosition = new Vector2(x, 0f);

            // UI�̕`�揇�𒲐��i���̒���ɒǉ����邽�� +1�j
            rt.SetSiblingIndex(1 + i);

            // �Ǘ��p���X�g�ɒǉ�
            UIList.Add(rt);
        }
    }

    // �w�肳�ꂽ�C���f�b�N�X�̈ʒu�Ɂu�v���C���[UI�v���ړ�������
    /// <param name="arrayNum">�Ώۂ�UI�C���f�b�N�X�i0�`Count-1�j</param>
    public void SetArrayNumber(int arrayNum)
    {
        // �v���C���[UI���A�w��C���f�b�N�X��UI�̈ʒu�ֈړ�
        PlayerUIRect.anchoredPosition = UIList[arrayNum].anchoredPosition;
    }
}