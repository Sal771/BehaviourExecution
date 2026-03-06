using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourCategory("Misc/Get GameObject Transform")]
public class GetGameObjectTransform : BehaviourAction
{
    private BehaviourBinding m_gameObjectReference;
    private BehaviourBinding m_resultReference;

    public override void DefineBindings(IBehaviourVariable executionContext)
    {
        m_gameObjectReference = executionContext.DeclareVariable<GameObject>("GameObject");
        m_resultReference = executionContext.DeclareVariable<Transform>("Result");
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var gameObject = executionContext.ReadVariable<GameObject>(m_gameObjectReference);
        
        executionContext.WriteVariable(m_resultReference, gameObject.transform);

        return ExecutionActionResult.Successful;
    }

    protected override string GetActionName()
    {
        return "Get GameObject Transform";
    }
}
