using System.Collections.Generic;
using UnityEngine;
using Variables;

public class Selectable : MonoBehaviour
{
    [field: SerializeField] public SelectableType Type;
    [SerializeField] private GameObject m_selectionIndicator;
    [SerializeField] private SelectionValueVariable selectionValueVariable;

    private bool _isHovered;

    void OnEnable()
    {
        selectionValueVariable.Modified += OnSelectAction;
        InputController.SelectableHovered += OnSelectableHovered;

        Deselect();
        Unhover();
    }

    private void OnDisable()
    {
        selectionValueVariable.Modified -= OnSelectAction;
        InputController.SelectableHovered -= OnSelectableHovered;
    }


    private void OnSelectAction((List<Selectable> currentSelectables, List<Selectable> previousSelectables) value)
    {
        if (value.currentSelectables.Contains(this))
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

    private void Hover()
    {
        _isHovered = true;
        Debug.Log($"{name} hovered");
    }

    private void Unhover()
    {
        _isHovered = false;
        Debug.Log($"{name} unhovered");
    }

    private void OnSelectableHovered(Selectable selectable)
    {
        if (selectable == this)
        {
            if (!_isHovered)
            {
                Hover();
            }
        }
        else
        {
            if (_isHovered)
            {
                Unhover();
            }
        }
    }
}