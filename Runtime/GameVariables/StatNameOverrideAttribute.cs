using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace com.Sal77.GameVariables
{
    public class StatNameOverrideAttribute : Attribute
    {
        public string NameOverride;
        public StatNameOverrideAttribute(string nameOverride)
        {
            NameOverride = nameOverride;
        }
    }
}
