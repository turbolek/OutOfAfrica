using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Variables;

[Serializable]
public class PlayerUnitController : MonoBehaviour
{
    [field: SerializeField] public float BaseRadius { get; private set; }

    [SerializeField] private GameObject m_selectionIndicator;
    [SerializeField] private float _movementSpeed = 2f;


    [FormerlySerializedAs("_selectionVariable")] [SerializeField]
    private SelectionValueVariable selectionValueVariable;

    private Vector3 _movementTargetPosition;
    private bool _isMoving;
    private NavMeshAgent _navMeshAgent;

    void OnEnable()
    {
        selectionValueVariable.Modified += OnSelectAction;
        Deselect();
    }

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnDisable()
    {
        selectionValueVariable.Modified -= OnSelectAction;
    }

    private void OnSelectAction((List<PlayerUnitController> selectedPlayers,
        List<PlayerUnitController> previouslySelectedPlayers) value)
    {
        if (value.selectedPlayers.Contains(this))
        {
            Select();
        }
        else
        {
            Deselect();
        }
    }

    private void Select()
    {
        m_selectionIndicator.gameObject.SetActive(true);
    }

    private void Deselect()
    {
        m_selectionIndicator.gameObject.SetActive(false);
    }

    public void SetMovementTarget(Vector3 targetPosition)
    {
        _isMoving = true;
        _navMeshAgent.destination = targetPosition;
        _movementTargetPosition = targetPosition;
    }
}