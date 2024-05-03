using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private RectTransform _content;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _message;

    private TooltipRequester _currentRequester;
    private OverlapFixRequester _overlapFixRequester;


    private void Start()
    {
        _overlapFixRequester = new OverlapFixRequester();
        TooltipRequester.TooltipShowRequested += OnShowRequested;
        TooltipRequester.TooltipHideRequested += OnHideRequested;
        Hide();
    }

    private void OnDestroy()
    {
        TooltipRequester.TooltipShowRequested -= OnShowRequested;
        TooltipRequester.TooltipHideRequested -= OnHideRequested;
    }

    private void OnShowRequested(TooltipRequester requester, string title, string message)
    {
        _currentRequester = requester;
        _content.gameObject.SetActive(true);
        _title.text = title;
        _message.text = message;
        _overlapFixRequester.RequestFixSubscribe(_content, Input.mousePosition);
    }

    private void OnHideRequested(TooltipRequester requester)
    {
        if (requester == _currentRequester)
        {
            Hide();
        }
    }

    private void Hide()
    {
        _currentRequester = null;
        _content.gameObject.SetActive(false);
        _overlapFixRequester.RequestFixUnsubscribe(_content);
    }
}