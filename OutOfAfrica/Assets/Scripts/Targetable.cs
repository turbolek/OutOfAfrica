using System;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    public static event Action<PlayerUnitController, Targetable> InteractionPopupRequest;
    public static event Action<Targetable, PlayerUnitController> UnitEntered;
    public static event Action<Targetable, PlayerUnitController> UnitExited;

    [field: SerializeField] public InteractionPopup InteractionPopupTemplate;

    private void OnTriggerEnter(Collider other)
    {
        PlayerUnitController unit = other.GetComponent<PlayerUnitController>();
        if (unit)
        {
            UnitEntered?.Invoke(this, unit);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerUnitController unit = other.GetComponent<PlayerUnitController>();
        if (unit)
        {
            UnitExited?.Invoke(this, unit);
        }
    }

    public void Interact(PlayerUnitController unit)
    {
        InteractionPopupRequest?.Invoke(unit, this);
    }
}