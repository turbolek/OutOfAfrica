using System;
using UnityEngine;

public class StructureGhost : MonoBehaviour
{
    public static event Action<StructureGhost> Enabled;
    public static event Action<StructureGhost> Disabled;
    [SerializeField] private GameObject structurePrefab;
    [SerializeField] private Vector3Variable _nearestPositionOnGroundVariable;

    private void OnEnable()
    {
        Enabled?.Invoke(this);
        InputController.BuildingConfirmed += OnConfirmed;
        InputController.BuildingCanceled += OnCanceled;
    }

    private void OnDisable()
    {
        Disabled?.Invoke(this);
        InputController.BuildingConfirmed -= OnConfirmed;
        InputController.BuildingCanceled -= OnCanceled;
    }

    private void Update()
    {
        SetPosition();
    }

    public void SetPosition()
    {
        transform.position = _nearestPositionOnGroundVariable.Value;
    }

    private void OnConfirmed()
    {
        var structure = Instantiate(structurePrefab);
        structure.transform.position = transform.position;
        Destroy(gameObject);
    }

    private void OnCanceled()
    {
        Destroy(gameObject);
    }
}