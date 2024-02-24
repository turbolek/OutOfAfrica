using System.Collections.Generic;

public static class ListExtensions
{
    public static void AddExclusive<T>(this List<T> list, T element)
    {
        if (!list.Contains(element))
        {
            list.Add(element);
        }
    }

    public static void AddExclusiveRange<T>(this List<T> list, List<T> elements)
    {
        foreach (var element in elements)
        {
            list.AddExclusive(element);
        }
    }
}