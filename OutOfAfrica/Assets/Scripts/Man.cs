using UnityEngine;

public class Man : MonoBehaviour
{
    [field: SerializeField]
    public Inventory Inventory { get; private set; }

    private void Start()
    {
        Inventory.Init();
    }
}
