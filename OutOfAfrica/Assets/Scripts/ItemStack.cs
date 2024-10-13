using UnityEngine;

public class ItemStack : MonoBehaviour, IInventoryOwner
{
    private Inventory _inventory;

    private void Start()
    {

        _inventory = GetComponent<Inventory>();
    }

    public Inventory GetInventory()
    {
        return _inventory;
    }
}