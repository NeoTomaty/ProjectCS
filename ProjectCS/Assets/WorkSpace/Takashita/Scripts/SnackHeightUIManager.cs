//====================================================
// スクリプト名：SnackHeightUIManager
// 作成者：高下
// 内容：スナックの地面までの距離をUIで管理するスクリプト
// 最終更新日：06/25
// 
// [Log]
// 05/14 高下 スクリプト作成
// 05/15 高下 ポインター表示のときの高さ上限値を超えられるように仮で実装
// 06/25 荒井 複数のスナックに対応するように変更
//====================================================

// ******* このスクリプトの使い方 ******* //
// 1.SnackObjectにスナックを必ず設定する
// 2.プレハブ化しているので、Canvasに付けてください

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SnackHeightUIManager : MonoBehaviour
{
    // ゲージの表示方法を表す
    public enum GaugeDisplayMethod
    {
        [InspectorName("常に表示（Always Visible）")]
        AlwaysVisible,     // 常に表示
        [InspectorName("上昇が始まったとき（Rising Started）")]
        RisingStarted,     // 上昇が始まったとき
        [InspectorName("落下が始まったとき（Falling Started）")]
        FallingStarted     // 落下が始まったとき
    }

    // ゲージタイプ
    public enum GaugeDisplayType
    {
        [InspectorName("バー表示（Bar）")]
        Bar,       // バー表示
        [InspectorName("ポイント表示（Pointer）")]
        Pointer,   // ポイント表示
    }

    [SerializeField] private GameObject MeterObject;    // メーターオブジェクト
    [SerializeField] private GameObject PointerObject;  // ポインターオブジェクト
    [SerializeField] private GameObject BarObject;      // バーオブジェクト
    [SerializeField] private GameObject TextObject;     // テキストオブジェクト
    [SerializeField] private GameObject SnackObject;    // スナックオブジェクト
    [SerializeField] private float MaxHeight = 500.0f;  // ゲージ最大時の高さ
    [SerializeField] private float AddLimitUnlock = 0f; // ポインターの上限解放の加算値
    [SerializeField] private bool IsTextVisible = true; // テキストを表示するかどうか
   
    [Header("表示関連")]
    [SerializeField] private GaugeDisplayMethod DisplayMethod = GaugeDisplayMethod.AlwaysVisible;
    [SerializeField] private GaugeDisplayType DisplayType = GaugeDisplayType.Bar;

    private Vector3 GroundPoint = new Vector3(0f, 0f, 0f); // 地面の座標
    private Vector3 SnackPoint = new Vector3(0f, 0f, 0f);  // スナックの座標
    private float SnackOffsetY = 0f;                       // スナックの半径分
    private Rigidbody SnackRb;                             // スナックのRigidbody
    private bool IsGround = false;                         // スナックが地面に接しているかどうか
    private FallPointCalculator FPCalculator;              // FallPointCalculatorコンポーネント
    private float PointMinY = 0f;                          // 最小Y位置（ポイント表示のときに使用）
    private float PointMaxY = 200f;                        // 最大Y位置（ポイント表示のときに使用）
    private RectTransform MeterRect;                       // メーターのRectTransform
    private RectTransform PointerRect;                     // ポインターのRectTransform
    private Image BarImage;                                // バー画像
    private GameObject PointerImage;                       // ポインター画像
    private GameObject DisplayObject;                      // 実際に表示させるバーかポインターのオブジェクトを入れておく
    private Text DistanceToGroundText;                     // 地面までの距離のテキスト
    private float DistanceToGround = 0f;                   // 地面までの距離
    private bool IsObjectCurrentlyActive = false;          // 現在UIオブジェクトがアクティブかどうか
    private bool HasStartedRising = true;                  // 一度でも上昇が始まったかどうか

    private List<GameObject> SnackObjects = new List<GameObject>(); // スナックオブジェクトのリスト


    void Start()
    {
        BarObject.SetActive(false);
        PointerObject.SetActive(false);
        MeterObject.SetActive(false);
        TextObject.SetActive(false);

        if (!SnackObject)
        {
            Debug.LogError("SnackObjectが設定されていません");
            return;
        }
        
        SnackOffsetY = SnackObject.GetComponent<Collider>().bounds.extents.y;
        SnackRb = SnackObject.GetComponent<Rigidbody>();
        FPCalculator = SnackObject.GetComponent<FallPointCalculator>();
        BarImage = BarObject.GetComponent<Image>();
        MeterRect = MeterObject.GetComponent<RectTransform>();
        PointerRect = PointerObject.GetComponent<RectTransform>();
        DistanceToGroundText = TextObject.GetComponent<Text>();

        if (SnackObject != null)
        {
            SnackObjects.Add(SnackObject);
        }

        switch (DisplayType)
        {
            case GaugeDisplayType.Bar: // バー表示の場合
                // 表示オブジェクトの設定
                DisplayObject = BarObject;

                break;
            case GaugeDisplayType.Pointer: // ポインター表示の場合
                // 表示オブジェクトの設定
                DisplayObject = PointerObject;

                // メーターの上下の高さを計算する
                PointMaxY = MeterRect.anchoredPosition.y + (MeterRect.rect.height / 2.0f) - (PointerRect.rect.height / 2.0f);
                PointMaxY += AddLimitUnlock;
                PointMinY = MeterRect.anchoredPosition.y - (MeterRect.rect.height / 2.0f) + (PointerRect.rect.height / 2.0f);

                break;
        }
        SetObjectActive(true);
    }

    void Update()
    {
        if (SnackObjects.Count == 0) return;
        //if (!SnackObject) return;

        float SnackHeight = 100000f;
        bool IsSnackGround = true;
        float SnackVelocityY = 0f;
        foreach (var Snack in SnackObjects)
        {
            // 地面に着いていない
            if (!Snack.GetComponent<FallPointCalculator>().GetIsGround())
            {
                IsSnackGround = false; // 地面に着いてないスナックがある
                // より低い位置のスナック
                if (Snack.transform.position.y < SnackHeight)
                {
                    SnackHeight = Snack.transform.position.y;
                    SnackVelocityY = Snack.GetComponent<Rigidbody>().linearVelocity.y;
                }
            }
        }

        if (IsTextVisible)
        {
            // 地面までの距離を計算
            DistanceToGround = Mathf.Max(0f, (SnackHeight - SnackOffsetY) - GroundPoint.y);
            //DistanceToGround = Mathf.Max(0f, (SnackObject.transform.position.y - SnackOffsetY) - GroundPoint.y);

            // 数値をテキストに反映
            DistanceToGroundText.text = Mathf.FloorToInt(DistanceToGround).ToString() + "m";
        }

        // 地面に着いているかどうかを取得
        IsGround = FPCalculator.GetIsGround();

        // 落下地点の座標を取得
        GroundPoint = FPCalculator.GetFallPoint();

        // 現在の高さを割合で取得
        float HeightRatio = ((SnackHeight - SnackOffsetY) - GroundPoint.y) / (MaxHeight - GroundPoint.y);
        //float HeightRatio = ((SnackObject.transform.position.y - SnackOffsetY) - GroundPoint.y) / (MaxHeight - GroundPoint.y);

        switch (DisplayType)
        {
            case GaugeDisplayType.Bar: // バー表示の場合
                BarImage.fillAmount = Mathf.Clamp01(HeightRatio);

                break;
            case GaugeDisplayType.Pointer: // ポインター表示の場合

                Vector3 TempRectPosition = PointerRect.anchoredPosition;

                // Y座標を割合に応じて変化させる
                TempRectPosition.y = Mathf.Lerp(PointMinY, PointMaxY, HeightRatio);
                PointerRect.anchoredPosition = TempRectPosition;

                break;
        }

        bool ShouldBeVisible = true; // デフォルトの表示状態

        switch (DisplayMethod)
        {
            case GaugeDisplayMethod.AlwaysVisible:
                ShouldBeVisible = true;
                break;

            case GaugeDisplayMethod.RisingStarted:
                // 上昇開始検出
                if (!HasStartedRising && !IsSnackGround && SnackVelocityY > 1f)
                //if (!HasStartedRising && !IsGround && SnackRb.linearVelocity.y > 1f)
                {
                        HasStartedRising = true;
                }

                // 地面に着いたらリセット
                if (IsSnackGround)
                {
                    HasStartedRising = false;
                }

                // フラグが立っている間は表示
                ShouldBeVisible = HasStartedRising;
                break;

            case GaugeDisplayMethod.FallingStarted:
                ShouldBeVisible = !IsSnackGround && SnackVelocityY < 1f;
                //ShouldBeVisible = !IsGround && SnackRb.linearVelocity.y < 1f;

                break;
        }

        // 表示状態が変わったときだけSetObjectActiveを呼ぶ
        if (IsObjectCurrentlyActive != ShouldBeVisible)
        {
            SetObjectActive(ShouldBeVisible);
            IsObjectCurrentlyActive = ShouldBeVisible;
        }

    }

    // オブジェクトの表示状態を切り替える
    private void SetObjectActive(bool isActive)
    {
        MeterObject.SetActive(isActive);
        DisplayObject.SetActive(isActive);
        if(IsTextVisible)
        {
            TextObject.SetActive(isActive);
        }
    }

    public void AddSnack(GameObject snackClone)
    {
        if (snackClone == null)
        {
            return;
        }

        // 要素被りチェック
        if (SnackObjects.Contains(snackClone)) return;

        // スナックの新たなクローンをリストに追加
        SnackObjects.Add(snackClone);
    }
}