using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIController _uiController;
    [SerializeField] private GameObject _qrCodeParent;
    [SerializeField] private PlatformSpawner _platformSpawner;

    // MonoBehaviour METHODS
    private void OnValidate()
    {
        if (_uiController == null) Debug.LogError($"GameManager: _uiController null!");
        if (_qrCodeParent == null) Debug.LogError($"GameManager: _qrCodeParent null!");
        if (_platformSpawner == null) Debug.LogError($"GameManager: _platformSpawner null!");
    }

    private void Start()
    {
        _qrCodeParent.SetActive(true);
        _platformSpawner.gameObject.SetActive(false);

        _uiController.OnReadyClickedEvent += StartGame;
    }

    private void OnDestroy()
    {
        _uiController.OnReadyClickedEvent -= StartGame;
    }

    // PRIVATE METHODS
    private void StartGame()
    {
        _qrCodeParent.SetActive(false);
        _platformSpawner.gameObject.SetActive(true);
    }
}