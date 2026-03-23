using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Extension class for faster retrieval of a random element from a list.
/// </summary>
public static class ChooseRandomListExtension
{
    private class WeightedEntry<T>
    {
        public T Value;
        public float Weight;
    }

    /// <summary>
    /// Chooses a single random element from the list.
    /// </summary>
    public static T ChooseRandom<T>(this IEnumerable<T> list)
    {
        var result = ChooseRandom(list, 1);
        return result.Length > 0 ? result[0] : default;
    }

    /// <summary>
    /// Chooses up to maxAmount random elements from the list.
    /// </summary>
    public static T[] ChooseRandom<T>(this IEnumerable<T> list, int maxAmount)
    {
        var enumerable = list.ToList();
        
        if (enumerable.Count <= maxAmount)
        {
            //We already have the same or less amount of elements requested
            return list.ToArray();
        }
        
        var indices = Enumerable.Range(0, enumerable.Count).ToList();
        
        // Fisher-Yates shuffle
        for (int i = indices.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (enumerable[i], enumerable[j]) = (enumerable[j], enumerable[i]);
        };
        
        return enumerable.ToArray();
    }

    /// <summary>
    /// Choose  a single random element from the list with a weight function.
    /// </summary>
    public static T ChooseRandomWeighted<T>(this IEnumerable<T> list, Func<T, float> weightFunction)
    {
        var result = ChooseRandomWeighted(list, weightFunction, 1);
        return result.Length > 0 ? result[0] : default;
    }

    /// <summary>
    /// Choose  a single random element from the list with a weight function.
    /// </summary>
    public static T[] ChooseRandomWeighted<T>(this IEnumerable<T> list, Func<T, float> weightFunction, int maxAmount)
    {
        var enumerable = list.ToList();
        
        if (enumerable.Count <= maxAmount)
        {
            //We already have the same or less amount of elements requested
            return list.ToArray();
        }

        // Setup.
        
        float totalWeight = 0f;
        List<WeightedEntry<T>> availableValues = new();
        List<T> result = new();
        
        foreach(var entry in list){
            WeightedEntry<T> weightedEntry = new();
            float weight = Mathf.Max(0f, weightFunction(entry));
            weightedEntry.Value = entry;
            weightedEntry.Weight = weight;
            totalWeight += weight;

            availableValues.Add(weightedEntry);
        }
        
        // Picking the random values
        for (int i = 0; i < maxAmount; i++)
        {
            float randomValue = UnityEngine.Random.Range(0f, totalWeight);
            float cumulativeWeight = 0f;
            int j = 0;
            WeightedEntry<T> currentValue = default;

            while(randomValue > cumulativeWeight)
            {
                currentValue = availableValues[j];
                cumulativeWeight += currentValue.Weight;
                j++;
            }
            
            //Adding to result
            totalWeight -= currentValue.Weight;
            availableValues.Remove(currentValue);
            result.Add(currentValue.Value);
        }
        
        return result.ToArray();
    }
}
