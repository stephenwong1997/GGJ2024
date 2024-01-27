using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    public event Action OnReadyClickedEvent;

    [Header("Screens")]

    [SerializeField] private GameObject _titleScreenParent;
    [SerializeField] private Button _titleScreenStartButton;

    [SerializeField] private GameObject _readyScreenParent;
    [SerializeField] private Button _readyButton;

    [Header("Lives")]

    [SerializeField] private TextMeshProUGUI _chickenLives1;
    [SerializeField] private TextMeshProUGUI _chickenLives2;
    [SerializeField] private TextMeshProUGUI _eggLives1;
    [SerializeField] private TextMeshProUGUI _eggLives2;

    [Header("Timer")]
    [SerializeField] private GameObject _timerTarget;
    [SerializeField] private GameObject _timerEndRef;

    [Header("Winning Images")]
    [SerializeField] private float _wonImageTweenDuration = 3f;
    [SerializeField] private Image _chickenWonImage;
    [SerializeField] private Image _eggWonImage;

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

        _chickenWonImage.gameObject.SetActive(false);
        _eggWonImage.gameObject.SetActive(false);

        _titleScreenStartButton.onClick.AddListener(OnStartClicked);
        _readyButton.onClick.AddListener(OnReadyClicked);
    }

    // PUBLIC METHODS
    public void StartTweenTimer()
    {
        _timerTarget.transform.DOMove(_timerEndRef.transform.position, PlatformSpawner.Setting.TotalGameTime);
    }

    public void StartTweenChickenWonImage()
    {
        _chickenWonImage.color = new Color(0, 0, 0, 0);
        _chickenWonImage.gameObject.SetActive(true);

        _chickenWonImage.DOColor(Color.white, _wonImageTweenDuration).SetUpdate(UpdateType.Normal, isIndependentUpdate: true);
    }

    public void StartTweenEggWonImage()
    {
        _eggWonImage.color = new Color(0, 0, 0, 0);
        _eggWonImage.gameObject.SetActive(true);

        _eggWonImage.DOColor(Color.white, _wonImageTweenDuration).SetUpdate(UpdateType.Normal, isIndependentUpdate: true);
    }

    // PRIVATE METHODS
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