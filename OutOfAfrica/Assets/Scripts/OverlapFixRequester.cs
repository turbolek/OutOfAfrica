using System;
using UnityEngine;

public class OverlapFixRequester
{
    public static Action<RectTransform, Vector2> FixSubscribeRequested;
    public static Action<RectTransform> FixUnsubscribeRequested;

    public void RequestFixSubscribe(RectTransform rectTransform, Vector2 screenPosition)
    {
        FixSubscribeRequested?.Invoke(rectTransform, screenPosition);
    }

    public void RequestFixUnsubscribe(RectTransform rectTransform)
    {
        FixUnsubscribeRequested?.Invoke(rectTransform);
    }
}