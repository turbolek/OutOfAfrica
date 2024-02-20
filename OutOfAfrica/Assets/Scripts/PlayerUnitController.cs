using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitController : MonoBehaviour
{
    [SerializeField] private GameObject m_selectionIndicator;

    void OnEnable()
    {
        InputController.SelectAction += OnSelectAction;
        Deselect();
    }

    private void OnDisable()
    {
        InputController.SelectAction -= OnSelectAction;
    }

    private void OnSelectAction(List<PlayerUnitController> selectedPlayers)
    {
        if (selectedPlayers.Contains(this))
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