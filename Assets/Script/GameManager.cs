using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public static bool IsGameReady => _instance._isGameReady;
    private static GameManager _instance;

    [Header("Settings")]
    [SerializeField] private bool _updateSettingsFromJoystick;

    [Header("References")]
    [SerializeField] private UIController _uiController;
    [SerializeField] private GameObject _qrCodeParent;
    [SerializeField] private PlatformSpawner _platformSpawner;

    private bool _isGameReady = false;

    // MonoBehaviour METHODS
    private void OnValidate()
    {
        if (_uiController == null) Debug.LogError($"GameManager: _uiController null!");
        if (_qrCodeParent == null) Debug.LogError($"GameManager: _qrCodeParent null!");
        if (_platformSpawner == null) Debug.LogError($"GameManager: _platformSpawner null!");
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        StartGameSequenceAsync().Forget();
    }

    private void OnDestroy()
    {
        _uiController.OnReadyClickedEvent -= StartGame;
    }

    // PRIVATE METHODS
    private async UniTaskVoid StartGameSequenceAsync()
    {
        _isGameReady = false;
        Debug.Log("GameManager: Starting game...");

        _qrCodeParent.SetActive(true);
        _platformSpawner.gameObject.SetActive(false);

        if (_updateSettingsFromJoystick)
        {
            await UpdateSettingsFromJoystickAsync();
        }

        _uiController.OnReadyClickedEvent += StartGame;

        _isGameReady = true;
        Debug.Log("GameManager: Game ready!");
    }

    private void StartGame()
    {
        _qrCodeParent.SetActive(false);
        _platformSpawner.gameObject.SetActive(true);
    }

    private async UniTask UpdateSettingsFromJoystickAsync()
    {
        Debug.Log("GameManager: Update settings from joystick...");

        await API.GetJoystickSettingsAsync();

        // Create a clone because not all settings need to be overriden
        PlatformSpawnerSettingsSO settingsClone = Instantiate(PlatformSpawner.Setting);

        // TODO : Override settings as needed
        settingsClone.TotalGameTime = 2f;

        _platformSpawner.OverrideSettings(settingsClone);

        await UniTask.Delay(5000); // TODO : Delete this later
        Debug.Log("GameManager: Updated settings!");
    }
}