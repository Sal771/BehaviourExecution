using com.Sal77.BehaviourExecution;

[BehaviourCategory("Control/Wait Seconds")]
public class WaitUntilAction : BehaviourAction
{
    private BehaviourBinding m_conditionBooleanReference;
    public override void DefineBindings(IBehaviourVariable actionContext)
    {
        m_conditionBooleanReference = actionContext.DeclareVariable<bool>("ConditionBoolean");
    }

    protected override string GetActionName()
    {
        return "Wait Seconds";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        executionContext.WriteVariable<bool>(m_conditionBooleanReference, false);

        return ExecutionActionResult.Waiting;
    }
    public override bool WaitCondition(IBehaviourExecution executionContext)
    {
        var pooledBoolean = executionContext.ReadVariable<bool>(m_conditionBooleanReference);

        return pooledBoolean;
    }
}