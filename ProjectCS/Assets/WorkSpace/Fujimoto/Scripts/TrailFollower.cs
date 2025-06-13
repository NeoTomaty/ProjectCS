

using UnityEngine;
using System.Collections.Generic;
using Tiny;

[RequireComponent(typeof(Trail))]
public class TrailFollower : MonoBehaviour
{
    private Trail trail;
    public float pointInterval = 0.05f; // ‰½•b‚²‚Æ‚É“_‚ğ’Ç‰Á‚·‚é‚©
    private float timer = 0f;

    void Start()
    {
        trail = GetComponent<Trail>();
        // Null–h~FÅ‰‚É‹ó”z—ñ‚ğ‘ã“ü
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
            // Œ»İ‚ÌˆÊ’u‚ğ trail ‚É’Ç‰Á
            var points = new List<Vector3>(trail.Points);
            points.Add(transform.position);
            trail.Points = points.ToArray();
            timer = 0f;
        }
    }
}
