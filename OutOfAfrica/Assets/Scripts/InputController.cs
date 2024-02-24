using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(MeshCollider))]
public class InputController : MonoBehaviour
{
    public static event Action<Vector3> CommandAction;

    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _terrainLayerMask;
    [SerializeField] private float _selectionColliderHeight = 100f;
    [SerializeField] private int _selectionBoxLifetimeMs = 100;
    [SerializeField] private float _clickLength = 0.1f;
    [SerializeField] private RectTransform _selectionFrame;

    [SerializeField] private SelectionVariable _selectionVariable;

    private MeshCollider _selectionCollider;
    private InputActions _inputActions;
    private Vector3 _mousePositionWorld;
    private Vector2 _mousePositionScreen;

    private bool _isSelecting;
    private Vector2 _selectionStartPositionScreen;
    private Vector2 _selectionEndPositionScreen;
    private bool _disableCollider;

    private float _selectionStartTime;
    private bool _isBoxSelection;

    private void Start()
    {
        _selectionCollider = GetComponent<MeshCollider>();
        _inputActions = new();
        _inputActions.Enable();

        _inputActions.Selection.StartSelection.performed += ctx => { StartSelection(); };
        _inputActions.Selection.StartSelection.canceled += ctx => { EndSelection(); };
        _inputActions.Selection.Command.performed += ctx => { OnCommandAction(); };

        _selectionFrame.gameObject.SetActive(false);
    }

    private void Update()
    {
        _mousePositionScreen = Mouse.current.position.ReadValue();
        var ray = _camera.ScreenPointToRay(_mousePositionScreen);
        _mousePositionWorld = Physics.Raycast(ray, out RaycastHit hit, 1000f, _terrainLayerMask)
            ? hit.point
            : _camera.ScreenToWorldPoint(_mousePositionScreen);

        HandleSelection();
    }

    private void OnCommandAction()
    {
        CommandAction?.Invoke(_mousePositionWorld);
    }

    private void HandleSelection()
    {
        if (_isSelecting)
        {
            float clickDuration = Time.time - _selectionStartTime;
            _isBoxSelection = clickDuration > _clickLength;

            if (_isBoxSelection)
            {
                _selectionEndPositionScreen = _mousePositionScreen;
                CalculateSelectionRect();
            }
        }

        _selectionFrame.gameObject.SetActive(_isSelecting && _isBoxSelection);
    }

    private void StartSelection()
    {
        _selectionStartTime = Time.time;
        _isSelecting = true;
        _selectionStartPositionScreen = _mousePositionScreen;
    }

    private async void EndSelection()
    {
        _selectionVariable.Clear();

        if (_isBoxSelection)
        {
            await HandleBoxSelection();
        }
        else
        {
            HandleClickSelection();
        }

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
        var sizeVector = new Vector2(width, height);

        _selectionFrame.anchoredPosition = topLeft;
        _selectionFrame.sizeDelta = sizeVector;
    }

    private Mesh GenerateSelectionMesh(Vector3[] corners)
    {
        Vector3[] vertices = new Vector3[8];
        int[] triangles =
        {
            0, 1, 2,
            2, 1, 3,
            4, 6, 0,
            0, 6, 2,
            6, 7, 2,
            2, 7, 3,
            7, 5, 3,
            3, 5, 1,
            5, 0, 1,
            1, 4, 0,
            4, 5, 6,
            6, 5, 7
        };

        for (int i = 0; i < 4; i++)
        {
            vertices[i] = corners[i];
        }

        for (int i = 4; i < 8; i++)
        {
            vertices[i] = corners[i - 4] + Vector3.up * _selectionColliderHeight;
        }

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = vertices;
        selectionMesh.triangles = triangles;

        return selectionMesh;
    }

    private void HandleClickSelection()
    {
        var ray = _camera.ScreenPointToRay(_mousePositionScreen);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            PlayerUnitController unit = hit.transform.GetComponent<PlayerUnitController>();
            if (unit)
            {
                _selectionVariable.Set(unit);
            }
        }
    }

    private async Task HandleBoxSelection()
    {
        Vector3[] corners = new Vector3[4];
        _selectionFrame.GetWorldCorners(corners);

        Vector3[] worldCorners = new Vector3[4];

        var ray0 = _camera.ScreenPointToRay(corners[0]);
        Physics.Raycast(ray0, out RaycastHit hit0, 1000f, _terrainLayerMask, QueryTriggerInteraction.UseGlobal);
        worldCorners[0] = hit0.point;
        var ray1 = _camera.ScreenPointToRay(corners[1]);
        Physics.Raycast(ray1, out RaycastHit hit1, 1000f, _terrainLayerMask, QueryTriggerInteraction.UseGlobal);
        worldCorners[1] = hit1.point;
        var ray2 = _camera.ScreenPointToRay(corners[2]);
        Physics.Raycast(ray2, out RaycastHit hit2, 1000f, _terrainLayerMask, QueryTriggerInteraction.UseGlobal);
        worldCorners[2] = hit2.point;
        var ray3 = _camera.ScreenPointToRay(corners[3]);
        Physics.Raycast(ray3, out RaycastHit hit3, 1000f, _terrainLayerMask, QueryTriggerInteraction.UseGlobal);
        worldCorners[3] = hit3.point;

        _selectionCollider.sharedMesh = GenerateSelectionMesh(worldCorners);
        _selectionCollider.enabled = true;

        await DisableCollider();
    }

    private void OnTriggerEnter(Collider other)
    {
        var unit = other.GetComponent<PlayerUnitController>();
        if (unit)
        {
            _selectionVariable.Add(unit);
        }
    }

    private async Task DisableCollider()
    {
        await Task.Delay(_selectionBoxLifetimeMs);
        _selectionCollider.enabled = false;
    }
}