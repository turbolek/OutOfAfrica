using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverTooltip : MonoBehaviour
{
    [SerializeField] private TMP_Text _title;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector3 _offset;

    private void OnEnable()
    {
        InputController.SelectableHovered += OnSelectableHovered;
    }

    private void Start()
    {
        Hide();
    }

    private void OnDisable()
    {
        InputController.SelectableHovered -= OnSelectableHovered;
    }

    private void Show()
    {
        _canvasGroup.alpha = 1f;
    }

    private void Hide()
    {
        _canvasGroup.alpha = 0f;
    }

    private void OnSelectableHovered(Selectable selectable)
    {
        if (selectable == null)
        {
            Hide();
            return;
        }


        transform.position = _camera.WorldToScreenPoint(selectable.transform.position) + _offset;
        Show();
        DisplaySelectable(selectable);

        Inventory inventory = selectable.GetComponent<Inventory>();
        if (inventory == null)
        {
            return;
        }

        DisplayInventory(inventory);
    }

    private void DisplaySelectable(Selectable selectable)
    {
        _title.text = selectable.name;
    }

    private void DisplayInventory(Inventory inventory)
    {
    }
}