using com.Sal77.BehaviourExecution;

[BehaviourCategory("Arithmetics/Get Two Vectors Direction")]
public class GetTwoVectorsDirectionAction : BehaviourAction
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
        return "Get Two Vectors Direction";
    }
}
