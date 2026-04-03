using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    public interface IBehaviourAction
    {
       public void DeclareVariable<T>(string name, IBehaviourActionReadMode readMode);
       public T DeclareEnum<T>(string name);
       public void DeclareActionBuffer(string name);
       public int DeclareNumberOption(string name);
    }
}