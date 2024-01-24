using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField][Tooltip("Unit: Seconds")] private float _spawnFrequency;

    [Header("References")]
    [SerializeField] private Transform _spawnPoint;

    private bool _isSpawning = false;

    private void OnValidate()
    {
        Assert.IsNotNull(_spawnPoint);
    }

    private void Start()
    {
        // TODO : Import UniTask
        // StartSpawnLoopAsync();
    }

    private async void StartSpawnLoopAsync()
    {
        _isSpawning = true;


        int spawnFrequencyMiliseconds = Mathf.CeilToInt(_spawnFrequency * 1000);

        while (_isSpawning)
        {
            Debug.Log("Spawn!");
            await Task.Delay(spawnFrequencyMiliseconds);
        }
    }
}
