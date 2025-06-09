using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UILineConnector : MonoBehaviour
{
    [SerializeField] private GameObject UIPrefab;         // 生成するUIプレハブ
    [SerializeField] private int Count = 5;               // 生成する数
    [SerializeField] private float TotalWidth = 400f;     // 並べる横幅

    private List<RectTransform> UIList = new List<RectTransform>();
    private RectTransform CanvasRect;       // CanvasのRectTransform（親）
    private RectTransform SelfRect;         // 自分自身のRectTransform（起点位置）
    private RectTransform LineRect;
    private RectTransform PlayerUIRect;

    void Start()
    {
        SelfRect = GetComponent<RectTransform>();
        // 子オブジェクトから LineRect / PlayerUIRect を取得
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

            // 1番目以降に挿入（既存の0の後に入れる）
            rt.SetSiblingIndex(1 + i);

            UIList.Add(rt);
        }


    }

    public void SetArrayNumber(int arrayNum)
    {
        PlayerUIRect.anchoredPosition = UIList[arrayNum].anchoredPosition;
    }
}
