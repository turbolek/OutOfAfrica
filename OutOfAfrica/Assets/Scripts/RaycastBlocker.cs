using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastBlocker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public static Action<RaycastBlocker> CursorEntered;
    public static Action<RaycastBlocker> CursorExited;

    [SerializeField] private CanvasGroup _canvasGroup;

    private void OnDisable()
    {
        CursorExited?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsVisible())
        {
            CursorEntered?.Invoke(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorExited?.Invoke(this);
    }

    private bool IsVisible()
    {
        bool canvasGroupVisible = _canvasGroup == null || _canvasGroup.alpha > 0;
        return gameObject.activeInHierarchy && canvasGroupVisible;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        OnPointerEnter(eventData);
    }
}