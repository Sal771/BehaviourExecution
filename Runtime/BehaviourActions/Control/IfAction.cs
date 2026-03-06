using com.Sal77.BehaviourExecution;

[BehaviourCategory("Control/If Action")]
public class IfAction : BehaviourAction
{
    public override void DefineBindings(IBehaviourVariable actionContext)
    {
        
    }

    protected override string GetActionName()
    {
        return "If Action";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        return ExecutionActionResult.Successful;
    }
}