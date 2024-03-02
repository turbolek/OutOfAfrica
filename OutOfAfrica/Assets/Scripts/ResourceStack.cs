using UnityEngine;

public class ResourceStack : MonoBehaviour
{
    [field: SerializeField] public int Amount { get; private set; }

    public void ChangeValue(int delta)
    {
        Amount += delta;
    }
}