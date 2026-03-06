using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    public interface IBehaviourVariable
    {
       public BehaviourBinding DeclareVariable<T>(string name);
       public BehaviourBinding DeclareVariable(string name, Type type);
    }
}