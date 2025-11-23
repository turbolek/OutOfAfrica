using UnityEngine;

public class ItemStack : MonoBehaviour, IInventoryOwner
{
    private Inventory _inventory;

    private void Start()
    {

        _inventory = GetComponent<Inventory>();
        _inventory?.Init();
    }

    public Inventory GetInventory()
    {
        return _inventory;
    }
}