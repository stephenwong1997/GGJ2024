using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    private static RespawnManager _instance;

    [Header("External References")]
    [SerializeField] private List<Transform> _targetTransforms;

    [Header("Internal References")]
    [SerializeField] private Transform _respawnPoint;
    [SerializeField] private Transform _leftEdge;
    [SerializeField] private Transform _rightEdge;
    [SerializeField] private Transform _topEdge;
    [SerializeField] private Transform _bottomEdge;

    private void OnValidate()
    {
        if (_respawnPoint == null) Debug.LogError("PlatformSpawner: _respawnPoint null!", this.gameObject);
        if (_leftEdge == null) Debug.LogError("PlatformSpawner: _leftEdge null!", this.gameObject);
        if (_rightEdge == null) Debug.LogError("PlatformSpawner: _rightEdge null!", this.gameObject);
        if (_topEdge == null) Debug.LogError("PlatformSpawner: _topEdge null!", this.gameObject);
        if (_bottomEdge == null) Debug.LogError("PlatformSpawner: _bottomEdge null!", this.gameObject);
    }

    private void Awake()
    {
        _instance = this;
    }

    private void LateUpdate()
    {
        // Placed in late update because FixedUpdate / Update may move the players
        TryRespawnPlayers();
    }

    // PUBLIC METHODS
    public static void AddToTargetTransforms(Transform t)
    {
        _instance._targetTransforms.Add(t);
    }

    // PRIVATE METHODS
    private void TryRespawnPlayers()
    {
        foreach (Transform targetTransform in _targetTransforms)
        {
            if (targetTransform == null)
                continue;

            if (
                targetTransform.position.x < _leftEdge.position.x ||
                targetTransform.position.x > _rightEdge.position.x ||
                targetTransform.position.y < _bottomEdge.position.y ||
                targetTransform.position.y > _topEdge.position.y
            )
            {
                Debug.Log($"Respawning {targetTransform.gameObject.name} to {_respawnPoint.position}...");
                targetTransform.position = _respawnPoint.position;
            }
        }
    }
}
