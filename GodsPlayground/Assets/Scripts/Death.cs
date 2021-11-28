using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    private ParticleSystem deathParticles;
    private float startTime;
    public float emissionTime = 5.0f;
    public float deleteTime = 10.0f;

    public void Init(Color color)
    {
        deathParticles = transform.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule settings = deathParticles.main;
        settings.startColor = new ParticleSystem.MinMaxGradient(color);
        deathParticles.Play();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime >= emissionTime && deathParticles.isEmitting)
        {
            deathParticles.Stop();
        }
        if (Time.time - startTime >= deleteTime)
        {
            Destroy(gameObject);
        }
    }
}
