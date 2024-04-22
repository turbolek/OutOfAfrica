using System;

[Serializable]
public class Item
{
    public static Action<Item> CollectionProgressChanged;

    public ItemData Data { get; private set; }
    public float CollectionProgress { get; private set; } = 0f;

    public Item(ItemData data)
    {
        Data = data;
        SetCollectionProgress(0f);
    }

    public void ChangeCollectionProgress(float progressDelta)
    {
        SetCollectionProgress(CollectionProgress += progressDelta);
    }

    private void SetCollectionProgress(float progress)
    {
        CollectionProgress = progress;
        CollectionProgressChanged?.Invoke(this);
    }
}