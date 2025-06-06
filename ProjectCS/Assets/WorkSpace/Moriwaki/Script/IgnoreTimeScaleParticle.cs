using UnityEngine;

public class IgnoreTimeScaleParticle : MonoBehaviour
{
    private ParticleSystem ps;
    private float unscaledTimePrev;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ps.Play();
        unscaledTimePrev = Time.unscaledTime;
    }

    private void Update()
    {
        float deltaUnscaled = Time.unscaledTime - unscaledTimePrev;
        ps.Simulate(deltaUnscaled, true, false);
        ps.Play();
        unscaledTimePrev = Time.unscaledTime;
    }
}