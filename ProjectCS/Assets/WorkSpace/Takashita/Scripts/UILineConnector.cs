//====================================================
// スクリプト名：UILineConnector
// 作成者：高下
// 内容：現在選択中のステージをUIで表す
// 最終更新日：06/01
// 
// [Log]
// 06/01 高下 スクリプト作成
//====================================================
using UnityEngine;
using System.Collections.Generic;

// 横一列にUIを並べ、選択中のUIの位置にプレイヤーUIを移動させるUI制御クラス
public class UILineConnector : MonoBehaviour
{
    [SerializeField] private GameObject UIPrefab;     // UI要素のプレハブ（例：選択アイコンなど）
    [SerializeField] private int Count = 5;           // 生成するUIの個数
    [SerializeField] private float TotalWidth = 400f; // 並べるUI全体の横幅

    // 生成されたUIオブジェクトのRectTransformを保持
    private List<RectTransform> UIList = new List<RectTransform>();

    // UIの親となるCanvasのRectTransform（未使用だが拡張時に使える）
    private RectTransform CanvasRect;

    // 自身（このスクリプトがアタッチされたUIオブジェクト）のRectTransform
    private RectTransform SelfRect;

    // 線を描画するUI（横一列を視覚的に補助する用）
    private RectTransform LineRect;

    // プレイヤー位置などを示すUI（選択カーソル的なUI）
    private RectTransform PlayerUIRect;

    void Start()
    {
        // 自分自身のRectTransformを取得
        SelfRect = GetComponent<RectTransform>();

        // 子オブジェクト0番目を線用のRectTransformとみなして取得
        LineRect = transform.GetChild(0).GetComponent<RectTransform>();

        // 線の幅をUI全体の幅に合わせて調整
        Vector2 size = LineRect.sizeDelta;
        size.x = TotalWidth;
        LineRect.sizeDelta = size;

        // 子オブジェクト1番目を「プレイヤーUI」（選択カーソル）とみなして取得
        PlayerUIRect = transform.GetChild(1).GetComponent<RectTransform>();

        // 間隔を計算（Countが1以下の場合は0）
        float spacing = (Count > 1) ? TotalWidth / (Count - 1) : 0f;

        // 自身の中心位置を基準にUIを左右に並べる
        Vector2 center = SelfRect.anchoredPosition;

        for (int i = 0; i < Count; i++)
        {
            // UI要素を生成し、親を SelfRect に設定
            GameObject obj = Instantiate(UIPrefab, SelfRect);
            RectTransform rt = obj.GetComponent<RectTransform>();

            // x座標を等間隔に計算し配置（中央を0として左から右へ並べる）
            float x = -TotalWidth / 2f + i * spacing;
            rt.anchoredPosition = new Vector2(x, 0f);

            // UIの描画順を調整（線の直後に追加するため +1）
            rt.SetSiblingIndex(1 + i);

            // 管理用リストに追加
            UIList.Add(rt);
        }
    }

    // 指定されたインデックスの位置に「プレイヤーUI」を移動させる
    /// <param name="arrayNum">対象のUIインデックス（0～Count-1）</param>
    public void SetArrayNumber(int arrayNum)
    {
        // プレイヤーUIを、指定インデックスのUIの位置へ移動
        PlayerUIRect.anchoredPosition = UIList[arrayNum].anchoredPosition;
    }
}