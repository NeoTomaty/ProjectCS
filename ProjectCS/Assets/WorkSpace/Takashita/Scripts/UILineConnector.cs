using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UILineConnector : MonoBehaviour
{
    [SerializeField] private GameObject UIPrefab;         // ��������UI�v���n�u
    [SerializeField] private int Count = 5;               // �������鐔
    [SerializeField] private float TotalWidth = 400f;     // ���ׂ鉡��

    private List<RectTransform> UIList = new List<RectTransform>();
    private RectTransform CanvasRect;       // Canvas��RectTransform�i�e�j
    private RectTransform SelfRect;         // �������g��RectTransform�i�N�_�ʒu�j
    private RectTransform LineRect;
    private RectTransform PlayerUIRect;

    void Start()
    {
        SelfRect = GetComponent<RectTransform>();
        // �q�I�u�W�F�N�g���� LineRect / PlayerUIRect ���擾
        LineRect = transform.GetChild(0).GetComponent<RectTransform>();
        Vector2 size = LineRect.sizeDelta;
        size.x = TotalWidth;
        LineRect.sizeDelta = size;

        PlayerUIRect = transform.GetChild(1).GetComponent<RectTransform>();

        float spacing = (Count > 1) ? TotalWidth / (Count - 1) : 0f;
        Vector2 center = SelfRect.anchoredPosition;
        for (int i = 0; i < Count; i++)
        {
            GameObject obj = Instantiate(UIPrefab, SelfRect);
            RectTransform rt = obj.GetComponent<RectTransform>();

            float x = -TotalWidth / 2f + i * spacing;
            rt.anchoredPosition = new Vector2(x, 0f);

            // 1�Ԗڈȍ~�ɑ}���i������0�̌�ɓ����j
            rt.SetSiblingIndex(1 + i);

            UIList.Add(rt);
        }


    }

    public void SetArrayNumber(int arrayNum)
    {
        PlayerUIRect.anchoredPosition = UIList[arrayNum].anchoredPosition;
    }
}
