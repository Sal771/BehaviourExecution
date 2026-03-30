using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourCategory("Arithmetics/Get Ray Rotated Around Point")]
public class GetRayRotatedAroundPointAction : BehaviourAction
{
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        actionContext.DeclareVariable<Ray>("Ray", IBehaviourActionReadMode.Input);
        actionContext.DeclareVariable<Vector3>("Pivot Position", IBehaviourActionReadMode.Input);
        actionContext.DeclareVariable<Quaternion>("Rotation", IBehaviourActionReadMode.Input);
        actionContext.DeclareVariable<Ray>("Output", IBehaviourActionReadMode.Output);
    }

    protected override string GetActionName()
    {
        return "Get Ray Rotated Around Point";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var ray = executionContext.ReadVariable<Ray>("Ray");
        var pivotPosition = executionContext.ReadVariable<Vector3>("Pivot Position");
        var rotation = executionContext.ReadVariable<Quaternion>("Rotation");

        var direction = ray.direction;
        var rotatedDirection = rotation * direction;
        var newOrigin = pivotPosition + rotatedDirection;
        
        var result = new Ray(newOrigin, rotatedDirection);
        executionContext.WriteVariable<Ray>("Output", result);

        return ExecutionActionResult.Successful;
    }
}
