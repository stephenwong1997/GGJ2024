public interface IEquippedItem
{
    public void Use(IItemContext context);
    public EItemType GetItemType();
}