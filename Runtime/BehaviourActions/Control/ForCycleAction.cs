using com.Sal77.BehaviourExecution;
using UnityEngine;

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

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        int iterationCount = executionContext.ReadVariable<int>("Iteration Count");

        for(int i=0; i<iterationCount; i++)
        {
            executionContext.ExecuteActionBuffer("Loop Actions");
        }

        return ExecutionActionResult.Successful;
    }
}
