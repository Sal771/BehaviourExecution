using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourCategory("Misc/Debug Log")]
public class DebugLogAction : BehaviourAction
{
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        actionContext.DeclareVariable<string>("Text");
    }

    protected override string GetActionName()
    {
        return "Debug Log";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var text = executionContext.ReadVariable<string>("Text");

        Debug.Log(text);

        return ExecutionActionResult.Successful;
    }
}