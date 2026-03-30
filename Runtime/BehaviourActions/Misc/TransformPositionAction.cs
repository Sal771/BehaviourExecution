using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourCategory("Misc/Transform Position")]
public class TransformPositionAction : BehaviourAction
{
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        actionContext.DeclareVariable<Transform>("Transform", IBehaviourActionReadMode.Input);
        actionContext.DeclareVariable<Vector3>("Result", IBehaviourActionReadMode.Input);
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var transform = executionContext.ReadVariable<Transform>("Transform");
        
        executionContext.WriteVariable<Vector3>("Result", transform.position);

        return ExecutionActionResult.Successful;
    }

    protected override string GetActionName()
    {
        return "Transform Position";
    }
}
