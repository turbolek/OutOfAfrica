using System.Collections.Generic;
using UnityEngine;
using Variables;

public class CommandManager : MonoBehaviour
{
    [SerializeField] private SelectionValueVariable _currentSelection;
    [SerializeField] private float _positionSearchAngleStep;

    private void OnEnable()
    {
        InputController.CommandAction += OnCommandAction;
    }

    private void OnDisable()
    {
        InputController.CommandAction -= OnCommandAction;
    }

    private void OnCommandAction(Vector3 targetPosition)
    {
        if (_currentSelection.Value.Count < 0)
        {
            return;
        }

        List<(PlayerUnitController Unit, Vector3 Position)> takenPositions = new();

        takenPositions.Add(new(_currentSelection.Value[0], targetPosition));

        for (int i = 1; i < _currentSelection.Value.Count; i++)
        {
            var unit = _currentSelection.Value[i];
            var position = GetFreePosition(takenPositions, unit);

            takenPositions.Add(new(unit, position));
        }

        foreach (var takenPosition in takenPositions)
        {
            takenPosition.Unit.SetMovementTarget(takenPosition.Position);
        }
    }

    private Vector3 GetFreePosition(List<(PlayerUnitController unit, Vector3 position)> takenPositions,
        PlayerUnitController unit)
    {
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