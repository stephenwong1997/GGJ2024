using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public event Action OnReadyClickedEvent;

    [SerializeField] private GameObject _titleScreenParent;
    [SerializeField] private Button _titleScreenStartButton;

    [SerializeField] private GameObject _readyScreenParent;
    [SerializeField] private Button _readyButton;

    [SerializeField] private TextMeshProUGUI _chickenLives1;
    [SerializeField] private TextMeshProUGUI _chickenLives2;
    [SerializeField] private TextMeshProUGUI _eggLives1;
    [SerializeField] private TextMeshProUGUI _eggLives2;

    private void OnValidate()
    {
        if (_titleScreenParent == null) Debug.LogError($"UIController: _titleScreenParent null!");
        if (_titleScreenStartButton == null) Debug.LogError($"UIController: _titleScreenStartButton null!");

        if (_readyScreenParent == null) Debug.LogError($"UIController: _readyScreenParent null!");
        if (_readyButton == null) Debug.LogError($"UIController: _readyButton null!");
    }

    private void Start()
    {
        _titleScreenParent.SetActive(true);
        _readyScreenParent.SetActive(true);

        _chickenLives1.text = GameManager.ChickenLives.ToString();
        _chickenLives2.text = GameManager.ChickenLives.ToString();
        _eggLives1.text = GameManager.EggLives.ToString();
        _eggLives2.text = GameManager.EggLives.ToString();

        GameManager.OnChickenLivesChanged += (x) =>
        {
            _chickenLives1.text = GameManager.ChickenLives.ToString();
            _chickenLives2.text = GameManager.ChickenLives.ToString();
        };

        GameManager.OnEggLivesChanged += (x) =>
        {
            _eggLives1.text = GameManager.EggLives.ToString();
            _eggLives2.text = GameManager.EggLives.ToString();
        };

        _titleScreenStartButton.onClick.AddListener(OnStartClicked);
        _readyButton.onClick.AddListener(OnReadyClicked);
    }

    private void OnStartClicked()
    {
        if (!GameManager.IsGameReady)
            return;

        _titleScreenParent.SetActive(false);
    }

    private void OnReadyClicked()
    {
        _readyScreenParent.SetActive(false);
        OnReadyClickedEvent?.Invoke();
    }
}