using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemControllerDependencies
{
    // GETTERS
    public bool IsFacingLeft => IsFacingLeftGetter.Invoke();

    // CALLBACKS
    public Func<bool> IsFacingLeftGetter = () => false;
}

public class PlayerItemController : MonoBehaviour
{
    [SerializeField] private Transform _spawnPosition;

    private PlayerItemControllerDependencies _dependencies = new();
    private IEquippedItem _equippedItem = null;
    private ItemContext _itemContext = null;

    // MonoBehaviour METHODS
    private void OnValidate()
    {
        if (_spawnPosition == null) Debug.LogError($"{this.gameObject.name}: _spawnPosition null!");
    }

    private void Awake()
    {
        _itemContext = new()
        {
            SpawnPositionGetter = () => _spawnPosition.position,
            IsFacingLeftGetter = () => _dependencies.IsFacingLeft,
        };
    }

    // PUBLIC METHODS
    public void InjectDependencies(PlayerItemControllerDependencies dependencies)
    {
        _dependencies = dependencies;
    }

    public void OnSkillDown()
    {
        Debug.Log("PlayerItemController.OnSkillDown");
        TryUseEquippedItem();
    }

    public void EquipItem(IEquippedItem item)
    {
        Debug.Log("PlayerItemController.EquipItem: Equipped!");
        SetEquippedItem(item);
    }

    // PRIVATE METHODS
    private void TryUseEquippedItem()
    {
        if (_equippedItem == null)
            return;

        if (_dependencies == null)
        {
            Debug.LogError("PlayerItemController: _dependencies null! Unable to use item!");
            return;
        }

        if (_itemContext == null)
        {
            Debug.LogError("PlayerItemController: Item Context null! Unable to use item!");
            return;
        }

        _equippedItem.Use(_itemContext);
        //DiscardEquippedItem();
    }

    private void SetEquippedItem(IEquippedItem item)
    {
        _equippedItem = item;
        // TODO : OnEquippedItemChanged => UnityEvent!
    }

    private void DiscardEquippedItem()
    {
        _equippedItem = null;
        // TODO : Update display
    }

    // HELPER CLASS
    public class ItemContext : IItemContext
    {
        public Vector2 SpawnPosition => SpawnPositionGetter.Invoke();
        public bool IsFacingLeft => IsFacingLeftGetter.Invoke();

        // CALLBACKS
        public Func<Vector2> SpawnPositionGetter = () => Vector2.zero;
        public Func<bool> IsFacingLeftGetter = () => false;
    }
}