using com.Sal77.BehaviourExecution;

[BehaviourCategory("Arithmetics/Get Ray Rotated")]
public class GetRayRotated : BehaviourAction
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
        return "Get Ray Rotated";
    }
}
