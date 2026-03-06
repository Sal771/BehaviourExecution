using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    public interface IBehaviourExecution
    {
       public void WriteVariable<T>(BehaviourBinding variableReference, T value);
       public T ReadVariable<T>(BehaviourBinding variableReference);
    }
}