using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private bool _hasSomeoneCrossed;

    private void Awake()
    {
        _hasSomeoneCrossed = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_hasSomeoneCrossed)
            return;

        var playerMovement = col.GetComponentInChildren<PlayerMovement>();
        if (playerMovement == null)
            return;

        // Prevent multiple people triggering
        _hasSomeoneCrossed = true;

        Debug.Log($"FinishLine: OnTriggerEnter2D: {col.gameObject.name}, Is Chicken: {playerMovement.IsChicken}");
        FinishGameSequenceAsync(playerMovement).Forget();
    }

    private async UniTaskVoid FinishGameSequenceAsync(PlayerMovement playerMovement)
    {
        Time.timeScale = 0;

        await UniTask.Delay(3000, ignoreTimeScale: true);

        var cameraPosition = Camera.main.transform.position;
        cameraPosition.x = playerMovement.transform.position.x;
        cameraPosition.y = playerMovement.transform.position.y;

        Camera.main.transform.position = cameraPosition;
        Camera.main.orthographicSize = 1.2f;
    }
}