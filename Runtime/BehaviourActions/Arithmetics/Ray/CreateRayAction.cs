using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourCategory("Arithmetics/Ray/Create Ray Action")]
public class CreateRayAction : BehaviourAction
{
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        actionContext.DeclareVariable<Vector3>("Position");
        actionContext.DeclareVariable<Vector3>("Direction");
        actionContext.DeclareVariable<Ray>("Result");
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var position = executionContext.ReadVariable<Vector3>("Position");
        var direction = executionContext.ReadVariable<Vector3>("Direction");

        var newRay = new Ray(position, direction);

        executionContext.WriteVariable<Ray>("Result", newRay);
        return ExecutionActionResult.Successful;
    }

    protected override string GetActionName()
    {
        return "Create Ray Action";
    }
}
