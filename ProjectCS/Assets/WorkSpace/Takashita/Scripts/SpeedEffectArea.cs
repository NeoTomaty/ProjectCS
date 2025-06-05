//====================================================
// スクリプト名：SpeedEffectArea
// 作成者：高下
// 内容：プレイヤーが一時的に減速するエリアを制御するクラス
// 最終更新日：06/04
// 
// [Log]
// 05/08 高下 スクリプト作成 
// 06/04 中町 減速SE実装
// 06/05 竹内 減速処理の仕様を変更（タイプB追加）
// 06/05 竹内 加速処理の仕様を追加（タイプC追加）
// 06/05 竹内 SpeedEffectAreaに改名
//====================================================
using UnityEngine;
// ※このスクリプトをアタッチしている減速エリアはプレハブで作ってます

public enum SpeedEffectType
{
    TypeA, // 一時減速 （減速後に速度は戻る）
    TypeB, // 継続減速 （減速後に速度は戻らない）
    TypeC  // 加速     （減速後に速度は戻らない）
}

public class SpeedEffectArea : MonoBehaviour
{
    [SerializeField] private GameObject Player; // プレイヤーオブジェクト

    [Header("減速率（0.0～1.0で設定）")]
    [SerializeField] private float DecelerationRatio = 0.1f;
    
    //減速の基準を最大速度にするかどうか
    [Header("true:最大速度から減速値を決定 false:現在の速度から減速値を決定")]
    [SerializeField] private bool IsDecelerationBasedOnMaxSpeed = true;

    [Header("速度を補完する時間（秒）")]
    [SerializeField] private float InterpolationDuration = 1.0f;

    //減速時に鳴らすサウンドエフェクト
    [Header("SE設定")]
    [Tooltip("減速時に再生するSE")]
    [SerializeField] private AudioClip DecelerationSE;

    [Header("使用する加減速タイプ A:一時減速 B:減速 C:加速")]
    [SerializeField] private SpeedEffectType effectType = SpeedEffectType.TypeA;

    [Header("どれくらいの間隔で加減速するのか")]
    [SerializeField] private float TickInterval = 0.5f;         // B/C用：定期的に加減速する間隔

    [Header("どれくらい加減速するのか")]
    [SerializeField] private float SpeedAmount = 0.5f;   // B/C用：1回あたりの変化量

    private float tickTimer = 0f;           // B/C用：経過時間記録
    private bool isPlayerInside = false;    // B/C用：エリア内判定

    //SE再生用のAudioSource
    private AudioSource audioSource;

    private PlayerSpeedManager SpeedManager; // プレイヤーのPlayerSpeedManagerコンポーネント
    private float TempPlayerSpeed = 0f;      // 減速前の速度
    private float StartSpeed = 0f;           // 補完開始時の速度
    private float TargetSpeed = 0f;          // 補完終了時の目標速度
    private float InterpolationTimer = 0f;   // 補完中の経過時間
    private bool IsInterpolating = false;    // 補完中かどうか

    void Start()
    {
        //プレイヤーが設定されていないときはエラーを出力
        if (!Player)
        {
            Debug.LogError("プレイヤーオブジェクトが設定されていません");
            return;
        }

        //プレイヤーの速度管理コンポーネントを取得
        SpeedManager = Player.GetComponent<PlayerSpeedManager>();

        //AudioSourceをこのオブジェクトに追加し、SE再生用に設定
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {

        // Aタイプの補完処理
        if (effectType == SpeedEffectType.TypeA && IsInterpolating)
        {
            InterpolationTimer += Time.deltaTime;
            float t = Mathf.Clamp01(InterpolationTimer / InterpolationDuration);
            float newSpeed = Mathf.Lerp(StartSpeed, TargetSpeed, t);
            SpeedManager.SetSpeed(newSpeed);

            if (t >= 1.0f)
                IsInterpolating = false;
        }

        // B/CタイプのTick処理
        if ((effectType == SpeedEffectType.TypeB || effectType == SpeedEffectType.TypeC) && isPlayerInside)
        {
            tickTimer += Time.deltaTime;
            if (tickTimer >= TickInterval)
            {
                tickTimer = 0f;

                float currentSpeed = SpeedManager.GetPlayerSpeed;
                float newSpeed = currentSpeed;

                if (effectType == SpeedEffectType.TypeB)
                    newSpeed = Mathf.Max(SpeedManager.GetMinSpeed(), currentSpeed - SpeedAmount);
                else if (effectType == SpeedEffectType.TypeC)
                    newSpeed = Mathf.Min(SpeedManager.GetMaxSpeed(), currentSpeed + SpeedAmount);

                SpeedManager.SetSpeed(newSpeed);
            }
        }
    }


    // プレイヤーがエリアに入ったときの処理
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            Debug.Log("エリアに入りました");

            // SEが設定されていれば再生
            if (DecelerationSE != null) audioSource.PlayOneShot(DecelerationSE);

            // プレイヤーがエリア内にいる状態に設定（B/C用の判定用）
            isPlayerInside = true;

            // Aタイプ
            if (effectType == SpeedEffectType.TypeA)
            {
                // すでに補完中ならその目標速度を一時保存、それ以外は現在の速度を保存
                TempPlayerSpeed = IsInterpolating ? TargetSpeed : SpeedManager.GetPlayerSpeed;

                // 補完の開始速度を記録
                StartSpeed = TempPlayerSpeed;

                // 減速の基準速度を決定（最大速度 or 現在速度）
                float baseSpeed = IsDecelerationBasedOnMaxSpeed ? SpeedManager.GetMaxSpeed() : TempPlayerSpeed;

                // 減速量を算出
                float decelerationValue = baseSpeed * DecelerationRatio;

                // 最小速度を下回らないように調整
                TargetSpeed = Mathf.Max(SpeedManager.GetMinSpeed(), TempPlayerSpeed - decelerationValue);

                // 補完処理の初期化
                InterpolationTimer = 0f;
                IsInterpolating = true;
            }
            else
            {
                // B/Cタイプ用：タイマー初期化
                tickTimer = 0f;
            }
        }
    }

    // プレイヤーがエリアから出たときの処理
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            Debug.Log("エリアから出ました");

            // プレイヤーがエリア外に出た状態にする（B/C用の判定用）
            isPlayerInside = false;

            // Aタイプ
            if (effectType == SpeedEffectType.TypeA)
            {
                // 補完の開始速度を現在の速度に設定
                StartSpeed = SpeedManager.GetPlayerSpeed;

                // 補完の目標速度をエリア侵入前の速度に設定
                TargetSpeed = TempPlayerSpeed;

                // 補完処理の初期化
                InterpolationTimer = 0f;
                IsInterpolating = true;
            }
        }
    }

}
