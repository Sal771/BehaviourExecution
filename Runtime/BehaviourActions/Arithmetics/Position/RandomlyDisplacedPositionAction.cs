using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourCategory("Arithmetics/Randomly Displaced Position")]
public class RandomlyDisplacedPositionAction : BehaviourAction
{
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        actionContext.DeclareVariable<Vector3>("Position");
        actionContext.DeclareVariable<Vector3>("Random Angles");
        actionContext.DeclareVariable<Vector3>("Result");
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var position = executionContext.ReadVariable<Vector3>("Position");
        var weights = executionContext.ReadVariable<Vector3>("Random Angles");

        float displacementX = Random.Range(-weights.x, weights.x);
        float displacementY = Random.Range(-weights.y, weights.y);
        float displacementZ = Random.Range(-weights.z, weights.z);
        Vector3 result = position + new Vector3(displacementX, displacementY, displacementZ);

        executionContext.WriteVariable<Vector3>("Result", result);

        return ExecutionActionResult.Successful;
    }

    protected override string GetActionName()
    {
        return "Randomly Displaced Position";
    }
}
