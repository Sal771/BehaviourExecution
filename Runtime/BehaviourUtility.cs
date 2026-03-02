using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace com.Sal77.BehaviourExecution
{
    public static class BehaviourUtility
    {
        private static readonly Dictionary<string, Type> s_actionsCache = new();
        private static readonly Dictionary<string, Type> s_eventsCache = new();
        private static readonly Dictionary<string, string> s_actionsCategorizedCache = new();
        private static readonly Dictionary<string, string> s_eventsCategorizedCache = new();

        public static Type ActionTypeFromName(string actionName)
        {
            InitializeActionsCache();
            if (s_actionsCache.TryGetValue(actionName, out var type))
            {
                return type;
            }
            throw new ArgumentException($"Action '{actionName}' not found");
        }

        public static Type EventTypeFromName(string eventName)
        {
            InitializeEventsCache();
            if (s_eventsCache.TryGetValue(eventName, out var type))
            {
                return type;
            }
            throw new ArgumentException($"Event '{eventName}' not found");
        }

        public static Type ActionTypeFromCategorized(string actionCategorized)
        {
            InitializeActionsCache();
            foreach(var categorized in s_actionsCategorizedCache)
            {
                Debug.Log($"Categorized '{categorized.Key}' '{categorized.Value}'");
            }
            foreach(var normal in s_actionsCache)
            {
                Debug.Log($"Normal '{normal.Key}' '{normal.Value}'");
            }
            if (s_actionsCategorizedCache.TryGetValue(actionCategorized, out var name))
            {
                return ActionTypeFromName(name);
            }
            throw new ArgumentException($"Categorized action '{actionCategorized}' not found");
        }

        public static Type EventTypeFromCategorized(string eventCategorized)
        {
            InitializeEventsCache();
            if (s_eventsCategorizedCache.TryGetValue(eventCategorized, out var name))
            {
                return EventTypeFromName(name);
            }
            throw new ArgumentException($"Categorized event '{eventCategorized}' not found");
        }

        public static string[] AllAvailableActions()
        {
            InitializeActionsCache();
            return s_actionsCache.Keys.ToArray();
        }

        public static string[] AllAvailableEvents()
        {
            InitializeEventsCache();
            return s_eventsCache.Keys.ToArray();
        }

        public static string[] AllAvailableActionsCategorized()
        {
            InitializeActionsCache();
            return s_actionsCategorizedCache.Keys.ToArray();
        }

        public static string[] AllAvailableEventsCategorized()
        {
            InitializeEventsCache();
            return s_eventsCategorizedCache.Keys.ToArray();
        }
        public static string ActionNameFromCategorized(string categorizedName)
        {
            InitializeActionsCache();
            return s_actionsCategorizedCache[categorizedName];
        }

        public static string EventNameFromCategorized(string categorizedName)
        {
            InitializeEventsCache();
            return s_eventsCategorizedCache[categorizedName];
        }

        private static void InitializeActionsCache()
        {
            if (s_actionsCache.Count == 0)
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    var types = assembly.GetTypes();
                    foreach (var type in types)
                    {
                        if (type.IsAbstract || !type.IsClass) continue;
                        if (typeof(BehaviourAction).IsAssignableFrom(type))
                        {
                            string name = type.Name;
                            s_actionsCache[name] = type;
                            var attr = type.GetCustomAttribute<BehaviourCategoryAttribute>();
                            if (attr != null)
                            {
                                s_actionsCategorizedCache[attr.Category] = name;
                            }
                            else
                            {
                               s_actionsCategorizedCache[name] = name;
                            }
                        }
                    }
                }
            }
        }

        private static void InitializeEventsCache()
        {
            if (s_eventsCache.Count == 0)
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    var types = assembly.GetTypes();
                    foreach (var type in types)
                    {
                        if (type.IsAbstract || !type.IsClass) continue;
                        if (typeof(BehaviourEvent).IsAssignableFrom(type))
                        {
                            string name = type.Name;
                            s_eventsCache[name] = type;
                            var attr = type.GetCustomAttribute<BehaviourCategoryAttribute>();
                            if (attr != null)
                            {
                                s_eventsCategorizedCache[attr.Category] = name;
                            }
                            else
                            {
                                s_eventsCategorizedCache[name] = name;
                            }
                        }
                    }
                }
            }
        }
    }
}
