using System;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    public event Action<PlayerUnitController> UnitEntered;
    public event Action<PlayerUnitController> UnitExited;

    private void OnTriggerEnter(Collider other)
    {
        PlayerUnitController unit = other.GetComponent<PlayerUnitController>();
        if (unit)
        {
            UnitEntered?.Invoke(unit);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerUnitController unit = other.GetComponent<PlayerUnitController>();
        if (unit)
        {
            UnitExited?.Invoke(unit);
        }
    }
}