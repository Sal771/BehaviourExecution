using System;

public class BehaviourTypeDescription
{
    public string Category => m_category;
    public string DisplayName => m_displayName;
    public Type Type => m_type;

    private string m_category;
    private string m_displayName;
    private Type m_type;
    public BehaviourTypeDescription(Type type, string category, string displayName)
    {
        m_category = category;
        m_displayName = displayName;
        m_type = type;
    }
}