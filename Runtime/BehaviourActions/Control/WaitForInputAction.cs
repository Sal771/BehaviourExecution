using UnityEngine.InputSystem;
using com.Sal77.BehaviourExecution;
using System;

[BehaviourCategory("Control/Wait For Input")]
public class WaitForInputAction : BehaviourAction
{
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        actionContext.DeclareVariable<InputAction>("Input Action");
    }

    protected override string GetActionName()
    {
        return "Wait For Input";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        throw new NotImplementedException();

        return ExecutionActionResult.Waiting;
    }

    public override bool WaitCondition(IBehaviourExecution executionContext)
    {
        var inputAction = executionContext.ReadVariable<InputAction>("Input Action");
        
        if (inputAction != null)
        {
            return inputAction.WasPerformedThisFrame();
        }
        
        return false;
    }
}
