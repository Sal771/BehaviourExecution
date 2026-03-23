using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourCategory("Control/Wait Seconds")]
public class WaitSecondsAction : BehaviourAction
{
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        actionContext.DeclareVariable<float>("WaitSeconds");
        actionContext.DeclareVariable<float>("WaitSecondsLeft");
    }

    protected override string GetActionName()
    {
        return "Wait Seconds";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var secondsToWait = executionContext.ReadVariable<float>("WaitSeconds");

        executionContext.WriteVariable<float>("WaitSecondsLeft", secondsToWait);

        return ExecutionActionResult.Waiting;
    }
    public override bool WaitCondition(IBehaviourExecution executionContext)
    {
        var secondsToWait = executionContext.ReadVariable<float>("WaitSecondsLeft");

        if(secondsToWait <= 0){
            return true;
        }

        executionContext.WriteVariable<float>("WaitSecondsLeft", secondsToWait - Time.deltaTime);
        return false;
    }
}