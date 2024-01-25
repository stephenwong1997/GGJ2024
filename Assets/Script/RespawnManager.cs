using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
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
        if (_respawnPoint == null) Debug.LogError("PlatformSpawner: _respawnPoint null!");
        if (_leftEdge == null) Debug.LogError("PlatformSpawner: _leftEdge null!");
        if (_rightEdge == null) Debug.LogError("PlatformSpawner: _rightEdge null!");
        if (_topEdge == null) Debug.LogError("PlatformSpawner: _topEdge null!");
        if (_bottomEdge == null) Debug.LogError("PlatformSpawner: _bottomEdge null!");
    }

    private void LateUpdate()
    {
        foreach (Transform targetTransform in _targetTransforms)
        {
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
