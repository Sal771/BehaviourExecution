using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourCategory("Arithmetics/Ray/Relatively Displaced Ray")]
public class RelativelyDisplacedRay : BehaviourAction
{
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        actionContext.DeclareVariable<Ray>("Position");
        actionContext.DeclareVariable<Vector2>("RandomAngle");//TODO Add a way to convert between types
        actionContext.DeclareVariable<Ray>("Output");
    }
    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var ray = executionContext.ReadVariable<Ray>("Position");
        var randomAngle = executionContext.ReadVariable<Vector2>("RandomAngle");

        float displacementX = Random.Range(-randomAngle.x, randomAngle.x);
        float displacementY = Random.Range(-randomAngle.y, randomAngle.y);
        
        var newOrigin = ray.origin + new Vector3(displacementX, displacementY, 0);
        var result = new Ray(newOrigin, ray.direction);

        executionContext.WriteVariable<Ray>("Output", result);
        return ExecutionActionResult.Successful;
    }

    protected override string GetActionName()
    {
        return "Relatively Displaced Ray";
    }
}
