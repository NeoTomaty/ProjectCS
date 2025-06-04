//====================================================
// スクリプト名：DecelerationArea
// 作成者：高下
// 内容：プレイヤーが一時的に減速するエリアを制御するクラス
// 最終更新日：05/08
// 
// [Log]
// 05/08 高下 スクリプト作成 
// 
//====================================================
using UnityEngine;
// ※このスクリプトをアタッチしている減速エリアはプレハブで作ってます

public class DecelerationArea : MonoBehaviour
{
    [SerializeField] private GameObject Player; // プレイヤーオブジェクト

    [Tooltip("減速率（0.0～1.0で設定）")]
    [SerializeField] private float DecelerationRatio = 0.1f;
    
    //減速の基準を最大速度にするかどうか
    [Tooltip("true:最大速度から減速値を決定 false:現在の速度から減速値を決定")]
    [SerializeField] private bool IsDecelerationBasedOnMaxSpeed = true;

    [Tooltip("速度を補完する時間（秒）")]
    [SerializeField] private float InterpolationDuration = 1.0f;

    //減速時に鳴らすサウンドエフェクト
    [Header("SE設定")]
    [Tooltip("減速時に再生するSE")]
    [SerializeField] private AudioClip DecelerationSE;

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
        // 補完中か
        if (IsInterpolating)
        {
            InterpolationTimer += Time.deltaTime;

            // 補完の進行度をクランプ
            float t = Mathf.Clamp01(InterpolationTimer / InterpolationDuration);

            // 次に設定する速度
            float newSpeed = Mathf.Lerp(StartSpeed, TargetSpeed, t);

            // 速度を反映
            SpeedManager.SetSpeed(newSpeed);

            if (t >= 1.0f)
            {
                IsInterpolating = false; // 補完終了
            }
        }
    }

    //プレイヤーが減速エリアに入ったときの処理
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            Debug.Log("減速エリアに入りました");

            //SEが設定されていれば再生
            if(DecelerationSE != null)
            {
                audioSource.PlayOneShot(DecelerationSE);
            }

            // 補完中なら目標速度を、そうでなければ現在のプレイヤー速度を一時保存
            TempPlayerSpeed = IsInterpolating ? TargetSpeed : SpeedManager.GetPlayerSpeed;

            // 開始速度
            StartSpeed = TempPlayerSpeed;

            // 減速値の基準速度を決定（trueなら最大速度、falseなら現在の速度）
            float baseSpeed = IsDecelerationBasedOnMaxSpeed ? SpeedManager.GetMaxSpeed() : TempPlayerSpeed;

            // 減速値計算
            float decelerationValue = baseSpeed * DecelerationRatio;

            // 目標速度が最小速度を下回らないように調整
            TargetSpeed = Mathf.Max(SpeedManager.GetMinSpeed(), TempPlayerSpeed - decelerationValue);

            //補完の初期化
            InterpolationTimer = 0f;
            IsInterpolating = true;
        }
    }

    //プレイヤーが減速エリアから出たときの処理
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            Debug.Log("減速エリアから出ました");

            // 開始速度
            StartSpeed = SpeedManager.GetPlayerSpeed;

            // エリアに入った時の速度を設定
            TargetSpeed = TempPlayerSpeed;            

            //補完の初期化
            InterpolationTimer = 0f;
            IsInterpolating = true;
        }
    }
}
