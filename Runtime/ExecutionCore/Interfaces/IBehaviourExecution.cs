using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    public interface IBehaviourExecution
    {
       public void WriteVariable<T>(string variableName, T value);
       public T ReadVariable<T>(string variableName);
       public void ExecuteActionBuffer(string bufferName);
    }
}