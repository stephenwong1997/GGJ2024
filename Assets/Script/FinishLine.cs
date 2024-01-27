using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

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

        const float DELAY_TIME = 1;
        await UniTask.Delay(Mathf.CeilToInt(DELAY_TIME * 1000), ignoreTimeScale: true);

        var cameraPosition = Camera.main.transform.position;
        cameraPosition.x = playerMovement.transform.position.x;
        cameraPosition.y = playerMovement.transform.position.y;

        const float TWEEN_DURATION = 0.5f;
        const float ORTHO_SIZE = 1.2f;

        Camera.main.transform.DOMove(cameraPosition, TWEEN_DURATION).SetUpdate(UpdateType.Normal, isIndependentUpdate: true);
        Camera.main.DOOrthoSize(ORTHO_SIZE, TWEEN_DURATION).SetUpdate(UpdateType.Normal, isIndependentUpdate: true);

        const float AFTER_ZOOM_DELAY_TIME = 2;
        await UniTask.Delay(Mathf.CeilToInt(AFTER_ZOOM_DELAY_TIME * 1000), ignoreTimeScale: true);

        if (playerMovement.IsChicken)
            GameManager.StartChickenWonSequence();
        else
            GameManager.StartEggWonSequence();
    }
}