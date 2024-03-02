using System.Collections.Generic;
using UnityEngine;
using Variables;

public class CommandManager : MonoBehaviour
{
    [SerializeField] private SelectionValueVariable _currentSelection;
    [SerializeField] private float _positionSearchAngleStep;
    [SerializeField] private Camera _camera; //TODO inject?

    private void OnEnable()
    {
        InputController.CommandAction += OnCommandAction;
    }

    private void OnDisable()
    {
        InputController.CommandAction -= OnCommandAction;
    }

    private void OnCommandAction()
    {
        if (_currentSelection.Value.Count < 1)
        {
            return;
        }

        var targetable = GetTargetable(InputController.MousePositionScreen);

        Vector3 targetPosition =
            targetable != null ? targetable.transform.position : InputController.MousePositionWorld;

        List<(PlayerUnitController Unit, Vector3 Position)> takenPositions = new();

        var mainUnit = _currentSelection.Value[0].GetComponent<PlayerUnitController>();

        if (!mainUnit)
        {
            return;
        }

        takenPositions.Add(new(mainUnit, targetPosition));

        for (int i = 1; i < _currentSelection.Value.Count; i++)
        {
            var unit = _currentSelection.Value[i].GetComponent<PlayerUnitController>();
            if (!unit)
            {
                continue;
            }

            var position = GetFreePosition(takenPositions, unit);

            takenPositions.Add(new(unit, position));
        }

        foreach (var takenPosition in takenPositions)
        {
            if (takenPosition.Unit)
            {
                takenPosition.Unit.SetTarget(targetable);
                takenPosition.Unit.SetMovementTarget(takenPosition.Position);
            }
        }
    }

    private Targetable GetTargetable(Vector3 position)
    {
        Targetable targetable = null;
        var ray = _camera.ScreenPointToRay(position);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            targetable = hit.transform.GetComponent<Targetable>();
        }

        return targetable;
    }

    private Vector3 GetFreePosition(List<(PlayerUnitController unit, Vector3 position)> takenPositions,
        PlayerUnitController unit)
    {
        if (takenPositions.Count < 1)
        {
            return Vector3.zero;
        }

        var startingPosition = takenPositions[0].position;
        var startingRadius = takenPositions[0].unit.BaseRadius;

        int loopCount = 1;
        while (true)
        {
            float offsetRadius = startingRadius + loopCount * unit.BaseRadius;
            float currentAngle = 0f;
            Vector3 direction = new Vector3(1, 0, 1);

            while (currentAngle < 360f)
            {
                direction = Quaternion.Euler(0, currentAngle, 0) * direction;
                Vector3 position = startingPosition + direction * offsetRadius;

                bool positionValid = true;

                foreach (var takenPosition in takenPositions)
                {
                    positionValid &= Vector3.Distance(takenPosition.position, position) >=
                                     takenPosition.unit.BaseRadius + unit.BaseRadius;

                    if (!positionValid)
                    {
                        break;
                    }
                }

                if (positionValid)
                {
                    return position;
                }

                currentAngle += _positionSearchAngleStep;
            }

            loopCount++;
        }
    }
}