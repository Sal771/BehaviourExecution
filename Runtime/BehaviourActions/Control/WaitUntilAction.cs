using com.Sal77.BehaviourExecution;

[BehaviourCategory("Control/Wait Until")]
public class WaitUntilAction : BehaviourAction
{
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        actionContext.DeclareVariable<bool>("Condition Boolean", IBehaviourActionReadMode.Input);
        actionContext.DeclareActionBuffer("Pool Actions");
    }

    protected override string GetActionName()
    {
        return "Wait Until";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var conditionBoolean = executionContext.ReadVariable<bool>("Condition Boolean");

        executionContext.ExecuteActionBuffer("Pool Actions");

        if (conditionBoolean)
        {
            return ExecutionActionResult.Successful;
        }

        return ExecutionActionResult.Waiting;
    }
    public override bool WaitCondition(IBehaviourExecution executionContext)
    {
        executionContext.ExecuteActionBuffer("Pool Actions");

        var conditionBoolean = executionContext.ReadVariable<bool>("Condition Boolean");

        return conditionBoolean;
    }
}