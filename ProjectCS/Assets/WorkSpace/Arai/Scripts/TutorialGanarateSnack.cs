//======================================================
// [TutorialGanarateSnack]
// �쐬�ҁF�r��C
// �ŏI�X�V���F06/27
// 
// [Log]
// 06/26�@�r��@�ǂɈ��񐔂Ԃ���ƃX�i�b�N�����������悤�Ɏ���
// 06/27  ���� �Q�ƃI�u�W�F�N�g�̒ǉ�
// 06/27  �r�� ������S�̓I�ɕύX
//======================================================
using UnityEngine;

public class TutorialGanarateSnack : MonoBehaviour
{
    [Header("��������X�i�b�N")]
    [SerializeField] private GameObject SnackObject;

    [Header("�����܂ł̏Փˉ�")]
    [SerializeField] private int ToGanarateIndex = 3;

    [SerializeField] private TutorialDisplayTexts TutorialDisplayTextsComponent;

    [Header("�J�n���ɔ�\���ɂ���I�u�W�F�N�g�̎Q��")]
    [SerializeField] private GameObject ArrowObject;
    [SerializeField] private GameObject SnackHeightGaugeObject;

    private int CollidedIndex = 0;

    private void Start()
    {
        ArrowObject.SetActive(false);
        SnackHeightGaugeObject.SetActive(false);
    }

    public void OnCollided()
    {
        CollidedIndex++;

        if (CollidedIndex != ToGanarateIndex) return;

        if (SnackObject != null)
        {
            SnackObject.SetActive(true);
            ArrowObject.SetActive(true);
            SnackHeightGaugeObject.SetActive(true);

            TutorialDisplayTextsComponent.DisplayTutorialUI2();
        }
    }
}