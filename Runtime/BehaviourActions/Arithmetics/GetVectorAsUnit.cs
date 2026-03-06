using com.Sal77.BehaviourExecution;

[BehaviourCategory("Arithmetics/Get Vector As Unit")]
public class GetVectorAsUnit : BehaviourAction
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
        return "Get Vector As Unit";
    }
}
