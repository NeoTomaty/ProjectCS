

using UnityEngine;
using System.Collections.Generic;
using Tiny;

[RequireComponent(typeof(Trail))]
public class TrailFollower : MonoBehaviour
{
    private Trail trail;
    public float pointInterval = 0.05f; // ���b���Ƃɓ_��ǉ����邩
    private float timer = 0f;

    void Start()
    {
        trail = GetComponent<Trail>();
        // Null�h�~�F�ŏ��ɋ�z�����
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
            // ���݂̈ʒu�� trail �ɒǉ�
            var points = new List<Vector3>(trail.Points);
            points.Add(transform.position);
            trail.Points = points.ToArray();
            timer = 0f;
        }
    }
}
