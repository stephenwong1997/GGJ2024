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
        var itemController = col.GetComponentInChildren<PlayerItemController>();
        if (itemController == null)
            return;

        IItemPrefab itemPrefab = _itemPrefab.GetComponent<IItemPrefab>();
        IEquippedItem item = EquippedItemFactory.Create(_itemType, itemPrefab);

        itemController.EquipItem(item);

        // Use object pool... later if needed~
        Destroy(this.gameObject);
    }
}
