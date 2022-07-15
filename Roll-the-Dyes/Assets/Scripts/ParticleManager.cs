using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] pSystems = null;

    public static Action<int, Vector3, Vector3> SpawnParticles;

    private void OnEnable() { SpawnParticles += OnSpawnParticles; }
    private void OnDisable() { SpawnParticles -= OnSpawnParticles; }

    private void OnSpawnParticles(int particleTypeIndex, Vector3 spawnPos, Vector3 spawnNormal)
    {
        var spawnedParticles = Instantiate(pSystems[particleTypeIndex], spawnPos, Quaternion.identity);
        spawnedParticles.transform.up = spawnNormal;
    }
}
