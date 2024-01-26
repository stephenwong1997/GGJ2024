using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemContext
{
    public bool IsFacingLeft { get; set; }
}

public interface IItem
{
    public void Use(IItemContext context);
}

public interface IItemPrefab { }

public class PlayerItemController : MonoBehaviour
{
    private IItem _equippedItem = null;
    private IItemContext _itemContext = null;

    // PUBLIC METHODS
    public void OnSkillDown()
    {
        Debug.Log("PlayerItemController.OnSkillDown");
        TryUseItem();
    }

    public void EquipItem(IItem item)
    {
        Debug.Log("PlayerItemController.EquipItem: Equipped!");
        SetEquippedItem(item);
    }

    // PRIVATE METHODS
    private void TryUseItem()
    {
        if (_equippedItem == null)
            return;

        if (_itemContext == null)
        {
            Debug.LogError("PlayerItemController: Item Context null! Unable to use item!");
            return;
        }

        _equippedItem.Use(_itemContext);

        DiscardEquippedItem();
    }

    private void SetEquippedItem(IItem item)
    {
        _equippedItem = item;
        // TODO : OnEquippedItemChanged => UnityEvent!
    }

    private void DiscardEquippedItem()
    {
        _equippedItem = null;
        // TODO : Update display
    }
}