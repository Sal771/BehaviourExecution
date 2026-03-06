using UnityEngine;
using System;
using System.Collections.Generic;

namespace com.Sal77.GameVariables
{
    [Serializable]
    public class GameList<T>
    {
        [SerializeField] private List<T> m_list = new List<T>();

        public Action OnValueChanged;
        public T[] Value => ToArray();

        public int Count => m_list.Count;
        public T this[int index]
        {
            get => m_list[index];
            set
            {
                m_list[index] = value;
                OnValueChanged?.Invoke();
            }
        }

        public void Add(T item)
        {
            m_list.Add(item);
            OnValueChanged?.Invoke();
        }

        public bool Remove(T item)
        {
            bool result = m_list.Remove(item);
            if (result) OnValueChanged?.Invoke();
            return result;
        }

        public void Clear()
        {
            if (m_list.Count == 0) return;
            m_list.Clear();
            OnValueChanged?.Invoke();
        }

        public bool Contains(T item) => m_list.Contains(item);

        public int IndexOf(T item) => m_list.IndexOf(item);

        public void AddRange(IEnumerable<T> items)
        {
            m_list.AddRange(items);
            OnValueChanged?.Invoke();
        }

        public List<T>.Enumerator GetEnumerator() => m_list.GetEnumerator();

        public T[] ToArray() => m_list.ToArray();
    }
}