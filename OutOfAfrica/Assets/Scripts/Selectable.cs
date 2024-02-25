using System.Collections.Generic;
using UnityEngine;
using Variables;

public class Selectable : MonoBehaviour
{
    [field: SerializeField] public SelectableType Type;
    [SerializeField] private GameObject m_selectionIndicator;
    [SerializeField] private SelectionValueVariable selectionValueVariable;

    void OnEnable()
    {
        selectionValueVariable.Modified += OnSelectAction;
        Deselect();
    }

    private void OnDisable()
    {
        selectionValueVariable.Modified -= OnSelectAction;
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
}