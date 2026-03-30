using com.Sal77.BehaviourExecution;

[BehaviourCategory("Control/While Cycle")]
public class WhileCycleAction : BehaviourAction
{
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        actionContext.DeclareVariable<bool>("Condition", IBehaviourActionReadMode.Input);
        actionContext.DeclareActionBuffer("Loop Actions");
    }

    protected override string GetActionName()
    {
        return "While Cycle";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        bool condition = executionContext.ReadVariable<bool>("Condition");

        if(condition)
        {
            executionContext.ExecuteActionBuffer("Loop Actions");
        }

        if(condition)
        {
            return ExecutionActionResult.Successful;
        }
        else
        {
            return ExecutionActionResult.Waiting;
        }
    }
    public override bool WaitCondition(IBehaviourExecution executionContext)
    {
        bool condition = executionContext.ReadVariable<bool>("Condition");

        if(condition)
        {
            executionContext.ExecuteActionBuffer("Loop Actions");
        }

        if(condition)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}