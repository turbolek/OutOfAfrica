using UnityEngine;
using Variables;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GenericVariable[] _variablesToClear;

    private void Start()
    {
        foreach (var variable in _variablesToClear)
        {
            variable.Clear();
        }
    }
}