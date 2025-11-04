using System.Collections.Generic;
using UnityEngine;

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

    public static void ClearNulls<T>(this List<T> list) where T : Component
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i] == null)
            {
                list.RemoveAt(i);
            }
        }
    }

    public static void DestroyElements<T>(this List<T> list, bool clear = false) where T : MonoBehaviour
    {
        list.ClearNulls();
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i] == null)
            {
                GameObject.Destroy(list[i].gameObject);
            }
        }

        if (clear)
        {
            list.Clear();
        }
    }
}