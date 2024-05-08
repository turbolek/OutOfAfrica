using System.Collections.Generic;
using UnityEngine;
using Variables;

public class Selectable : MonoBehaviour
{
    [field: SerializeField] public SelectableType Type;
    [SerializeField] private SelectionIndicator m_selectionIndicator;
    [SerializeField] private SelectionValueVariable selectionValueVariable;


    public bool IsSelected => selectionValueVariable.Value.Contains(this);
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
        m_selectionIndicator.Select();
    }

    private void Deselect()
    {
        m_selectionIndicator.Deselect();
    }

    private void Hover()
    {
        _isHovered = true;
        m_selectionIndicator.Hover();
    }

    private void Unhover()
    {
        _isHovered = false;
        m_selectionIndicator.Unhover();
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