using System;
using com.Sal77.BehaviourExecution;

[BehaviourCategory("Control/Do While")]
public class DoWhileCycleAction : BehaviourAction
{
    public override void DefineBindings(IBehaviourVariable actionContext)
    {
        
    }

    protected override string GetActionName()
    {
        return "Do While";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        return ExecutionActionResult.Successful;
    }
}