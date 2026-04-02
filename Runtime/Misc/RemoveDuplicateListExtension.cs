using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RemoveDuplicateListExtension
{
    public static TSource[] RemoveDuplicates<TSource, TResult>(this List<TSource> list, Func<TSource,int,TResult> predicate)
    {
        HashSet<TSource> removedInstances = new();
        for(int i=0; i<list.Count(); i++)
        {
            for(int j = list.Count() - 1; j>0; j--)
            {   
                if(i==j) continue;

                if (predicate.Invoke(list[i], i).Equals( predicate.Invoke(list[j], j) ))
                {
                    removedInstances.Add(list[j]);
                    list.RemoveAt(j);
                }
            }
        }
        return removedInstances.ToArray();
    }

    public static TSource[] RemoveDuplicates<TSource>(this List<TSource> list)
    {
        HashSet<TSource> removedInstances = new();

        for(int i=0; i<list.Count(); i++)
        {
            for(int j = list.Count() - 1; j>0; j--)
            {
                if(i==j || list[i] == null || list[j] == null) continue;

                if(list[i].Equals(list[j])){
                    removedInstances.Add(list[j]);
                    list.RemoveAt(j);
                }
            }
        }
        return removedInstances.ToArray();
    }
}
