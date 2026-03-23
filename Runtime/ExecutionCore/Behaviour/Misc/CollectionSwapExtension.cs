using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class CollectionSwapExtension
{
    public static void InsertTo<T>(this List<T> list, T instance, int targetIndex)
    {
        int currentIndex = list.IndexOf(instance);
        if (currentIndex == -1) return;
        
        if (currentIndex == targetIndex) return;
        
        targetIndex = Mathf.Max(0, Mathf.Min(targetIndex, list.Count - 1));
        
        list.RemoveAt(currentIndex);
        
        list.Insert(targetIndex, instance);
    }
}
