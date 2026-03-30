using System;
using com.Sal77.BehaviourExecution;

[BehaviourCategory("Control/Do While")]
public class DoWhileCycleAction : BehaviourAction
{
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        //TODO FIll out this
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