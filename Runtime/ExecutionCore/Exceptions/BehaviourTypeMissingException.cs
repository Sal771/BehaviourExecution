using System;

public class BehaviourTypeMissingException : Exception
{
    public BehaviourTypeMissingException() : base()
    {
        
    }
    public BehaviourTypeMissingException(string message) : base(message)
    {
        
    }
}