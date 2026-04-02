using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    [CreateAssetMenu(fileName = "New Behaviour Object", menuName = "Behaviour Execution/Behaviour Object")]
    public class BehaviourObject : ScriptableObject
    {
        public BehaviourEvent[] BehaviourEvents => m_behaviourEvents.ToArray();
        public BehaviourAction[] BehaviourActions => m_behaviourActionBuffer.BehaviourActions;
        public BehaviourActionBuffer BehaviourActionBuffer => m_behaviourActionBuffer;
        public BehaviourBlackboard[] BehaviourBlackboards => m_behaviourBlackboards.ToArray();
        [SerializeReference] private List<BehaviourEvent> m_behaviourEvents = new();
        [SerializeField] private BehaviourActionBuffer m_behaviourActionBuffer = new("Actions");
        [SerializeReference] private List<BehaviourBlackboard> m_behaviourBlackboards = new();

        public void AddAction(BehaviourAction behaviourAction)
        {
            m_behaviourActionBuffer.Add(behaviourAction);
        }
        public void AddEvent(BehaviourEvent behaviourEvent)
        {
            m_behaviourEvents.Add(behaviourEvent);
        }

        public void AddBlackboard(BehaviourBlackboard behaviourBlackboard)
        {
            m_behaviourBlackboards.Add(behaviourBlackboard);
        }

        public void RemoveAction(BehaviourAction behaviourAction)
        {
            m_behaviourActionBuffer.Remove(behaviourAction);
        }
        public void RemoveEvent(BehaviourEvent behaviourEvent)
        {
            m_behaviourEvents.Remove(behaviourEvent);
        }

        public void RemoveBlackboard(BehaviourBlackboard behaviourBlackboard)
        {
            m_behaviourBlackboards.Remove(behaviourBlackboard);
        }

        public void InsertEventTo(BehaviourEvent behaviourEvent, int index)
        {
            m_behaviourEvents.Insert(index, behaviourEvent);
        }

        public void InsertActionTo(BehaviourAction behaviourAction, int index)
        {
            m_behaviourActionBuffer.InsertTo(behaviourAction, index);
        }

        public void Validate()
        {
            var removedBlackboards = m_behaviourBlackboards.RemoveDuplicates();
            var removedEvents = m_behaviourEvents.RemoveDuplicates((source, index) => {return source.EventName;});

            m_behaviourBlackboards.RemoveAll(x => x == null);

            #if UNITY_EDITOR

            bool removed = false;

            foreach(var removedEvent in removedEvents)
            {
                AssetDatabase.RemoveObjectFromAsset(removedEvent);

                DestroyImmediate(removedEvent, true);

                removed = true;
            }

            if (removed)
            {
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
            }
            #endif
        }
    }
}
