using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

/// <summary>
/// The extension for ensuring a string field of a element in a list is unique.
/// </summary>
public static class MakeUniqueStringListExtension
{
    private static readonly Regex s_suffixRegex = new Regex(@"^(.*)\s*\((\d+)\)$");
    
    /// <summary>
    /// Makes a unique name by checking against existing items using the selector
    /// </summary>
    public static string MakeUnique<T>(this IEnumerable<T> list, Func<T, string> selector, string name)
    {
        if(name == string.Empty) return "";
        var existingNames = list.Select(selector).ToList();
        
        //No match, it's unique
        if (!existingNames.Contains(name))
            return name;
        
        //We found a match so we gotta start trying to make it unique
        var match = s_suffixRegex.Match(name);
        int nameNumber;
        
        if (match.Success)
        {
            //It's a variation that needs a subsequent number
            nameNumber = int.Parse(match.Groups[2].Value) + 1;
        }
        else
        {
            //It's the very first variation
            nameNumber = 1;
        }
        
        int counter = nameNumber;
        string nameCandidate;
        do
        {
            nameCandidate = $"{name} ({counter})";
            counter++;
        } while (existingNames.Contains(nameCandidate));
        
        return nameCandidate;
    }
}
