//====================================================
// スクリプト名：SnackHeightUIManager
// 作成者：高下
// 内容：スナックの地面までの距離をUIで管理するスクリプト
// 最終更新日：05/14
// 
// [Log]
// 05/14 高下 スクリプト作成
// 05/15 高下 ポインター表示のときの高さ上限値を超えられるように仮で実装
//====================================================

// ******* このスクリプトの使い方 ******* //
// 1.SnackObjectにスナックを必ず設定する
// 2.プレハブ化しているので、Canvasに付けてください

using UnityEngine;
using UnityEngine.UI;

public class SnackHeightUIManager_Ver2 : MonoBehaviour
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
    private GameObject[] PointerObject;  // ポインターオブジェクト
    private GameObject[] IndicatorObject;
    [SerializeField] private GameObject BarObject;      // バーオブジェクト
    [SerializeField] private GameObject TextObject;     // テキストオブジェクト
    [SerializeField] private GameObject SnackObject;    // スナックオブジェクト
    private GameObject[] SnackObjects;
    [SerializeField] private float MaxHeight = 500.0f;  // ゲージ最大時の高さ
    [SerializeField] private float AddLimitUnlock = 0f; // ポインターの上限解放の加算値
    [SerializeField] private float ExtraYMargin = 30f;
    [SerializeField] private bool IsTextVisible = true; // テキストを表示するかどうか
    [SerializeField] private GameObject OriginalSnackPointer;
    [SerializeField] private GameObject OriginalOffscreenIndicator;


   [Header("表示関連")]
    [SerializeField] private GaugeDisplayMethod DisplayMethod = GaugeDisplayMethod.AlwaysVisible;
    [SerializeField] private GaugeDisplayType DisplayType = GaugeDisplayType.Bar;

    private Vector3 GroundPoint = new Vector3(0f, 0f, 0f); // 地面の座標
    private Vector3 SnackPoint = new Vector3(0f, 0f, 0f);  // スナックの座標
    private float SnackOffsetY = 0f;                       // スナックの半径分
    private Rigidbody SnackRb;                             // スナックのRigidbody
    private bool IsGround = false;                         // スナックが地面に接しているかどうか
    private FallPointCalculator[] FPCalculator;              // FallPointCalculatorコンポーネント
    private float PointMinY = 0f;                          // 最小Y位置（ポイント表示のときに使用）
    private float PointMaxY = 200f;                        // 最大Y位置（ポイント表示のときに使用）
    private RectTransform MeterRect;                       // メーターのRectTransform
    private RectTransform[] PointerRect;                     // ポインターのRectTransform
    private RectTransform[] IndicatorRect;
    private Image BarImage;                                // バー画像
    private GameObject PointerImage;                       // ポインター画像
    private GameObject DisplayObject;                      // 実際に表示させるバーかポインターのオブジェクトを入れておく
    private Text DistanceToGroundText;                     // 地面までの距離のテキスト
    private float DistanceToGround = 0f;                   // 地面までの距離
    private bool IsObjectCurrentlyActive = false;          // 現在UIオブジェクトがアクティブかどうか
    private bool HasStartedRising = true;                  // 一度でも上昇が始まったかどうか
    private AllSnackManager ASM;
    private SnackDuplicator SD;
    private int CurrentPointerCount = 0;
    private Vector3[] corners = new Vector3[4];

    void Start()
    {
        if (!OriginalSnackPointer) Debug.LogError("OriginalSnackPointerに「SnackPointer」プレハブがアタッチされていません");

        SD = Object.FindFirstObjectByType<SnackDuplicator>();
        if (SD)
        {
            PointerObject = new GameObject[SD.GetMaxSnackCount()];
            PointerRect = new RectTransform[SD.GetMaxSnackCount()];
            IndicatorObject = new GameObject[SD.GetMaxSnackCount()];
            IndicatorRect = new RectTransform[SD.GetMaxSnackCount()];
            SnackObjects = new GameObject[SD.GetMaxSnackCount()];
            FPCalculator = new FallPointCalculator[SD.GetMaxSnackCount()];
        }
        else
        {
            PointerObject = new GameObject[1];
            PointerRect = new RectTransform[1];
            IndicatorObject = new GameObject[1];
            IndicatorRect = new RectTransform[1];
            SnackObjects = new GameObject[1];
            FPCalculator = new FallPointCalculator[1];
        }

        SnackObjects[0] = SnackObject;
        FPCalculator[0] = SnackObject.GetComponent<FallPointCalculator>();
        CurrentPointerCount = 1;

        float OffsetX = 0f;

        for (int i = 0; i < IndicatorRect.Length; i++)
        {
            IndicatorObject[i] = Instantiate(OriginalOffscreenIndicator, new Vector3(0f, 0f, 0f), Quaternion.identity, gameObject.transform);
            IndicatorObject[i].SetActive(false);
            IndicatorRect[i] = IndicatorObject[i].GetComponent<RectTransform>();
            IndicatorRect[i].anchoredPosition3D = new Vector3(OffsetX, 630f, 0f);
            IndicatorRect[i].localRotation = Quaternion.identity;
            IndicatorRect[i].localScale = Vector3.one;
            OffsetX += 20.0f;
        }

        OffsetX = 0f;
        for (int i = 0; i < PointerRect.Length; i++)
        {
            PointerObject[i] = Instantiate(OriginalSnackPointer, new Vector3(0f, 0f, 0f), Quaternion.identity, gameObject.transform);
            PointerObject[i].SetActive(false);
            PointerRect[i] = PointerObject[i].GetComponent<RectTransform>();
            PointerRect[i].anchoredPosition3D = new Vector3(OffsetX, 0f, 0f);
            PointerRect[i].localRotation = Quaternion.identity;
            PointerRect[i].localScale = Vector3.one;
            OffsetX += 20.0f;
        }

        PointerObject[0].SetActive(true);
        IndicatorObject[0].SetActive(true);
        BarObject.SetActive(false);
        MeterObject.SetActive(false);
        TextObject.SetActive(false);

        SnackOffsetY = SnackObjects[0].GetComponent<Collider>().bounds.extents.y;
        SnackRb = SnackObjects[0].GetComponent<Rigidbody>();

        BarImage = BarObject.GetComponent<Image>();
        MeterRect = MeterObject.GetComponent<RectTransform>();

        DistanceToGroundText = TextObject.GetComponent<Text>();

        switch (DisplayType)
        {
            case GaugeDisplayType.Bar: // バー表示の場合
                // 表示オブジェクトの設定
                DisplayObject = BarObject;

                break;
            case GaugeDisplayType.Pointer: // ポインター表示の場合
                // 表示オブジェクトの設定
                DisplayObject = PointerObject[0];

                // メーターの上下の高さを計算する
                PointMaxY = MeterRect.anchoredPosition.y + (MeterRect.rect.height / 2.0f) - (PointerRect[0].rect.height / 2.0f);
                PointMaxY += AddLimitUnlock;
                PointMinY = MeterRect.anchoredPosition.y - (MeterRect.rect.height / 2.0f) + (PointerRect[0].rect.height / 2.0f);

                break;
        }
        SetObjectActive(true);
        ASM = Object.FindFirstObjectByType<AllSnackManager>();

    }

    void Update()
    {
        //if (!SnackObject) return;

        if (IsTextVisible)
        {
            // 地面までの距離を計算
            if (ASM)
            {
                DistanceToGround = ASM.GetDistanceToGround();
                if (DistanceToGround < 0f) DistanceToGround = 0f;
            }


            // 数値をテキストに反映
            DistanceToGroundText.text = Mathf.FloorToInt(DistanceToGround).ToString() + "m";
        }

        switch (DisplayType)
        {
            case GaugeDisplayType.Bar: // バー表示の場合
                //BarImage.fillAmount = Mathf.Clamp01(HeightRatio);

                break;
            case GaugeDisplayType.Pointer: // ポインター表示の場合

                for (int i = 0; i < CurrentPointerCount; i++)
                {
                    // 地面に着いているかどうかを取得
                    IsGround = FPCalculator[i].GetIsGround();

                    // 落下地点の座標を取得
                    GroundPoint = FPCalculator[i].GetFallPoint();

                    // 現在の高さを割合で取得
                    float HeightRatio = ((SnackObjects[i].transform.position.y - SnackOffsetY) - GroundPoint.y) / (MaxHeight - GroundPoint.y);

                    Vector3 TempRectPosition = PointerRect[i].anchoredPosition;

                    // Y座標を割合に応じて変化させる
                    TempRectPosition.y = Mathf.Lerp(PointMinY, PointMaxY, HeightRatio);
                    PointerRect[i].anchoredPosition = TempRectPosition;

                    bool isCompletelyOutOfScreen = true;

                    PointerRect[i].GetWorldCorners(corners);

                    foreach (Vector3 corner in corners)
                    {
                        Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(null, corner);
                        if (screenPoint.x >= 0 && screenPoint.x <= Screen.width &&
                            screenPoint.y >= 0 && screenPoint.y <= Screen.height + ExtraYMargin)
                        {
                            isCompletelyOutOfScreen = false; // 少しでも画面内に入っている
                        }
                    }

                    if (isCompletelyOutOfScreen)
                    {
                        IndicatorObject[i].SetActive(true);
                    }
                    else
                    {
                        IndicatorObject[i].SetActive(false);
                    }
                }
                break;
        }

        bool ShouldBeVisible = true; // デフォルトの表示状態

        switch (DisplayMethod)
        {
            case GaugeDisplayMethod.AlwaysVisible:
                ShouldBeVisible = true;
                break;

            case GaugeDisplayMethod.RisingStarted:
                //// 上昇開始検出
                //if (!HasStartedRising && !IsGround && SnackRb.linearVelocity.y > 1f)
                //{
                //    HasStartedRising = true;
                //}

                //// 地面に着いたらリセット
                //if (IsGround)
                //{
                //    HasStartedRising = false;
                //}

                //// フラグが立っている間は表示
                //ShouldBeVisible = HasStartedRising;
                break;

            case GaugeDisplayMethod.FallingStarted:
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
        if (IsTextVisible)
        {
            TextObject.SetActive(isActive);
        }
    }

    public void SetSnackObject(GameObject snack)
    {
        if (CurrentPointerCount >= PointerObject.Length) return;

        SnackObjects[CurrentPointerCount] = snack;
        FPCalculator[CurrentPointerCount] = snack.GetComponent<FallPointCalculator>();
        PointerObject[CurrentPointerCount].SetActive(true);
        CurrentPointerCount++;
    }
}