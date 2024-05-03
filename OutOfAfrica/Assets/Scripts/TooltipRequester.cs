using System;

public class TooltipRequester
{
    public static Action<TooltipRequester, string, string> TooltipShowRequested;
    public static Action<TooltipRequester> TooltipHideRequested;

    public void RequestShow(string title, string message)
    {
        TooltipShowRequested?.Invoke(this, title, message);
    }

    public void RequestHide()
    {
        TooltipHideRequested?.Invoke(this);
    }
}