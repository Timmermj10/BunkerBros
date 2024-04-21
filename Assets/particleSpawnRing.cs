using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleSpawnRing : MonoBehaviour
{
    public ParticleSystem particleSystemPrefab; // Reference to your Particle System prefab
    public int numParticles = 10; // Number of particles to spawn in a circle
    public float circleRadius = 1f; // Radius of the circle

    private ParticleSystem[] particles;

    void Start()
    {
        particles = new ParticleSystem[numParticles];
        SpawnParticlesInCircle();
    }

    void SpawnParticlesInCircle()
    {
        float angleIncrement = 360f / numParticles;
        for (int i = 0; i < numParticles; i++)
        {
            float angle = i * angleIncrement;
            Vector3 spawnPosition = transform.position + Quaternion.Euler(0, angle, 0) * (Vector3.forward * circleRadius);
            particles[i] = Instantiate(particleSystemPrefab, spawnPosition, Quaternion.identity, transform);
            particles[i].Play();
        }
    }
}
