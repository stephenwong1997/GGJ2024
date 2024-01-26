using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    [SerializeField] private EItemType _itemType;
    [SerializeField] private GameObject _itemPrefab;

    private void OnValidate()
    {
        if (!TryGetComponent(out Collider2D _))
            Debug.LogError($"{this.gameObject.name}: No collider 2D component!");

        if (_itemPrefab == null)
        {
            Debug.LogError($"{this.gameObject.name}: _itemPrefab null!");
        }
        else
        {
            bool isValid = _itemPrefab.TryGetComponent(out IItemPrefab _);
            if (!isValid)
                Debug.LogError($"{this.gameObject.name}: _itemPrefab does not have IItemPrefab component!");
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log($"FloatingItem.OnTriggerEnter2D: {col.gameObject.name}");

        var itemController = col.GetComponentInChildren<PlayerItemController>();
        if (itemController == null)
            return;

        IItemPrefab itemPrefab = _itemPrefab.GetComponent<IItemPrefab>();
        IItem item = ItemFactory.CreateItem(_itemType, itemPrefab);

        itemController.EquipItem(item);
    }
}

public enum EItemType
{
    Rocket = 0
}

public static class ItemFactory
{
    public static IItem CreateItem(EItemType itemType, IItemPrefab prefab)
    {
        if (prefab == null)
            throw new InvalidOperationException($"ItemFactory.CreateItem: prefab null! type: {itemType}");

        switch (itemType)
        {
            case EItemType.Rocket: return new RocketItem((Rocket)prefab);
        }

        throw new NotImplementedException($"ItemFactory.CreateItem: Did not handle type {itemType}!");
    }
}