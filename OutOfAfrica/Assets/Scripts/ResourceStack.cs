using UnityEngine;

public class ResourceStack : MonoBehaviour
{
    [field: SerializeField] public ResourceType ResourceType { get; private set; }
    [field: SerializeField] public int Amount { get; private set; }
    

    public void ChangeValue(int delta)
    {
        Amount += delta;
    }
}