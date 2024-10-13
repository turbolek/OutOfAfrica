using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private InteractionPopup _currentPopup;
    [SerializeField] private BoolVariable _mapPauseVariable;
    [SerializeField] private ItemExchangePopup _itemExchangePopup;
    [SerializeField] private CampInteractionPopup _campInteractionPopup;
    [SerializeField] private UIManager _uiManager;

    private void OnEnable()
    {
        Targetable.InteractionPopupRequest += OnInteractionPopupRequested;
        InteractionPopup.Closed += OnInteractionPopupClosed;
    }

    private void OnDisable()
    {
        Targetable.InteractionPopupRequest -= OnInteractionPopupRequested;
        InteractionPopup.Closed -= OnInteractionPopupClosed;
    }

    private void OnInteractionPopupRequested(PlayerUnitController unit, Targetable targetable)
    {
        if (targetable == null)
        {
            return;
        }

        var popup = GetInteractionPopup(targetable);

        if (popup == null)
        {
            return;
        }

        _currentPopup = Instantiate(popup, _uiManager.transform);
        _currentPopup.Init(unit, targetable);
        _mapPauseVariable.Set(true);
    }

    private void OnInteractionPopupClosed(InteractionPopup popup)
    {
        if (popup == _currentPopup)
        {
            _mapPauseVariable.Set(false);
        }
    }

    private InteractionPopup GetInteractionPopup(Targetable targetable)
    {
        var craftingStation = targetable.GetComponent<CraftingStation>();

        if (craftingStation != null)
        {
            return _campInteractionPopup;
        }

        var inventoryOwner = targetable.GetComponent<IInventoryOwner>();

        if (inventoryOwner != null)
        {
            return _itemExchangePopup;
        }

        return null;
    }
}
