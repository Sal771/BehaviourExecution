using com.Sal77.BehaviourExecution;

[BehaviourCategory("Control/Wait Until")]
public class WaitUntilAction : BehaviourAction
{
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        actionContext.DeclareVariable<bool>("Condition Boolean", IBehaviourActionReadMode.Input);
    }

    protected override string GetActionName()
    {
        return "Wait Until";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        executionContext.WriteVariable<bool>("Condition Boolean", false);

        return ExecutionActionResult.Waiting;
    }
    public override bool WaitCondition(IBehaviourExecution executionContext)
    {
        var pooledBoolean = executionContext.ReadVariable<bool>("Condition Boolean");

        return pooledBoolean;
    }
}