using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace com.Sal77.GameVariables
{
    public static class GameVariableUtility
    {
        private class StatCacheInfo
        {
            public string[] Names;
            public Dictionary<string, FieldInfo> Fields;
        }

        private static readonly Dictionary<Type, StatCacheInfo> s_simpleStatCache = new();
        private static readonly Dictionary<Type, StatCacheInfo> s_scalableStatCache = new();

        public static string[] AvailableSimpleStatsFromType<T>()
        {
            return AvailableSimpleStatsFromType(typeof(T));
        }

        public static string[] AvailableSimpleStatsFromType(Type type)
        {
            if (s_simpleStatCache.TryGetValue(type, out var cached))
                return cached.Names;

            List<string> result = new List<string>();
            Dictionary<string, FieldInfo> fieldDict = new();

            FieldInfo[] fields = type.GetFields(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic
            );

            foreach (FieldInfo field in fields)
            {
                Type fieldType = field.FieldType;

                if (IsCustomSimpleStat(fieldType))
                {
                    string name = GetStatName(field);
                    result.Add(name);
                    fieldDict[name] = field;
                }
            }

            string[] array = result.ToArray();
            s_simpleStatCache[type] = new StatCacheInfo { Names = array, Fields = fieldDict };
            return array;
        }

        public static string[] AvailableScalableStatsFromType<T>()
        {
            return AvailableScalableStatsFromType(typeof(T));
        }

        public static string[] AvailableScalableStatsFromType(Type type)
        {
            if (s_scalableStatCache.TryGetValue(type, out var cached))
                return cached.Names;

            List<string> result = new List<string>();
            Dictionary<string, FieldInfo> fieldDict = new();

            FieldInfo[] fields = type.GetFields(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic
            );

            foreach (FieldInfo field in fields)
            {
                if (IsCustomScalableStat(field.FieldType))
                {
                    string name = GetStatName(field);
                    result.Add(name);
                    fieldDict[name] = field;
                }
            }

            string[] array = result.ToArray();
            s_scalableStatCache[type] = new StatCacheInfo { Names = array, Fields = fieldDict };
            return array;
        }

        private static bool IsCustomSimpleStat(Type type)
        {
            return type.IsGenericType &&
                   type.GetGenericTypeDefinition() == typeof(GameSimpleStat<>);
        }

        private static bool IsCustomScalableStat(Type type)
        {
            return type.IsGenericType &&
                   type.GetGenericTypeDefinition() == typeof(GameSimpleStat<>) &&
                   type.GetGenericArguments()[0] == typeof(float);
        }

        private static string GetStatName(FieldInfo field)
        {
            var attr = field.GetCustomAttribute<StatNameOverrideAttribute>();
            return attr != null ? attr.NameOverride : field.Name;
        }

        public static GameSimpleStat<T> SimpleStatFromInstance<T>(object instance, string statName)
        {
            Type type = instance.GetType();
            if (!s_simpleStatCache.TryGetValue(type, out var cache))
            {
                AvailableSimpleStatsFromType(type); // Ensure cache is populated
                cache = s_simpleStatCache[type];
            }

            if (cache.Fields.TryGetValue(statName, out var field))
            {
                return (GameSimpleStat<T>)field.GetValue(instance);
            }

            throw new ArgumentException($"Stat '{statName}' not found in type {type.Name}");
        }

        public static GameScalableStat ScalableStatFromInstance(object instance, string statName)
        {
            Type type = instance.GetType();
            if (!s_scalableStatCache.TryGetValue(type, out var cache))
            {
                AvailableScalableStatsFromType(type); // Ensure cache is populated
                cache = s_scalableStatCache[type];
            }

            if (cache.Fields.TryGetValue(statName, out var field))
            {
                return (GameScalableStat)field.GetValue(instance);
            }

            throw new ArgumentException($"Stat '{statName}' not found in type {type.Name}");
        }
    }
}
