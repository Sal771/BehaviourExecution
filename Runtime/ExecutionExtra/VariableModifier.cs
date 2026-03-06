using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    public class VariableModifier
    {
        //Target
        public string TargetVariableName {get => m_targetVariableName; set => m_targetVariableName = value; }
        //Modifier
        public float AddModifier => m_addModifier;
        public float MultModifier => m_multModifier;

        [Header("Target")]
        private string m_targetVariableName;
        [Header("Modifier")]
        [Tooltip("The value added to the variable before multiplication.")]
        [SerializeField] private float m_addModifier = 0;
        [Tooltip("The value the variable will be multiplied by. 1 means no change.")]
        [SerializeField] private float m_multModifier = 1;
    }
}