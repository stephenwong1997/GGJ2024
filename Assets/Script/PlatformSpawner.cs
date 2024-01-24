using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Cysharp.Threading.Tasks;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField][Tooltip("Unit: Seconds")] private float _spawnFrequency;

    [Header("References")]
    [SerializeField] private Transform _spawnPoint;

    private bool _isSpawning = false;

    // MonoBehaviour METHODS
    private void OnValidate()
    {
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

        int spawnFrequencyMiliseconds = Mathf.CeilToInt(_spawnFrequency * 1000);

        while (_isSpawning)
        {
            Debug.Log("Spawn!");
            await UniTask.Delay(spawnFrequencyMiliseconds);
        }
    }
}
