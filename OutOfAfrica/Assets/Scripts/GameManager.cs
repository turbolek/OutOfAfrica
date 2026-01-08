using UnityEngine;
using Variables;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GenericVariable[] _variablesToClear;
    [SerializeField] private GameObject[] _worldPrefabs;
    [SerializeField] private PlayerUnitController _playerUnitController;

    private GameObject currentWorld;

    private void OnEnable()
    {
        CampInteractionPopup.DayFinishRequested += OnDayFinishRequested;
    }

    private void OnDisable()
    {
        CampInteractionPopup.DayFinishRequested -= OnDayFinishRequested;
    }


    private void Start()
    {
        foreach (var variable in _variablesToClear)
        {
            variable.Clear();
        }

        StartNewDay();
    }

    private void OnDayFinishRequested(Inventory campInventory)
    {
        FinishCurrentDay();
        StartNewDay();
    }

    private void FinishCurrentDay()
    {
        Debug.Log("Finishing day...");
        if (currentWorld != null)
        {
            Destroy(currentWorld);
        }
    }

    private void StartNewDay()
    {
        Debug.Log("Starting new day...");

        currentWorld = Instantiate(_worldPrefabs[Random.Range(0, _worldPrefabs.Length)]);
        _playerUnitController.FindWorldReferences();
    }
}