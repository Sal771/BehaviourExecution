using com.Sal77.BehaviourExecution;

[BehaviourCategory("Control/For Cycle")]
public class ForCycleAction : BehaviourAction
{
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        actionContext.DeclareVariable<int>("Iteration Count", IBehaviourActionReadMode.Input);
        actionContext.DeclareActionBuffer("Loop Actions");
    }

    protected override string GetActionName()
    {
        return "For Cycle";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)//TODO, tweak a bit the waiting timings
    {
        int iterations = executionContext.ReadVariable<int>("Iteration Count");

        if(iterations > 0)
        {
            executionContext.ExecuteActionBuffer("Loop Actions");
            executionContext.WriteVariable<int>("Iteration Count", iterations-1);
        }

        if(iterations > 0)
        {
            return ExecutionActionResult.Waiting;
        }
        else
        {
            return ExecutionActionResult.Successful;
        }
    }

    public override bool WaitCondition(IBehaviourExecution executionContext)
    {
        int iterations = executionContext.ReadVariable<int>("Iteration Count");

        if(iterations > 0)
        {
            executionContext.ExecuteActionBuffer("Loop Actions");
            executionContext.WriteVariable<int>("Iteration Count", iterations-1);
        }

        if(iterations > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
