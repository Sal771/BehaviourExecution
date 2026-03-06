using com.Sal77.BehaviourExecution;

[BehaviourCategory("Control/While Cycle")]
public class WhileCycleAction : BehaviourAction
{
    public override void DefineBindings(IBehaviourVariable actionContext)
    {
    }

    protected override string GetActionName()
    {
        return "While Cycle";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {

        return ExecutionActionResult.Successful;
    }
}