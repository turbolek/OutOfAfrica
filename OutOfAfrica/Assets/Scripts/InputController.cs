using UnityEngine;
using System;

public class InputController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    public static event Action<PlayerUnitController> SelectAction;

    [SerializeField] private GameObject m_clickIndicator;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hitInfo);

            var hitPlayer = hitInfo.transform ? hitInfo.transform.GetComponent<PlayerUnitController>() : null;

            SelectAction?.Invoke(hitPlayer);
        }
    }
}