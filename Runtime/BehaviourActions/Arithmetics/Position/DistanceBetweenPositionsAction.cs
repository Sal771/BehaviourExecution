using com.Sal77.BehaviourExecution;

using UnityEngine;

[BehaviourCategory("Arithmetics/Position/Distance Between Positions")]
public class DistanceBetweenPositionsAction : BehaviourAction
{
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        actionContext.DeclareVariable<Vector3>("PositionA", IBehaviourActionReadMode.Input);
        actionContext.DeclareVariable<Vector3>("PositionB", IBehaviourActionReadMode.Input);
        actionContext.DeclareVariable<float>("Result", IBehaviourActionReadMode.Output);
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var vectorA = executionContext.ReadVariable<Vector3>("PositionA");
        var vectorB = executionContext.ReadVariable<Vector3>("PositionB");
        
        float distance = Vector3.Distance(vectorA, vectorB);
        executionContext.WriteVariable("Result", distance);

        return ExecutionActionResult.Successful;
    }

    protected override string GetActionName()
    {
        return "Distance Between Positions";
    }
}
