using UnityEngine;

public class ItemStack : MonoBehaviour
{
    [field: SerializeField] public Inventory Inventory { get; private set; }
}