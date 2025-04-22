
//======================================================
// FlyingPointスクリプト
// 作成者：藤本
// 最終更新日：4/22
//
// [Log]
// 4/22 藤本　飛距離に応じたポイント計算実装
//======================================================
using System.Collections.Generic;
using UnityEngine;

public class FlyingPoint : MonoBehaviour
{
    // 飛ばしたプレイヤー（ポイントを加算する対象）
    public Transform Shooter;

    // 発射地点（飛距離の基準になる位置）
    private Vector3 StartPos;

    // 現在空中にいるかどうか（スコア処理が1回だけ行われるようにする）
    private bool IsFlying = false;

    // Rigidbody（速度などの取得に使用）
    private Rigidbody Rb;

    // 速度がこの値未満になったら「停止した」とみなす（= 着地判定）
    public float StopThreshold = 0.1f;

    // 判定を行うまでの猶予時間（着地のブレ防止のため）
    public float CheckDelay = 0.5f;

    // 経過時間の記録用（CheckDelay経過後に停止判定をする）
    private float CheckTimer = 0f;

    [System.Serializable]
    public class DistanceScorePair
    {
        public float DistanceThreshold; // この距離未満で、
        public int Score;              // このスコアを与える
    }

    [Header("距離に応じたスコア設定（昇順に並べてください）")]
    public List<DistanceScorePair> ScoreTable = new List<DistanceScorePair>()
    {
        new DistanceScorePair(){ DistanceThreshold = 50f, Score = 100 },
        new DistanceScorePair(){ DistanceThreshold = 150f, Score = 200 },
        new DistanceScorePair(){ DistanceThreshold = 300f, Score = 300 },
        new DistanceScorePair(){ DistanceThreshold = 500f, Score = 400 },
        new DistanceScorePair(){ DistanceThreshold = float.MaxValue, Score = 500 }
    };

    void Start()
    {
        // Rigidbodyを取得
        Rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 飛行中のみ処理を行う
        if (IsFlying)
        {
            // 経過時間を加算
            CheckTimer += Time.deltaTime;

            // 判定可能な時間を過ぎ、速度が閾値以下になったらスコア加算処理
            if (CheckTimer > CheckDelay && Rb.linearVelocity.magnitude < StopThreshold)
            {
                IsFlying = false; // 再発を防ぐ

                // 発射位置との距離を測定
                float Distance = Vector3.Distance(StartPos, transform.position);

                // 飛距離に応じたポイントを計算
                int Points = CalculatePoints(Distance);

                // シューターが有効ならスコア加算
                if (Shooter != null)
                {
                    PlayerScore PlayerScore = Shooter.GetComponent<PlayerScore>();
                    if (PlayerScore != null)
                    {
                        PlayerScore.AddScore(Points);

                        // デバッグ表示
                        Debug.Log($"{Shooter.name} が {Distance:F2}m 飛ばして {Points} ポイント獲得！");
                    }
                }
            }
        }
    }

    // Playerで呼び出すことで「飛ばされた」ことを登録する
    public void Launch(Transform shooter)
    {
        Shooter = shooter;               // 飛ばしたプレイヤーを記録
        StartPos = transform.position;   // 現在位置を発射位置として記録
        IsFlying = true;                 // 飛行中に設定
        CheckTimer = 0f;                 // タイマー初期化
    }

    // 飛距離に応じたポイントを返す（5段階制）
    int CalculatePoints(float distance)
    {
        foreach (var pair in ScoreTable)
        {
            if (distance < pair.DistanceThreshold)
            {
                return pair.Score;
            }
        }

        return 0;
    }
}
