using com.Sal77.BehaviourExecution;

[BehaviourCategory("Arithmetics/Get Randomized Unit Vector")]
public class GetRandomizedUnitVector : BehaviourAction
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
        return "Get Randomized Unit Vector";
    }
}
