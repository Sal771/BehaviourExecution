using com.Sal77.BehaviourExecution;

[BehaviourCategory("Control/If Action")]
public class IfAction : BehaviourAction
{
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        actionContext.DeclareVariable<bool>("Condition", IBehaviourActionReadMode.Input);
        actionContext.DeclareActionBuffer("True Actions");
        actionContext.DeclareActionBuffer("False Actions");
    }

    protected override string GetActionName()
    {
        return "If Action";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var condition = executionContext.ReadVariable<bool>("Condition");

        if (condition)
        {
            executionContext.ExecuteActionBuffer("True Actions");
        }
        else
        {
            executionContext.ExecuteActionBuffer("False Actions");
        }

        return ExecutionActionResult.Successful;
    }
}