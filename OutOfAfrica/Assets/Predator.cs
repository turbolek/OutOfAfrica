using Sirenix.OdinInspector;
using UnityEngine;

public class Predator : SerializedMonoBehaviour
{
    [SerializeField] private Range _reactionRange;
    [SerializeField] private Range _chaseRange;

    private void Start()
    {
        _reactionRange.Init(typeof(PlayerUnitController), OnTargetEnteredReactionRange, OnTargetExitedReactionRange);
    }

    private void OnTargetEnteredReactionRange()
    {
    }

    private void OnTargetExitedReactionRange()
    {
    }
}