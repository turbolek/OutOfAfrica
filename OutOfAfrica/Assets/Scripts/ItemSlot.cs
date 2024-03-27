using System;

[Serializable]
public class ItemSlot
{
    public ItemData ItemData;
    public int Amount;

    public bool CanFitItem(ItemData item)
    {
        return ItemData == null || (ItemData == item && Amount < ItemData.SlotCapacity);
    }

    public bool ContainsItem(ItemData item)
    {
        return ItemData == item && Amount > 0;
    }
}