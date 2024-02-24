using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Variables;

[Serializable]
public class PlayerUnitController : MonoBehaviour
{
    [SerializeField] private GameObject m_selectionIndicator;
    [SerializeField] private float _movementSpeed = 2f;

    [FormerlySerializedAs("_selectionVariable")] [SerializeField]
    private SelectionValueVariable selectionValueVariable;

    private Vector3 _movementTargetPosition;
    private bool _isMoving;
    private Rigidbody _rigidbody;

    void OnEnable()
    {
        selectionValueVariable.Modified += OnSelectAction;
        InputController.CommandAction += OnCommandAction;
        Deselect();
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_isMoving)
        {
            HandleMovement();
        }
    }

    private void OnDisable()
    {
        selectionValueVariable.Modified -= OnSelectAction;
        InputController.CommandAction -= OnCommandAction;
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

    private void OnCommandAction(Vector3 targetPosition)
    {
        if (selectionValueVariable.Value.Contains(this))
        {
            SetMovementTarget(targetPosition);
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

    private void SetMovementTarget(Vector3 targetPosition)
    {
        _isMoving = true;
        _movementTargetPosition = targetPosition;
    }

    private void HandleMovement()
    {
        Vector3 offset = _movementTargetPosition - transform.position;
        offset.y = 0;
        float stepLength = _movementSpeed * Time.deltaTime;

        transform.LookAt(transform.position + offset);
        _rigidbody.velocity = Vector3.zero;

        if (offset.magnitude < stepLength)
        {
            transform.position = _movementTargetPosition;
            _isMoving = false;
        }
        else
        {
            var velocity = transform.forward * _movementSpeed;
            velocity.y = 0;
            _rigidbody.AddForce(velocity, ForceMode.VelocityChange);
        }
    }
}