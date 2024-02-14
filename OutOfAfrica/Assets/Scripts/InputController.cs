using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InputController : MonoBehaviour
{
    public static event Action<PlayerUnitController> SelectAction;

    [SerializeField] private Camera _camera;

    [FormerlySerializedAs("m_clickIndicator")] [SerializeField]
    private GameObject _clickIndicator;

    [SerializeField] private RectTransform _selectionFrame;

    private InputActions _inputActions;
    private Vector3 _mousePositionWorld;
    private Vector2 _mousePositionScreen;

    private bool _isSelecting;

    private Vector2 _selectionStartPositionScreen;
    private Vector2 _selectionEndPositionScreen;

    private void Start()
    {
        _inputActions = new();
        _inputActions.Enable();

        _inputActions.Selection.StartSelection.performed += ctx => { StartSelection(); };
        _inputActions.Selection.StartSelection.canceled += ctx => { EndSelection(); };

        _selectionFrame.gameObject.SetActive(false);
    }

    private void Update()
    {
        _mousePositionScreen = Mouse.current.position.ReadValue();
        _mousePositionWorld = _camera.ScreenToWorldPoint(_mousePositionScreen);


        if (_isSelecting)
        {
            _selectionEndPositionScreen = _mousePositionScreen;
            CalculateSelectionRect();
        }

        // if (Input.GetMouseButtonDown(0))
        // {
        //     var ray = _camera.ScreenPointToRay(_mousePositionScreen);
        //     Physics.Raycast(ray, out RaycastHit hitInfo);
        //
        //     var hitPlayer = hitInfo.transform ? hitInfo.transform.GetComponent<PlayerUnitController>() : null;
        //
        //     SelectAction?.Invoke(hitPlayer);
        // }
    }

    private void StartSelection()
    {
        _selectionStartPositionScreen = _mousePositionScreen;
        _selectionFrame.gameObject.SetActive(true);
        _isSelecting = true;
    }

    private void EndSelection()
    {
        _selectionFrame.gameObject.SetActive(false);
        _isSelecting = false;
    }

    private void CalculateSelectionRect()
    {
        Vector2 startXEndY = new Vector2(_selectionStartPositionScreen.x, _selectionEndPositionScreen.y);
        Vector2 endXStartY = new Vector2(_selectionEndPositionScreen.x, _selectionStartPositionScreen.y);

        Vector2 topLeft = Vector2.zero;
        Vector2 topRight = Vector2.zero;
        Vector2 bottomRight = Vector2.zero;
        Vector2 bottomLeft = Vector2.zero;

        if (_selectionStartPositionScreen.x < _selectionEndPositionScreen.x
            && _selectionStartPositionScreen.y < _selectionEndPositionScreen.y)
        {
            // SX E
            // S  EX
            topLeft = startXEndY;
            topRight = _selectionEndPositionScreen;
            bottomRight = endXStartY;
            bottomLeft = _selectionStartPositionScreen;
        }
        else if (_selectionStartPositionScreen.x < _selectionEndPositionScreen.x
                 && _selectionStartPositionScreen.y >= _selectionEndPositionScreen.y)
        {
            // S  EX
            // SX E
            topLeft = _selectionStartPositionScreen;
            topRight = endXStartY;
            bottomRight = _selectionEndPositionScreen;
            bottomLeft = startXEndY;
        }
        else if (_selectionStartPositionScreen.x >= _selectionEndPositionScreen.x
                 && _selectionStartPositionScreen.y >= _selectionEndPositionScreen.y)
        {
            // EX S
            // E  SX
            topLeft = endXStartY;
            topRight = _selectionStartPositionScreen;
            bottomRight = startXEndY;
            bottomLeft = _selectionEndPositionScreen;
        }
        else if (_selectionStartPositionScreen.x >= _selectionEndPositionScreen.x
                 && _selectionStartPositionScreen.y < _selectionEndPositionScreen.y)
        {
            // E  SX
            // EX S
            topLeft = _selectionEndPositionScreen;
            topRight = startXEndY;
            bottomRight = _selectionStartPositionScreen;
            bottomLeft = endXStartY;
        }

        var width = topRight.x - topLeft.x;
        var height = topLeft.y - bottomLeft.y;

        _selectionFrame.anchoredPosition = topLeft;
        _selectionFrame.sizeDelta = new Vector2(width, height);
    }
}