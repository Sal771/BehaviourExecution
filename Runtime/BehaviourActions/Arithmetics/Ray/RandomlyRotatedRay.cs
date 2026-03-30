using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourCategory("Arithmetics/Ray/Randomly Rotated Ray")]
public class GetRayRotated : BehaviourAction
{
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        actionContext.DeclareVariable<Ray>("Position", IBehaviourActionReadMode.Input);
        actionContext.DeclareVariable<Vector2>("RandomAngle", IBehaviourActionReadMode.Input);
        actionContext.DeclareVariable<Ray>("Output", IBehaviourActionReadMode.Output);
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var ray = executionContext.ReadVariable<Ray>("Position");
        var randomAngle = executionContext.ReadVariable<Vector2>("RandomAngle");

        float rotationX = Random.Range(-randomAngle.x, randomAngle.x);
        float rotationY = Random.Range(-randomAngle.y, randomAngle.y);
        
        var rotation = Quaternion.Euler(rotationX, rotationY, 0);
        var newDirection = rotation * ray.direction;
        var result = new Ray(ray.origin, newDirection);

        executionContext.WriteVariable<Ray>("Output", result);
        return ExecutionActionResult.Successful;
    }

    protected override string GetActionName()
    {
        return "Randomly Rotated Ray";
    }
}
