using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionPopup : MonoBehaviour
{
    public static Action<InteractionPopup> Closed;

    [SerializeField] private TMP_Text _unitNameLabel;
    [SerializeField] private TMP_Text _targetableNameLabel;
    [SerializeField] private Button _closeButton;

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(Close);
    }

    public void Init(PlayerUnitController unit, Targetable targetable)
    {
        _unitNameLabel.text = unit.name;
        _targetableNameLabel.text = targetable.name;
        gameObject.SetActive(true);
    }

    [Button]
    public void Close()
    {
        Closed?.Invoke(this);
        Destroy(gameObject);
    }
}
