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

    [Header("Finish Line Settings")]
    public float TotalGameTime;
    public float SpawnFinishLineDelay;
    public MovingPlatform FinishLinePrefab;


    [Header("Individual Platform Settings")]
    public float PlatformSpeed;

    [Header("Items Settings")]
    [Range(0, 1)] public float ItemSpawnProbability;
    public List<FloatingItem> FloatingItemPrefabs;

    private void OnValidate()
    {
        if (MaxSpawnFrequency < MinSpawnFrequency)
            Debug.LogError("PlatformSpawnerSettingsSO: MaxSpawnFrequency < MinSpawnFrequency!");

        if (PlatformPrefabs.Count <= 0)
            Debug.LogError("PlatformSpawnerSettingsSO: No platform prefabs!");

        if (FloatingItemPrefabs.Count <= 0)
            Debug.LogError("PlatformSpawnerSettingsSO: No floating item prefabs!");

        if (TotalGameTime <= 0)
            Debug.LogError("PlatformSpawnerSettingsSO: TotalGameTime <= 0!");

        if (FinishLinePrefab == null)
            Debug.LogError("PlatformSpawnerSettingsSO: FinishLinePrefab null!");
    }

    public MovingPlatform GetRandomPlatformPrefab()
    {
        if (PlatformPrefabs.Count <= 0)
            throw new System.InvalidOperationException("PlatformSpawnerSettingsSO: No platform prefabs!");

        int randomIndex = Random.Range(0, PlatformPrefabs.Count);
        return PlatformPrefabs[randomIndex];
    }

    public float GetRandomSpawnFrequency()
    {
        return Random.Range(MinSpawnFrequency, MaxSpawnFrequency);
    }

    public FloatingItem GetRandomFloatingItemPrefab()
    {
        if (FloatingItemPrefabs.Count <= 0)
            throw new System.InvalidOperationException("PlatformSpawnerSettingsSO: No floating item prefabs!");

        int randomIndex = Random.Range(0, FloatingItemPrefabs.Count);
        return FloatingItemPrefabs[randomIndex];
    }
}