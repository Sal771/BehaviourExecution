using System;
using System.Collections.Generic;
using System.Linq;
using com.Sal77.BehaviourExecution;
using UnityEngine;

public abstract class BehaviourStateMachine<EventT, ComponentT> : MonoBehaviour where EventT : BehaviourEvent where ComponentT : Component
{
    public StateEntry[] UnitStates => m_unitStates.ToArray();
    [SerializeField] private List<StateEntry> m_unitStates = new();
    [SerializeReference] private StateEntry m_currentState;
    private ExecutionRunner m_executionRunner;
    private void Start()
    {
        m_executionRunner = GetComponent<ExecutionRunner>();
    }
    private void Update()
    {
        if(m_currentState == null)
        {
            m_currentState = m_unitStates.ChooseRandomWeighted(x => x.Weight);

            var value = GetComponent<ComponentT>();

            m_executionRunner.ExecuteDirectly(m_currentState.BehaviourObject, typeof(EventT), value);
        }
    }

    [Serializable]
    public class StateEntry
    {
        [SerializeReference] public BehaviourObject BehaviourObject;
        public float Weight;
        [SerializeReference] public List<StateEntry> NextStates;
    }
}