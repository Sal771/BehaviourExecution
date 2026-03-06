using UnityEngine;
using System;
using System.Collections.Generic;

namespace com.Sal77.GameVariables
{
    [Serializable]
    public class GameScalableStat : GameSimpleStat<float>
    {
        new public float Value {
            get => GetValue();
        }
        public float BaseValue {
            get => m_value;
            set
            {
                m_value = value;

                OnValueChanged?.Invoke(GetValue());
            }
        }
        [SerializeField] private List<Modifier> addModifiers = new();
        [SerializeField] private List<Modifier> multModifiers = new();
        private float GetValue()//Todo, cache value and then uhhh yeah I get it 
        {
            float value = m_value;

            foreach (var add in addModifiers)
                value += add.Value;

            float multiplier = 1f;
            foreach (var mult in multModifiers)
                multiplier *= mult.Value;

            return value * multiplier;
        }

        public IReadOnlyList<Modifier> GetAddModifiers() => addModifiers;

        public void AddAdditive(float amount, string source)
        {
            RemoveAdditive(source);
            addModifiers.Add(new Modifier(amount, source));
            OnValueChanged?.Invoke(GetValue());
        }

        public void RemoveAdditive(string source)
        {
            addModifiers.RemoveAll(x => x.Source == source);
            OnValueChanged?.Invoke(GetValue());
        }

        public IReadOnlyList<Modifier> GetMultModifiers() => multModifiers;

        public void AddMultiplicative(float amount, string source)
        {
            RemoveMultiplicative(source);
            multModifiers.Add(new Modifier(amount, source));
            OnValueChanged?.Invoke(GetValue());
        }

        public void RemoveMultiplicative(string source)
        {
            multModifiers.RemoveAll(x => x.Source == source);
            OnValueChanged?.Invoke(GetValue());
        }

        [Serializable]
        public class Modifier
        {
            public float Value;
            public string Source;

            public Modifier(float value, string source)
            {
                Value = value;
                Source = source;
            }
        }
    }
}