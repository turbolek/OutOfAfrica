using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class UIOverlapResolver : MonoBehaviour
{
    public class UIData
    {
        public RectTransform RectTransform;
        public Vector2 PreferredPosition;
    }


    private List<UIData> _shownUIs = new();
    private List<UIData> _fixedUIs = new();

    private void OnEnable()
    {
        InventoryView.FixSubscribeRequested += OnSubscribeRequested;
        InventoryView.FixUnsubscribeRequested += OnUnsubscribeRequested;
    }

    private void Update()
    {
        //FixUIOverlap();
    }

    private void OnDisable()
    {
        InventoryView.FixSubscribeRequested -= OnSubscribeRequested;
        InventoryView.FixUnsubscribeRequested -= OnUnsubscribeRequested;
    }

    private void OnSubscribeRequested(RectTransform rectTransform, Vector2 position)
    {
        var uiData = GetUIData(rectTransform);
        if (uiData == null)
        {
            uiData = new UIData();
            uiData.RectTransform = rectTransform;
            _shownUIs.AddExclusive(uiData);
        }

        uiData.PreferredPosition = position;
        FixUIOverlap();
    }

    private void OnUnsubscribeRequested(RectTransform rectTransform)
    {
        var uiData = GetUIData(rectTransform);
        _shownUIs.Remove(uiData);
        FixUIOverlap();
    }

    private void FixUIOverlap()
    {
        _fixedUIs.Clear();
        foreach (var uiData in _shownUIs)
        {
            FixUI(uiData);
        }
    }

    private void FixUI(UIData uiData)
    {
        uiData.RectTransform.position = GetFreePosition(uiData);
        _fixedUIs.AddExclusive(uiData);
    }

    private UIData GetUIData(RectTransform rectTransform)
    {
        foreach (var uiData in _shownUIs)
        {
            if (uiData.RectTransform == rectTransform)
            {
                return uiData;
            }
        }

        return null;
    }

    private Vector3 GetFreePosition(UIData uiData)
    {
        Vector2 currentPosition = uiData.PreferredPosition;
        Rect currentRect = uiData.RectTransform.rect;

        bool canMoveLeft = true;
        bool canMoveRight = true;
        bool canMoveDown = true;
        bool canMoveUp = true;

        for (int i = 0; i < _fixedUIs.Count; i++)
        {
            var fixedUI = _fixedUIs[i];
            if (fixedUI == uiData)
            {
                continue;
            }

            var fixedRect = fixedUI.RectTransform.rect;

            Vector2 fixedUIMinPos = (Vector2)fixedUI.RectTransform.position + fixedRect.min;
            Vector2 fixedUIMaxPos = (Vector2)fixedUI.RectTransform.position + fixedRect.max;
            Vector2 currentUIMinPos = currentPosition + currentRect.min;
            Vector2 currentUIMaxPos = currentPosition + currentRect.max;

            if (!CheckOverlap(fixedUIMinPos, fixedUIMaxPos, currentUIMinPos, currentUIMaxPos))
            {
                continue;
            }

            Vector2 leftShift = new Vector2(fixedUIMinPos.x - currentUIMaxPos.x, 0);
            if (leftShift.x > -5)
            {
                leftShift.x = -5;
            }

            Vector2 rightShift = new Vector2(fixedUIMaxPos.x - currentUIMinPos.x, 0);
            if (rightShift.x < 5)
            {
                rightShift.x = 5;
            }

            Vector2 upShift = new Vector2(0, fixedUIMaxPos.y - currentUIMinPos.y);
            if (upShift.y < 5)
            {
                upShift.y = 5;
            }

            Vector2 downShift = new Vector2(0, fixedUIMinPos.y - currentUIMaxPos.y);
            if (downShift.y > -5)
            {
                downShift.y = -5;
            }

            List<Vector2> allowedShifts = new();

            if (canMoveLeft)
            {
                allowedShifts.Add(leftShift);
            }

            if (canMoveRight)
            {
                allowedShifts.Add(rightShift);
            }

            if (canMoveUp)
            {
                allowedShifts.Add(upShift);
            }

            if (canMoveDown)
            {
                allowedShifts.Add(downShift);
            }

            var minShift = allowedShifts.OrderBy(s => s.magnitude).First();

            canMoveLeft = minShift != rightShift;
            canMoveRight = minShift != leftShift;
            canMoveDown = minShift != upShift;
            canMoveUp = minShift != downShift;

            var randomMultiplier = Random.Range(1f, 1.1f);
            minShift *= randomMultiplier; //prevents infinite loops in edge case
            currentPosition += minShift;

            i = 0;
        }

        return currentPosition;
    }

    private bool CheckOverlap(Vector2 min1, Vector2 max1, Vector2 min2, Vector2 max2)
    {
        return (min1.x >= min2.x && min1.x <= max2.x && min1.y >= min2.y && min1.y <= max2.y) || //bottom left
               (min1.x >= min2.x && min1.x <= max2.x && max1.y >= min2.y && max1.y <= max2.y) || //top left
               (max1.x >= min2.x && max1.x <= max2.x && max1.y >= min2.y && max1.y <= max2.y) || //top right
               (max1.x >= min2.x && max1.x <= max2.x && min1.y >= min2.y && min1.y <= max2.y); // bottom right
    }
}