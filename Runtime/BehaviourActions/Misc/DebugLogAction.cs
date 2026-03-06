using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourCategory("Misc/Debug Log")]
public class DebugLogAction : BehaviourAction
{
    private BehaviourBinding m_textReference;
    public override void DefineBindings(IBehaviourVariable actionContext)
    {
        m_textReference = actionContext.DeclareVariable<string>("Text");
    }

    protected override string GetActionName()
    {
        return "Debug Log";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var text = executionContext.ReadVariable<string>(m_textReference);

        Debug.Log(text);

        return ExecutionActionResult.Successful;
    }
}