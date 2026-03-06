using com.Sal77.BehaviourExecution;

[BehaviourCategory("Arithmetics/Get Relatively Displaced Vector")]
public class GetRelativelyDisplacedVector : BehaviourAction
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
        return "Get Relatively Displaced Vector";
    }
}
