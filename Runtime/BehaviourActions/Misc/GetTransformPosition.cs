using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourCategory("Misc/Get Transform Position")]
public class GetTransformPosition : BehaviourAction
{
    private BehaviourBinding m_transformReference;
    private BehaviourBinding m_resultReference;

    public override void DefineBindings(IBehaviourVariable executionContext)
    {
        m_transformReference = executionContext.DeclareVariable<Transform>("Transform");
        m_resultReference = executionContext.DeclareVariable<Vector3>("Result");
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var transform = executionContext.ReadVariable<Transform>(m_transformReference);
        
        executionContext.WriteVariable(m_resultReference, transform.position);

        return ExecutionActionResult.Successful;
    }

    protected override string GetActionName()
    {
        return "Get Transform Position";
    }
}
