using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "PlatformSpawnerSettingsSO", menuName = "ScriptableObjects/PlatformSpawnerSettingsSO")]
public class PlatformSpawnerSettingsSO : ScriptableObject
{
    [Header("General Settings")]
    public float MinSpawnFrequency;
    public float MaxSpawnFrequency;
    public List<MovingPlatform> PlatformPrefabs;

    [Header("Individual Platform Settings")]
    public float PlatformSpeed;

    private void OnValidate()
    {
        if (MaxSpawnFrequency < MinSpawnFrequency)
            Debug.LogError("MaxSpawnFrequency < MinSpawnFrequency!");
    }

    public MovingPlatform GetRandomPlatformPrefab()
    {
        Assert.IsTrue(PlatformPrefabs.Count > 0);

        int randomIndex = Random.Range(0, PlatformPrefabs.Count);
        return PlatformPrefabs[randomIndex];
    }

    public float GetRandomSpawnFrequency()
    {
        return Random.Range(MinSpawnFrequency, MaxSpawnFrequency);
    }
}