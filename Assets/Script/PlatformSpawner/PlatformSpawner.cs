using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Cysharp.Threading.Tasks;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private PlatformSpawnerSettingsSO _settings;

    [Header("References")]
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _destroyPoint;

    private bool _isSpawning = false;

    // MonoBehaviour METHODS
    private void OnValidate()
    {
        if (_settings == null)
            Debug.LogError("PlatformSpawner: _settings null!");

        if (_spawnPoint == null)
            Debug.LogError("PlatformSpawner: _spawnPoint null!");

        if (_destroyPoint == null)
            Debug.LogError("PlatformSpawner: _destroyPoint null!");
    }

    private void Start()
    {
        StartSpawnLoopAsync().Forget(); // Forget means fire and forget, no need to await
    }

    // PUBLIC METHODS
    public void StopSpawning()
    {
        _isSpawning = false;
    }

    // PRIVATE METHODS
    private async UniTaskVoid StartSpawnLoopAsync()
    {
        _isSpawning = true;
        while (_isSpawning)
        {
            SpawnRandomPlatform();

            int spawnFrequencyMiliseconds = Mathf.CeilToInt(_settings.GetRandomSpawnFrequency() * 1000);
            await UniTask.Delay(spawnFrequencyMiliseconds);
        }
    }

    private void SpawnRandomPlatform()
    {
        MovingPlatform platformPrefab = _settings.GetRandomPlatformPrefab();

        // Use object pool.... later on if have time😂
        MovingPlatform platformInstance = Instantiate(platformPrefab);

        platformInstance.transform.SetParent(this.transform);
        platformInstance.transform.position = _spawnPoint.position;
        platformInstance.transform.rotation = Quaternion.identity;

        platformInstance.SetSettings(_settings);
        platformInstance.SetDestroyPosition(_destroyPoint.position);
        platformInstance.StartMoving();
    }
}
