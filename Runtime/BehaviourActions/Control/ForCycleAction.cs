using com.Sal77.BehaviourExecution;

[BehaviourCategory("Control/For Cycle")]
public class ForCycleAction : BehaviourAction
{
    public override void DefineBindings(IBehaviourVariable actionContext)
    {
        
    }

    protected override string GetActionName()
    {
        return "For Cycle";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        return ExecutionActionResult.Successful;
    }
}