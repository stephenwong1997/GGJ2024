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

    private bool _isSpawning = false;

    // MonoBehaviour METHODS
    private void OnValidate()
    {
        Assert.IsNotNull(_settings);
        Assert.IsNotNull(_spawnPoint);
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
            Debug.Log("Spawn!");

            // Calculate in every iteration so that can have quick update from settings SO
            int spawnFrequencyMiliseconds = Mathf.CeilToInt(_settings.SpawnFrequency * 1000);

            await UniTask.Delay(spawnFrequencyMiliseconds);
        }
    }
}
