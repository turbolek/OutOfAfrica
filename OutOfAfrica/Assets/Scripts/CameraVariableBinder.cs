using UnityEngine;

public class CameraVariableBinder : MonoBehaviour
{
    [SerializeField] private CameraVariable _cameraVariable;
    [SerializeField] private Camera _camera;

    private void Awake()
    {
        _cameraVariable.Set(_camera);
    }
}
