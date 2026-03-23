using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace com.Sal77.BehaviourExecution
{
    public class BehaviourCategoryAttribute : Attribute
    {
        public string Category;
        public BehaviourCategoryAttribute(string category)
        {
            Category = category;
        }
    }
}
