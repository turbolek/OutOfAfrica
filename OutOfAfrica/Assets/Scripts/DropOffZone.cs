using System;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class DropOffZone : MonoBehaviour
{
    public Inventory Inventory { get; private set; }

    private void Start()
    {
        Inventory = GetComponent<Inventory>();
    }
}