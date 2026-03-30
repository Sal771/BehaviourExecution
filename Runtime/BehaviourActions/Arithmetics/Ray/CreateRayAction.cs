using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourCategory("Arithmetics/Ray/Create Ray Action")]
public class CreateRayAction : BehaviourAction
{
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        actionContext.DeclareVariable<Vector3>("Position", IBehaviourActionReadMode.Input);
        actionContext.DeclareVariable<Vector3>("Direction", IBehaviourActionReadMode.Input);
        actionContext.DeclareVariable<Ray>("Result", IBehaviourActionReadMode.Output);
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
