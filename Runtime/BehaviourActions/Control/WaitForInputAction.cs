using com.Sal77.BehaviourExecution;

[BehaviourCategory("Control/Wait For Input")]
public class WaitForInputAction : BehaviourAction
{
    public override void DefineBindings(IBehaviourVariable actionContext)
    {
        
    }

    protected override string GetActionName()
    {
        return "Wait For Input";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        return ExecutionActionResult.Successful;
    }
}