using com.Sal77.BehaviourExecution;

[BehaviourCategory("Control/Execute Behaviour Object")]
public class ExecuteBehaviourObjectAction : BehaviourAction
{
    public override void DefineBindings(IBehaviourVariable actionContext)
    {
        
    }

    protected override string GetActionName()
    {
        return "Execute Behaviour Object";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        return ExecutionActionResult.Successful;
    }
}