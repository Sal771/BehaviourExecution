using com.Sal77.BehaviourExecution;

[BehaviourCategory("Arithmetics/Math Basic Operator")]
public class MathBasicOperatorAction : BehaviourAction
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
        return "Math Basic Operator";
    }
}
