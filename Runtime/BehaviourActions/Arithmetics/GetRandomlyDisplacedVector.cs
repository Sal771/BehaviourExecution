using com.Sal77.BehaviourExecution;

[BehaviourCategory("Arithmetics/Get Randomly Displaced Vector")]
public class GetRandomlyDisplacedVector : BehaviourAction
{
    public override void DefineBindings(IBehaviourVariable executionContext)
    {
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        return ExecutionActionResult.Successful;
    }

    protected override string GetActionName()
    {
        return "Get Randomly Displaced Vector";
    }
}
