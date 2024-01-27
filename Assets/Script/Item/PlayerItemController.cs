using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerItemControllerDependencies
{
    // GETTERS
    public bool IsFacingLeft => IsFacingLeftGetter.Invoke();

    // CALLBACKS
    public Func<bool> IsFacingLeftGetter = () => false;
}

public class PlayerItemController : MonoBehaviour
{
    public UnityEvent<EItemType> OnItemEquipped;
    public UnityEvent OnItemDiscarded;

    // SERIALIZED MEMBERS
    [SerializeField]
    [FormerlySerializedAs("_spawnPosition")]
    private Transform _faceRightSpawnPosition;

    [SerializeField]
    private Transform _faceLeftSpawnPosition;

    // PRIVATE MEMBERS
    private PlayerItemControllerDependencies _dependencies = new();
    private IEquippedItem _equippedItem = null;
    private ItemContext _itemContext = null;

    // MonoBehaviour METHODS
    private void Awake()
    {
        _itemContext = new()
        {
            SpawnPositionGetter = () => _dependencies.IsFacingLeft ? _faceLeftSpawnPosition.position : _faceRightSpawnPosition.position,
            IsFacingLeftGetter = () => _dependencies.IsFacingLeft,
        };

        if (_faceRightSpawnPosition == null) Debug.LogError($"{this.gameObject.name}: _faceRightSpawnPosition null!", this.gameObject);
        if (_faceLeftSpawnPosition == null) Debug.LogError($"{this.gameObject.name}: _faceLeftSpawnPosition null!", this.gameObject);
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
        DiscardEquippedItem();
    }

    private void SetEquippedItem(IEquippedItem item)
    {
        _equippedItem = item;
        OnItemEquipped?.Invoke(item.GetItemType());
    }

    private void DiscardEquippedItem()
    {
        _equippedItem = null;
        OnItemDiscarded?.Invoke();
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