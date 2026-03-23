using System;

public class BehaviourTypeCategoryAttribute : Attribute
{
    public string Category;
    public string DisplayName;
    public BehaviourTypeCategoryAttribute(string category, string displayName)
    {
        Category = category;
        DisplayName = displayName;
    }
}