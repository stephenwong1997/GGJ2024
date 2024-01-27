using System;

public static class EquippedItemFactory
{
    public static IEquippedItem Create(EItemType itemType, IItemPrefab prefab)
    {
        if (prefab == null)
            throw new InvalidOperationException($"ItemFactory.CreateItem: prefab null! type: {itemType}");

        switch (itemType)
        {
            case EItemType.Rocket: return new RocketItem((Rocket)prefab);
            case EItemType.Grenade: return new GrenadeItem((Grenade)prefab);
            case EItemType.Poop: return new FallingPoopItem((FallingPoop)prefab);
            case EItemType.LandMine: return new LandMineItem((LandMine)prefab);
        }

        throw new NotImplementedException($"ItemFactory.CreateItem: Did not handle type {itemType}!");
    }
}