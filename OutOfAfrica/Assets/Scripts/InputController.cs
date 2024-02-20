using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InputController : MonoBehaviour
{
    public static event Action<List<PlayerUnitController>> SelectAction;

    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _terrainLayerMask;

    [FormerlySerializedAs("m_clickIndicator")] [SerializeField]
    private GameObject _clickIndicator;

    [SerializeField] private RectTransform _selectionFrame;
    [SerializeField] private MeshCollider _selectionCollider;

    private InputActions _inputActions;
    private Vector3 _mousePositionWorld;
    private Vector2 _mousePositionScreen;

    private bool _isSelecting;
    private Vector2 _selectionStartPositionScreen;
    private Vector2 _selectionEndPositionScreen;

    private bool _disableCollider;

    private List<PlayerUnitController> _selectedUnits = new();

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

        DisableCollider();
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
            vertices[i] = corners[i - 4] + Vector3.up * 100f;
        }

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = vertices;
        selectionMesh.triangles = triangles;

        return selectionMesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        var unit = other.GetComponent<PlayerUnitController>();
        if (unit)
        {
            _selectedUnits.Add(unit);
        }
    }

    private async void DisableCollider()
    {
        await Task.Delay(100);
        SelectAction?.Invoke(_selectedUnits);
        _selectionCollider.enabled = false;
        _selectedUnits.Clear();
    }
}