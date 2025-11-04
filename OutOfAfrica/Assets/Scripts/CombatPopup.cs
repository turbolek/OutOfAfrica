using System.Collections;
using TMPro;
using UnityEngine;


public class CombatPopup : InteractionPopup
{
    [SerializeField] private TMP_Text _resultText;

    private PlayerUnitController _unit;
    private Predator _predator;

    protected override void OnInit(PlayerUnitController unit, Targetable targetable)
    {
        _unit = unit;
        _predator = targetable.GetComponent<Predator>();

        HandleCombat();
    }

    private void HandleCombat()
    {
        if (_predator == null)
        {
            return;
        }

        bool playerWins = Random.Range(0f, 1f) >= 0.5f;

        if (playerWins)
        {
            _resultText.text = $"{ _predator.name} died!";
            Destroy(_predator.gameObject);
        }
        else
        {
            _resultText.text = $"{ _unit.name} died!";
            Destroy(_unit.gameObject);
        }
    }
}
