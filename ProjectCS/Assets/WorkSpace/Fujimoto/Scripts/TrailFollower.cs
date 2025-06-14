

using UnityEngine;
using System.Collections.Generic;
using Tiny;

[RequireComponent(typeof(Trail))]
public class TrailFollower : MonoBehaviour
{
    private Trail trail;
    public float pointInterval = 0.05f; // 何秒ごとに点を追加するか
    private float timer = 0f;

    void Start()
    {
        trail = GetComponent<Trail>();
        // Null防止：最初に空配列を代入
        if (trail.Points == null || trail.Points.Length == 0)
        {
            trail.Points = new Vector3[0];
        }
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= pointInterval)
        {
            // 現在の位置を trail に追加
            var points = new List<Vector3>(trail.Points);
            points.Add(transform.position);
            trail.Points = points.ToArray();
            timer = 0f;
        }
    }
}
