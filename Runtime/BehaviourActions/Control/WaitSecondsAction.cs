using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourCategory("Control/Wait Seconds")]
public class WaitSecondsAction : BehaviourAction
{
    private BehaviourBinding m_waitSecondsReference;
    private BehaviourBinding m_waitSecondsLeftReference;
    public override void DefineBindings(IBehaviourVariable actionContext)
    {
        m_waitSecondsReference = actionContext.DeclareVariable<float>("WaitSeconds");
        m_waitSecondsLeftReference = actionContext.DeclareVariable<float>("WaitSecondsLeft");
    }

    protected override string GetActionName()
    {
        return "Wait Seconds";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var secondsToWait = executionContext.ReadVariable<float>(m_waitSecondsReference);

        executionContext.WriteVariable<float>(m_waitSecondsLeftReference, secondsToWait);

        return ExecutionActionResult.Waiting;
    }
    public override bool WaitCondition(IBehaviourExecution executionContext)
    {
        var secondsToWait = executionContext.ReadVariable<float>(m_waitSecondsLeftReference);

        if(secondsToWait <= 0){
            return true;
        }

        executionContext.WriteVariable<float>(m_waitSecondsLeftReference, secondsToWait-Time.deltaTime);
        return false;;
    }
}